using Govrnanza.Registry.WebApi.ServiceContracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Govrnanza.Registry.WebApi.Model.Internal;
using Govrnanza.Registry.WebApi.ServiceContracts.Responses;

namespace Govrnanza.Registry.WebApi.Infrastructure.Database
{
    public class ApiService : IApiService
    {
        private readonly RegistryDbContext _context;

        public ApiService(RegistryDbContext context)
        {
            _context = context;
        }

        public async Task AddApiAsync(Api api)
        {
            await _context.Apis.AddAsync(api);
            await _context.SaveChangesAsync();
        }

        public async Task<DeleteResult> DeleteApiAsync(string apiName)
        {
            var api = await _context.Apis.FirstOrDefaultAsync(x => x.Name.Equals(apiName));
            if (api == null)
                return DeleteResult.NotFound;

            _context.Apis.Remove(api);
            await _context.SaveChangesAsync();

            return DeleteResult.Deleted;
        }

        public async Task<GetPayload<Api>> GetApiAsync(string name)
        {
            var api = await _context.Apis
                        .Include(x => x.BusinessSubDomain)
                        .FirstOrDefaultAsync(x => x.Name.Equals(name));

            if (api == null)
                return new GetPayload<Api>() { Result = GetResult.NotFound };
            
            return new GetPayload<Api>()
            {
                Result = GetResult.Retrieved,
                Data = api
            };
        }

        public async Task<IEnumerable<Api>> GetApisAsync()
        {
            return await _context.Apis
                .Include(x => x.BusinessSubDomain)
                .ToListAsync();
        }

        public async Task<UpdateResult> UpdateApiAsync(Api api, bool insertIfNotExists)
        {
            var dbApi = await _context.Apis
                            .Include(x => x.BusinessSubDomain)
                            .FirstOrDefaultAsync(x => x.Name.Equals(api.Name));
            if (dbApi == null)
            {
                if (insertIfNotExists)
                {
                    await _context.Apis.AddAsync(api);
                    await _context.SaveChangesAsync();
                    return UpdateResult.Inserted;
                }
                else
                {
                    return UpdateResult.NotFound;
                }
            }
            else
            {
                
                dbApi.Name = api.Name;
                dbApi.Description = api.Description;
                dbApi.BusinessSubDomainId = api.BusinessSubDomainId;
                
                await _context.SaveChangesAsync();

                return UpdateResult.Updated;
            }

        }
    }
}
