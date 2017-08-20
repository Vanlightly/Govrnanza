using Govrnanza.Registry.WebApi.ServiceContracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Govrnanza.Registry.WebApi.Model.Internal;
using Govrnanza.Registry.WebApi.ServiceContracts.Responses;

namespace Govrnanza.Registry.WebApi.Infrastructure.Database
{
    public class TagService : ITagService
    {
        private readonly RegistryDbContext _context;

        public TagService(RegistryDbContext context)
        {
            _context = context;
        }

        public async Task<UpdateResult> AddTagToApiAsync(string apiName, string tagName)
        {
            var apiTag = new ApiTag();

            var api = await _context.Apis.FirstOrDefaultAsync(x => x.Name.Equals(apiName));
            if (api == null)
                return UpdateResult.NotFound;

            apiTag.Api = api;
            apiTag.ApiId = api.Id;

            var tag = await _context.Tags.FirstOrDefaultAsync(x => x.Name.Equals(tagName));
            if (tag == null)
                tag = new Tag() { Id = Guid.NewGuid(), Name = tagName };

            apiTag.TagId = tag.Id;
            apiTag.Tag = tag;

            await _context.ApiTags.AddAsync(apiTag);
            await _context.SaveChangesAsync();

            return UpdateResult.Inserted;
        }

        public async Task<DeleteResult> DeleteTagsOfApiAsync(string apiName, IEnumerable<string> tags)
        {
            var api = await _context.Apis.FirstOrDefaultAsync(x => x.Name.Equals(apiName));
            if (api == null)
                return DeleteResult.NotFound;

            var apiTags = await _context.ApiTags.Where(x => tags.Contains(x.Tag.Name)).ToListAsync();
            _context.ApiTags.RemoveRange(apiTags);
            await _context.SaveChangesAsync();

            return DeleteResult.Deleted;
        }

        public async Task<IEnumerable<ApiTag>> GetApiTagsAsync()
        {
            return await _context.ApiTags
                .Include(x => x.Api)
                .Include(x => x.Tag)
                .ToListAsync();
        }

        public async Task<IEnumerable<Tag>> GetTagsAsync()
        {
            return await _context.Tags.ToListAsync();
        }

        public async Task<IEnumerable<Tag>> GetTagsAsync(IEnumerable<string> tags)
        {
            return await _context.Tags.Where(x => tags.Contains(x.Name)).ToListAsync();
        }

        public async Task<IEnumerable<Tag>> GetTagsOfApiAsync(string apiName)
        {
            return await _context.ApiTags.Where(x => x.Api.Name.Equals(apiName))
                .Select(x => x.Tag)
                .ToListAsync();
        }

        public async Task<UpdateResult> UpdateTagsOfApiAsync(string apiName, IEnumerable<string> tags)
        {
            var api = await _context.Apis.FirstOrDefaultAsync(x => x.Name.Equals(apiName));
            if (api == null)
                return UpdateResult.NotFound;

            var dbApiTags = await _context.ApiTags.Where(x => tags.Contains(x.Tag.Name)).ToListAsync();
            foreach(var incomingTag in tags)
            {
                if(!dbApiTags.Select(x => x.Tag.Name).Contains(incomingTag))
                {
                    var existingTag = await _context.Tags.FirstOrDefaultAsync(x => x.Name.Equals(incomingTag));
                    if (existingTag == null)
                        existingTag = new Tag() { Id = Guid.NewGuid(), Name = incomingTag };

                    await _context.Tags.AddAsync(existingTag);

                    dbApiTags.Add(new ApiTag()
                    {
                        ApiId = api.Id,
                        TagId = existingTag.Id
                    });
                }
            }

            foreach(var dbApiTag in new List<ApiTag>(dbApiTags))
            {
                var incomingTag = tags.FirstOrDefault(x => dbApiTag.Tag.Name.Equals(x));
                if (incomingTag == null)
                    _context.Remove(dbApiTag);
            }

            await _context.SaveChangesAsync();

            return UpdateResult.Updated;
        }
    }
}
