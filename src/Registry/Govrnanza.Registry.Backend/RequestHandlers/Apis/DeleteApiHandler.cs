using Govrnanza.Registry.Backend.Infrastructure.Database;
using Govrnanza.Registry.Backend.Requests.Apis;
using Govrnanza.Registry.Backend.Responses;
using Govrnanza.Registry.Core.Model;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Govrnanza.Registry.Backend.RequestHandlers.Apis
{
    public class DeleteApiHandler : IAsyncRequestHandler<DeleteApi, Response<DeleteResult>>
    {
        private readonly RegistryDbContext _context;

        public DeleteApiHandler(RegistryDbContext context)
        {
            _context = context;
        }

        public async Task<Response<DeleteResult>> Handle(DeleteApi request)
        {
            var api = await _context.Apis.FirstOrDefaultAsync(x => x.Name.Equals(request.ApiName));
            if (api == null)
                return new Response<DeleteResult>(DeleteResult.NotFound);

            await DeleteTagsOfApiAsync(api);
            _context.Apis.Remove(api);
            await _context.SaveChangesAsync();

            return new Response<DeleteResult>(DeleteResult.Deleted);
        }

        private async Task DeleteTagsOfApiAsync(Api api)
        {
            var apiTags = await _context.ApiTags.Where(x => x.Api.Name.Equals(api.Name)).ToListAsync();
            _context.ApiTags.RemoveRange(apiTags);
        }
    }
}
