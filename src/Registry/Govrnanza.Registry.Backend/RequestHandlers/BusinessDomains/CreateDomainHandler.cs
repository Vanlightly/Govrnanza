using Govrnanza.Registry.Backend.Infrastructure.Database;
using Govrnanza.Registry.Backend.Requests.BusinessDomains;
using Govrnanza.Registry.Backend.Responses;
using Govrnanza.Registry.Core.Model;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Govrnanza.Registry.Backend.RequestHandlers.BusinessDomains
{
    public class CreateDomainHandler : IAsyncRequestHandler<CreateDomain, Response<CreateResult>>
    {
        private readonly RegistryDbContext _context;

        public CreateDomainHandler(RegistryDbContext context)
        {
            _context = context;
        }

        public async Task<Response<CreateResult>> Handle(CreateDomain request)
        {
            var businessDomainId = Guid.NewGuid();

            var businessDomain = new BusinessDomain()
            {
                Id = businessDomainId,
                Name = request.Name,
                Description = request.Description,
                SubDomains = request.SubDomains.Select(x => new BusinessSubDomain()
                {
                    Id = Guid.NewGuid(),
                    Description = x.Description,
                    Name = x.Name,
                    ParentId = businessDomainId
                }).ToList()
            };

            await _context.AddAsync(businessDomain);
            await _context.SaveChangesAsync();

            return new Response<CreateResult>(CreateResult.Created);
        }
    }
}
