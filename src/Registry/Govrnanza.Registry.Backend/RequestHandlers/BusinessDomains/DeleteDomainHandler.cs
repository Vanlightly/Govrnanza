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
    public class DeleteDomainHandler : IAsyncRequestHandler<DeleteDomain, Response<DeleteResult>>
    {
        private readonly RegistryDbContext _context;

        public DeleteDomainHandler(RegistryDbContext context)
        {
            _context = context;
        }

        public async Task<Response<DeleteResult>> Handle(DeleteDomain request)
        {
            var domain = await _context.BusinessDomains
                            .Include(x => x.SubDomains)
                            .FirstOrDefaultAsync(x => x.Name.Equals(request.Name));

            if (domain == null)
                return new Response<DeleteResult>(DeleteResult.NotFound);

            if (domain.SubDomains.Any())
            {
                return new Response<DeleteResult>(DeleteResult.NotDeletedDueToDependentObjects,
                    "The business domain cannot be deleted because it has child sub domains");
            }

            _context.BusinessDomains.Remove(domain);
            await _context.SaveChangesAsync();

            return new Response<DeleteResult>(DeleteResult.Deleted);
        }
    }
}
