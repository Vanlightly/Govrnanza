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
    public class UpdateSubDomainHandler : IAsyncRequestHandler<UpdateSubDomain, Response<UpdateResult>>
    {
        private readonly RegistryDbContext _context;

        public UpdateSubDomainHandler(RegistryDbContext context)
        {
            _context = context;
        }

        public async Task<Response<UpdateResult>> Handle(UpdateSubDomain request)
        {
            var dbSubDomain = await _context.BusinessSubDomains
                .FirstOrDefaultAsync(x => x.Name.Equals(request.Name));

            if (dbSubDomain == null)
            {
                return new Response<UpdateResult>(UpdateResult.NotFound);
            }
            else
            {
                if (!string.IsNullOrEmpty(request.NewName))
                    dbSubDomain.Name = request.NewName;

                if (!string.IsNullOrEmpty(request.NewDescription))
                    dbSubDomain.Description = request.NewDescription;

                await _context.SaveChangesAsync();
                
                return new Response<UpdateResult>(UpdateResult.Updated);
            }
        }
    }
}
