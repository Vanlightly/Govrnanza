using Govrnanza.Registry.Backend.Infrastructure.Database;
using Govrnanza.Registry.Backend.Requests.Apis;
using Govrnanza.Registry.Backend.Responses;
using Govrnanza.Registry.Core.Model;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Govrnanza.Registry.Backend.RequestHandlers.Apis
{
    public class GetApiHandler : IAsyncRequestHandler<GetApi, GetResponse<Api>>
    {
        private readonly RegistryDbContext _context;

        public GetApiHandler(RegistryDbContext context)
        {
            _context = context;
        }

        public async Task<GetResponse<Api>> Handle(GetApi request)
        {
            var api = await _context.Apis
                        .Include(x => x.BusinessSubDomain)
                        .Include(x => x.Versions)
                        .Include(x => x.ApiTags)
                        .FirstOrDefaultAsync(x => x.Name.Equals(request.ApiName));

            if (api == null)
                return new GetResponse<Api>() { Result = GetResult.NotFound };

            return new GetResponse<Api>()
            {
                Result = GetResult.Retrieved,
                Data = api
            };
        }
    }
}
