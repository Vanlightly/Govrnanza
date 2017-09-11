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
    public class ChangeTagNameHandler : IAsyncRequestHandler<ChangeTagName, Response<UpdateResult>>
    {
        private readonly RegistryDbContext _context;

        public ChangeTagNameHandler(RegistryDbContext context)
        {
            _context = context;
        }

        public async Task<Response<UpdateResult>> Handle(ChangeTagName request)
        {
            var tag = await _context.Tags.FirstOrDefaultAsync(x => x.Name.Equals(request.TagName));
            if (tag == null)
                return new Response<UpdateResult>(UpdateResult.NotFound);

            tag.Name = request.NewTagName;
            await _context.SaveChangesAsync();

            return new Response<UpdateResult>(UpdateResult.Updated);
        }

        
    }
}
