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
    public class CreateSubDomainHandler : IAsyncRequestHandler<CreateSubDomain, Response<CreateResult>>
    {
        private readonly RegistryDbContext _context;

        public CreateSubDomainHandler(RegistryDbContext context)
        {
            _context = context;
        }

        public async Task<Response<CreateResult>> Handle(CreateSubDomain request)
        {
            var businessDomain = await _context.BusinessDomains.SingleOrDefaultAsync(x => x.Name.Equals(request.ParentDomainName));
            if (businessDomain == null)
                return new Response<CreateResult>(CreateResult.DependentObjectNotFound);

            var businessSubDomain = new BusinessSubDomain()
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Description = request.Description,
                ParentId = businessDomain.Id
            };

            await _context.AddAsync(businessSubDomain);
            await _context.SaveChangesAsync();

            return new Response<CreateResult>(CreateResult.Created);
        }
    }
}
