using Govrnanza.Registry.Backend.Infrastructure.Database;
using Govrnanza.Registry.Backend.Requests.BusinessDomains;
using Govrnanza.Registry.Backend.Responses;
using Govrnanza.Registry.Core.Model;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Govrnanza.Registry.Backend.RequestHandlers.BusinessDomains
{
    public class GetSubDomainHandler : IAsyncRequestHandler<GetSubDomain, GetResponse<BusinessSubDomain>>,
        IAsyncRequestHandler<GetSubDomains, Response<IEnumerable<BusinessSubDomain>>>
    {
        private readonly RegistryDbContext _context;

        public GetSubDomainHandler(RegistryDbContext context)
        {
            _context = context;
        }

        public async Task<Response<IEnumerable<BusinessSubDomain>>> Handle(GetSubDomains request)
        {
            var businessSubDomains = await _context.BusinessSubDomains
                .Include(x => x.Parent)
                .ToListAsync();

            return new Response<IEnumerable<BusinessSubDomain>>(businessSubDomains);
        }

        public async Task<GetResponse<BusinessSubDomain>> Handle(GetSubDomain request)
        {
            var subDomain = await _context.BusinessSubDomains
                .Include(x => x.Parent)
                .FirstOrDefaultAsync(x => x.Name.Equals(request.Name));

            if (subDomain == null)
                return new GetResponse<BusinessSubDomain>() { Result = GetResult.NotFound };

            return new GetResponse<BusinessSubDomain>()
            {
                Result = GetResult.Retrieved,
                Data = subDomain
            };
        }
    }
}
