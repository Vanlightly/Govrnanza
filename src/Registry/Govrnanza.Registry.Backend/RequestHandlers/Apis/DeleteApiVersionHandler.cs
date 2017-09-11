using Govrnanza.Registry.Backend.Infrastructure.Database;
using Govrnanza.Registry.Backend.Requests.Apis;
using Govrnanza.Registry.Backend.Responses;
using Govrnanza.Registry.Core.Model;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Govrnanza.Registry.Backend.RequestHandlers.Apis
{
    public class DeleteApiVersionHandler : IAsyncRequestHandler<DeleteApiVersion, Response<DeleteResult>>
    {
        private readonly RegistryDbContext _context;

        public DeleteApiVersionHandler(RegistryDbContext context)
        {
            _context = context;
        }

        public async Task<Response<DeleteResult>> Handle(DeleteApiVersion request)
        {
            var apiVersion = await _context.ApiVersions
                .Include(x => x.Api)
                .SingleOrDefaultAsync(x => x.Api.Name.Equals(request.ApiName)
                    && x.MajorVersion == request.MajorVersion
                    && x.MinorVersion == request.MinorVersion);

            if (apiVersion == null)
                return new Response<DeleteResult>(DeleteResult.NotFound);

            _context.ApiVersions.Remove(apiVersion);
            await _context.SaveChangesAsync();

            return new Response<DeleteResult>(DeleteResult.Deleted);
        }
    }
}
