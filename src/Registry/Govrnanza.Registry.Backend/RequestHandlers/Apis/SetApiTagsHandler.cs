
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
    public class SetApiTagsHandler : IAsyncRequestHandler<SetApiTags, Response<UpdateResult>>
    {
        private readonly RegistryDbContext _context;

        public SetApiTagsHandler(RegistryDbContext context)
        {
            _context = context;
        }

        public async Task<Response<UpdateResult>> Handle(SetApiTags request)
        {
            Api dbApi = await GetApiAsync(request);
            if (dbApi == null)
            {
                return new Response<UpdateResult>(UpdateResult.NotFound);
            }
            else
            {
                var dbApiTags = await _context.ApiTags
                    .Include(x => x.Tag)
                    .Where(x => x.ApiId.Equals(dbApi.Id)).ToListAsync();

                // add new tags
                foreach (var incomingTag in request.Tags)
                {
                    if (!dbApiTags.Select(x => x.Tag.Name).Contains(incomingTag))
                    {
                        var existingTag = await _context.Tags.FirstOrDefaultAsync(x => x.Name.Equals(incomingTag));
                        if (existingTag == null)
                        {
                            existingTag = new Tag() { Id = Guid.NewGuid(), Name = incomingTag };
                            await _context.Tags.AddAsync(existingTag);
                        }

                        await _context.ApiTags.AddAsync(new ApiTag()
                        {
                            Id = Guid.NewGuid(),
                            ApiId = dbApi.Id,
                            TagId = existingTag.Id,
                            Tag = existingTag
                        });
                    }
                }

                // remove tags not supplied 
                foreach (var dbApiTag in new List<ApiTag>(dbApiTags))
                {
                    var incomingTag = request.Tags.FirstOrDefault(x => dbApiTag.Tag.Name.Equals(x));
                    if (incomingTag == null)
                        _context.Remove(dbApiTag);
                }

                if(request.Commit)
                    await _context.SaveChangesAsync();

                return new Response<UpdateResult>(UpdateResult.Updated);
            }
        }

        private async Task<Api> GetApiAsync(SetApiTags request)
        {
            if (request.UseLocal)
            {
                var localCopy = _context.Apis.Local.FirstOrDefault(x => x.Name.Equals(request.ApiName));
                return localCopy;
            }
            else
                return await _context.Apis.FirstOrDefaultAsync(x => x.Name.Equals(request.ApiName));
        }
    }
}
