using Govrnanza.Registry.Backend.Infrastructure.Database;
using Govrnanza.Registry.Backend.Requests.Apis;
using Govrnanza.Registry.Backend.Responses;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Govrnanza.Registry.Backend.RequestHandlers.Apis
{
    public class MoveApiToSubDomainHandler : IAsyncRequestHandler<MoveApiToSubDomain, Response<MoveResult>>
    {
        private readonly RegistryDbContext _context;

        public MoveApiToSubDomainHandler(RegistryDbContext context)
        {
            _context = context;
        }

        public async Task<Response<MoveResult>> Handle(MoveApiToSubDomain request)
        {
            var api = await _context.Apis.FirstOrDefaultAsync(x => x.Name.Equals(request.ApiName));
            if (api == null)
                return new Response<MoveResult>(MoveResult.NotFound);

            var currentSubDomain = await _context.BusinessSubDomains.SingleOrDefaultAsync(x => x.Name.Equals(request.CurrentBusinessSubDomainName));
            if(currentSubDomain == null)
                return new Response<MoveResult>(MoveResult.NotFound, "The current business sub domain does not exist");

            var destinationSubDomain = await _context.BusinessSubDomains.SingleOrDefaultAsync(x => x.Name.Equals(request.DestinationBusinessSubDomainName));
            if (destinationSubDomain == null)
                return new Response<MoveResult>(MoveResult.NotFound, "The destination business sub domain does not exist");

            if (!currentSubDomain.Id.Equals(api.BusinessSubDomainId))
                return new Response<MoveResult>(MoveResult.NotMoved, "The current sub domain does not match the existing API sub domain");
                        
            api.BusinessSubDomainId = destinationSubDomain.Id;
            await _context.SaveChangesAsync();

            return new Response<MoveResult>(MoveResult.Moved);
        }
    }
}
