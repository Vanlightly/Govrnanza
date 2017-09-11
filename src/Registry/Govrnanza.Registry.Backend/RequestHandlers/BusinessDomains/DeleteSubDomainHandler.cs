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
    public class DeleteSubDomainHandler : IAsyncRequestHandler<DeleteSubDomain, Response<DeleteResult>>
    {
        private readonly RegistryDbContext _context;

        public DeleteSubDomainHandler(RegistryDbContext context)
        {
            _context = context;
        }

        public async Task<Response<DeleteResult>> Handle(DeleteSubDomain request)
        {
            var subDomain = await _context.BusinessSubDomains
                            .FirstOrDefaultAsync(x => x.Name.Equals(request.Name));

            if (subDomain == null)
                return new Response<DeleteResult>(DeleteResult.NotFound);

            var hasDependentApis = _context.Apis.Any(x => x.BusinessSubDomainId.Equals(subDomain.Id));
            if(hasDependentApis)
            {
                return new Response<DeleteResult>(DeleteResult.NotDeletedDueToDependentObjects,
                    "The business sub domain cannot be deleted because it has APIs associated to it.");
            }

            _context.BusinessSubDomains.Remove(subDomain);
            await _context.SaveChangesAsync();

            return new Response<DeleteResult>(DeleteResult.Deleted);
        }
    }
}
