using Govrnanza.Registry.WebApi.ServiceContracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Govrnanza.Registry.WebApi.Model;
using Govrnanza.Registry.WebApi.ServiceContracts.Responses;
using Govrnanza.Registry.WebApi.Model.Internal;

namespace Govrnanza.Registry.WebApi.Infrastructure.Database
{
    public class BusinessDomainsService : IBusinessDomainsService
    {
        private readonly RegistryDbContext _context;

        public BusinessDomainsService(RegistryDbContext context)
        {
            _context = context;
        }

        public async Task AddDomainAsync(BusinessDomain businessDomain)
        {
            await _context.AddAsync(businessDomain);

            foreach (var subDomain in businessDomain?.SubDomains)
            {
                subDomain.ParentId = businessDomain.Id;
                await _context.AddAsync(subDomain);
            }

            await _context.SaveChangesAsync();
        }

        public async Task AddSubDomainAsync(BusinessSubDomain businessSubDomain)
        {
            await _context.AddAsync(businessSubDomain);
            await _context.SaveChangesAsync();
        }

        public async Task<DeleteResult> DeleteDomainAsync(string domainName)
        {
            var domain = await _context.BusinessDomains.FirstOrDefaultAsync(x => x.Name.Equals(domainName)); 

            if (domain == null)
                return DeleteResult.NotFound;

            _context.BusinessDomains.Remove(domain);
            await _context.SaveChangesAsync();

            return DeleteResult.Deleted;
        }

        public async Task<DeleteResult> DeleteSubDomainAsync(string subDomainName)
        {
            var subDomain = await _context.BusinessSubDomains.FirstOrDefaultAsync(x => x.Name.Equals(subDomainName));

            if (subDomain == null)
                return DeleteResult.NotFound;

            _context.BusinessSubDomains.Remove(subDomain);
            await _context.SaveChangesAsync();

            return DeleteResult.Deleted;
        }

        public async Task<GetPayload<BusinessDomain>> GetDomainAsync(string domainName)
        {
            var domain = await _context.BusinessDomains
                .Include(x => x.SubDomains)
                .FirstOrDefaultAsync(x => x.Name.Equals(domainName));

            if (domain == null)
                return new GetPayload<BusinessDomain>() { Result = GetResult.NotFound };

            return new GetPayload<BusinessDomain>()
            {
                Result = GetResult.Retrieved,
                Data = domain
            };
        }

        public async Task<IEnumerable<BusinessDomain>> GetDomainsAsync()
        {
            return await _context.BusinessDomains
                .Include(x => x.SubDomains)
                .ToListAsync();
        }

        public async Task<IEnumerable<BusinessSubDomain>> GetSubDomainsAsync()
        {
            return await _context.BusinessSubDomains
                .Include(x => x.Parent)
                .ToListAsync();
        }

        public async Task<GetPayload<BusinessSubDomain>> GetSubDomainAsync(string subDomainName)
        {
            var subDomain = await _context.BusinessSubDomains
                .Include(x => x.Parent)
                .FirstOrDefaultAsync(x => x.Name.Equals(subDomainName));

            if (subDomain == null)
                return new GetPayload<BusinessSubDomain>() { Result = GetResult.NotFound };

            return new GetPayload<BusinessSubDomain>()
            {
                Result = GetResult.Retrieved,
                Data = subDomain
            };
        }

        public async Task<UpdateResult> UpdateDomainAsync(BusinessDomain businessDomain, bool insertIfNotExists)
        {
            var dbDomain = await _context.BusinessDomains
                .Include(x => x.SubDomains)
                .FirstOrDefaultAsync(x => x.Name.Equals(businessDomain.Name));

            if (dbDomain == null)
            {
                if (insertIfNotExists)
                {
                    await _context.BusinessDomains.AddAsync(businessDomain);
                    await _context.SaveChangesAsync();
                    return UpdateResult.Inserted;
                }
                else
                    return UpdateResult.NotFound;
            }
            else
            {
                // we would only ever update some fields, so we do this with some manual logic
                dbDomain.Name = businessDomain.Name;
                // new list to allow iteration over changing collection
                foreach (var dbSubDomain in new List<BusinessSubDomain>(dbDomain.SubDomains))
                {
                    var subDomain = businessDomain.SubDomains.FirstOrDefault(x => x.Name == dbSubDomain.Name);
                    if (subDomain == null)
                    {
                        dbDomain.SubDomains.Remove(dbSubDomain);
                    }
                    else
                    {
                        dbSubDomain.Name = subDomain.Name;
                        dbSubDomain.Description = subDomain.Description;
                    }
                }

                foreach(var subDomain in businessDomain.SubDomains)
                {
                    var dbSubDomain = dbDomain.SubDomains.FirstOrDefault(x => x.Name == subDomain.Name);
                    if (dbSubDomain == null)
                    {
                        dbDomain.SubDomains.Add(new BusinessSubDomain()
                        {
                            Name = subDomain.Name,
                            Description = subDomain.Description,
                            ParentId = dbDomain.Id
                        });
                    }
                }

                await _context.SaveChangesAsync();

                return UpdateResult.Updated;
            }
        }

        public async Task<UpdateResult> UpdateSubDomainAsync(BusinessSubDomain businessSubDomain, bool insertIfNotExists)
        {
            var dbSubDomain = await _context.BusinessSubDomains.FirstOrDefaultAsync(x => x.Name.Equals(businessSubDomain.Name));

            if (dbSubDomain == null)
            {
                if (insertIfNotExists)
                {
                    await _context.BusinessSubDomains.AddAsync(businessSubDomain);
                    await _context.SaveChangesAsync();
                    return UpdateResult.Inserted;
                }
                else
                    return UpdateResult.NotFound;
            }
            else
            {
                // not all values are updated, such as some dates
                dbSubDomain.Name = businessSubDomain.Name;
                dbSubDomain.Description = businessSubDomain.Description;

                if (businessSubDomain.ParentId != Guid.Empty)
                    dbSubDomain.ParentId = businessSubDomain.ParentId;

                await _context.SaveChangesAsync();

                return UpdateResult.Updated;
            }
        }
    }
}
