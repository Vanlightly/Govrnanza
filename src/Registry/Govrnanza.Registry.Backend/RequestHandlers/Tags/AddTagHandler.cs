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
    public class AddTagHandler : IAsyncRequestHandler<AddTag, Response<CreateResult>>
    {
        private readonly RegistryDbContext _context;

        public AddTagHandler(RegistryDbContext context)
        {
            _context = context;
        }

        public async Task<Response<CreateResult>> Handle(AddTag request)
        {
            var apiTag = new ApiTag();

            var api = await _context.Apis.FirstOrDefaultAsync(x => x.Name.Equals(request.ApiName));
            if (api == null)
                return new Response<CreateResult>(CreateResult.DependentObjectNotFound);

            apiTag.Api = api;
            apiTag.ApiId = api.Id;

            var tag = await _context.Tags.FirstOrDefaultAsync(x => x.Name.Equals(request.TagName));
            if (tag == null)
                tag = new Tag() { Id = Guid.NewGuid(), Name = request.TagName };

            apiTag.TagId = tag.Id;
            apiTag.Tag = tag;

            await _context.ApiTags.AddAsync(apiTag);
            await _context.SaveChangesAsync();

            return new Response<CreateResult>(CreateResult.Created);
        }

        
    }
}
