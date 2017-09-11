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
    public class GetTagsHandler : IAsyncRequestHandler<GetTags, Response<IEnumerable<string>>>
    {
        private readonly RegistryDbContext _context;

        public GetTagsHandler(RegistryDbContext context)
        {
            _context = context;
        }

        public async Task<Response<IEnumerable<string>>> Handle(GetTags request)
        {
            if (IsEmpty(request))
            {
                var tags = await _context.Tags.Select(x => x.Name).ToListAsync();
                return new Response<IEnumerable<string>>(tags);
            }
            else
            {
                var tags = await _context.ApiTags
                            .Include(x => x.Api)
                            .Include(x => x.Tag)
                            .Where(x => x.Api.Name.Equals(request.ApiName))
                            .Select(x => x.Tag.Name)
                            .ToListAsync();

                return new Response<IEnumerable<string>>(tags);
            }
        }

        private bool IsEmpty(GetTags request)
        {
            return string.IsNullOrEmpty(request.ApiName);
        }
    }
}
