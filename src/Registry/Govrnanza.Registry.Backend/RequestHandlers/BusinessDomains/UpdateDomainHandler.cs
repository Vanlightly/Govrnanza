using Govrnanza.Registry.Backend.Infrastructure.Database;
using Govrnanza.Registry.Backend.Requests.BusinessDomains;
using Govrnanza.Registry.Backend.Responses;
using Govrnanza.Registry.Core.Model;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Govrnanza.Registry.Backend.RequestHandlers.BusinessDomains
{
    public class UpdateDomainHandler : IAsyncRequestHandler<UpdateDomain, Response<UpdateResult>>
    {
        private readonly RegistryDbContext _context;

        public UpdateDomainHandler(RegistryDbContext context)
        {
            _context = context;
        }

        public async Task<Response<UpdateResult>> Handle(UpdateDomain request)
        {
            var dbDomain = await _context.BusinessDomains
                .Include(x => x.SubDomains)
                .FirstOrDefaultAsync(x => x.Name.Equals(request.Name));

            if (dbDomain == null)
            {
                return new Response<UpdateResult>(UpdateResult.NotFound);
            }
            else
            {
                if (!string.IsNullOrEmpty(request.NewName))
                    dbDomain.Name = request.NewName;

                if (request.UpdatedSubDomains != null)
                {
                    // new list to allow iteration over changing collection
                    foreach (var dbSubDomain in new List<BusinessSubDomain>(dbDomain.SubDomains))
                    {
                        var subDomain = request.UpdatedSubDomains.FirstOrDefault(x => x.Name == dbSubDomain.Name);
                        if (subDomain == null)
                        {
                            dbDomain.SubDomains.Remove(dbSubDomain);
                        }
                        else
                        {
                            if(!string.IsNullOrEmpty(subDomain.NewName))
                                dbSubDomain.Name = subDomain.NewName;

                            if (!string.IsNullOrEmpty(subDomain.NewDescription))
                                dbSubDomain.Description = subDomain.NewDescription;
                        }
                    }

                    foreach (var subDomain in request.UpdatedSubDomains)
                    {
                        var dbSubDomain = dbDomain.SubDomains.FirstOrDefault(x => x.Name == subDomain.Name);
                        if (dbSubDomain == null)
                        {
                            dbDomain.SubDomains.Add(new BusinessSubDomain()
                            {
                                Name = subDomain.Name,
                                Description = subDomain.NewDescription,
                                ParentId = dbDomain.Id
                            });
                        }
                    }
                }

                await _context.SaveChangesAsync();

                return new Response<UpdateResult>(UpdateResult.Updated);
            }
        }
    }
}
