using Govrnanza.Registry.Backend.Infrastructure.Database;
using Govrnanza.Registry.Backend.Requests.Tags;
using Govrnanza.Registry.Backend.Responses;
using Govrnanza.Registry.Core.Model;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Govrnanza.Registry.Backend.RequestHandlers.Tags
{
    public class DeleteTagHandler : IAsyncRequestHandler<DeleteTag, Response<DeleteResult>>
    {
        private readonly RegistryDbContext _context;

        public DeleteTagHandler(RegistryDbContext context)
        {
            _context = context;
        }

        public async Task<Response<DeleteResult>> Handle(DeleteTag request)
        {
            if (request.ForceDelete && !string.IsNullOrEmpty(request.ApiName))
                return new Response<DeleteResult>(DeleteResult.InvalidRequest, "Cannot use ForceDelete with an the ApiName property");

            if (!string.IsNullOrEmpty(request.ApiName))
                return await DeleteTagsOfApiAsync(request);

            return await DeleteTagAsync(request);
        }

        private async Task<Response<DeleteResult>> DeleteTagsOfApiAsync(DeleteTag request)
        {
            var api = await _context.Apis.FirstOrDefaultAsync(x => x.Name.Equals(request.ApiName));
            if (api == null)
                return new Response<DeleteResult>(DeleteResult.NotFound, "No API with the supplied nbame could be found");

            var apiTags = _context.ApiTags.AsQueryable();
            if (!string.IsNullOrEmpty(request.TagName))
            {
                apiTags = apiTags.Where(x => x.Tag.Name.Equals(request.TagName));
            }

            var apiTagsToDelete = await apiTags.ToListAsync();
            _context.ApiTags.RemoveRange(apiTagsToDelete);

            await _context.SaveChangesAsync();

            return new Response<DeleteResult>(DeleteResult.Deleted);
        }

        private async Task<Response<DeleteResult>> DeleteTagAsync(DeleteTag request)
        {
            var apiTags = await _context.ApiTags.Where(x => x.Tag.Name.Equals(request.TagName)).ToListAsync();
            _context.ApiTags.RemoveRange(apiTags);

            if(request.ForceDelete)
            {
                var tag = await _context.Tags.SingleOrDefaultAsync(x => x.Name.Equals(request.ApiName));
                if (tag == null)
                    return new Response<DeleteResult>(DeleteResult.NotFound);

                _context.Tags.Remove(tag);
            }

            await _context.SaveChangesAsync();

            return new Response<DeleteResult>(DeleteResult.Deleted);
        }
    }
}
