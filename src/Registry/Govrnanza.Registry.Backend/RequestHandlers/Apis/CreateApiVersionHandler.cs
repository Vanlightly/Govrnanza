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
    public class CreateApiVersionHandler : IAsyncRequestHandler<CreateApiVersion, Response<CreateResult>>
    {
        private readonly RegistryDbContext _context;

        public CreateApiVersionHandler(RegistryDbContext context)
        {
            _context = context;
        }

        public async Task<Response<CreateResult>> Handle(CreateApiVersion request)
        {
            var api = await _context.Apis.SingleOrDefaultAsync(x => x.Name.Equals(request.ApiName));
            if (api == null)
                return new Response<CreateResult>(CreateResult.DependentObjectNotFound, "No API can be found with the supplied name");

            var apiVersion = new ApiVersion()
            {
                ApiId = api.Id,
                MajorVersion = request.MajorVersion,
                MinorVersion = request.MinorVersion,
                Status = VersionStatus.Inception
            };

            await _context.ApiVersions.AddAsync(apiVersion);
            await _context.SaveChangesAsync();

            return new Response<CreateResult>(CreateResult.Created);
        }
    }
}
