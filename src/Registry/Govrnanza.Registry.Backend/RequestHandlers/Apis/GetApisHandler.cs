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
    public class GetApisHandler : IAsyncRequestHandler<GetApis, Response<IEnumerable<Api>>>
    {
        private readonly RegistryDbContext _context;

        public GetApisHandler(RegistryDbContext context)
        {
            _context = context;
        }

        public async Task<Response<IEnumerable<Api>>> Handle(GetApis request)
        {
            var apis = await _context.Apis
                        .Include(x => x.BusinessSubDomain)
                        .Include(x => x.Versions)
                        .Include(x => x.ApiTags)
                            .ThenInclude(x => x.Tag)
                        .ToListAsync();

            return new Response<IEnumerable<Api>>(apis);
        }
    }
}
