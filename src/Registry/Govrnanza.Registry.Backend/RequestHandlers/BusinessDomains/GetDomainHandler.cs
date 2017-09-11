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
    public class GetDomainHandler : IAsyncRequestHandler<GetDomain, GetResponse<BusinessDomain>>,
        IAsyncRequestHandler<GetDomains, Response<IEnumerable<BusinessDomain>>>
    {
        private readonly RegistryDbContext _context;

        public GetDomainHandler(RegistryDbContext context)
        {
            _context = context;
        }

        public async Task<Response<IEnumerable<BusinessDomain>>> Handle(GetDomains request)
        {
            var businessDomains = await _context.BusinessDomains
                .Include(x => x.SubDomains)
                .ToListAsync();

            return new Response<IEnumerable<BusinessDomain>>(businessDomains);
        }

        public async Task<GetResponse<BusinessDomain>> Handle(GetDomain request)
        {
            var domain = await _context.BusinessDomains
                .Include(x => x.SubDomains)
                .FirstOrDefaultAsync(x => x.Name.Equals(request.Name));

            if (domain == null)
                return new GetResponse<BusinessDomain>() { Result = GetResult.NotFound };

            return new GetResponse<BusinessDomain>()
            {
                Result = GetResult.Retrieved,
                Data = domain
            };
        }
    }
}
