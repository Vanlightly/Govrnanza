using Govrnanza.Registry.Backend.Infrastructure.Database;
using Govrnanza.Registry.Backend.Requests.Apis;
using Govrnanza.Registry.Backend.Responses;
using Govrnanza.Registry.Backend.Workflow;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Govrnanza.Registry.Backend.RequestHandlers.Apis
{
    public class ChangeApiVersionStatusHandler : IAsyncRequestHandler<ChangeApiVersionStatus, Response<UpdateResult>>
    {
        private readonly RegistryDbContext _context;

        public ChangeApiVersionStatusHandler(RegistryDbContext context)
        {
            _context = context;
        }

        public async Task<Response<UpdateResult>> Handle(ChangeApiVersionStatus request)
        {
            var apiVersion = await _context.ApiVersions
                                .Include(x => x.Api)
                                .FirstOrDefaultAsync(x => x.Api.Name.Equals(request.ApiName)
                                            && x.MajorVersion == request.MajorVersion
                                            && x.MinorVersion == request.MinorVersion);

            if (apiVersion == null)
                return new Response<UpdateResult>(UpdateResult.NotFound);

            if (!request.CurrentStatus.Equals(apiVersion.Status))
                return new Response<UpdateResult>(UpdateResult.NotUpdated, "Existing status does not match command");

            var validNextStatuses = StatusTransitions.GetNextValidStatuses(apiVersion.Status);
            if (!validNextStatuses.Contains(request.NewStatus))
                return new Response<UpdateResult>(UpdateResult.NotUpdated,
                    $"Not a valid status transition. Valid transitions are: { string.Join(',', validNextStatuses.Select(x => x.ToString())) }");

            apiVersion.Status = request.NewStatus;
            await _context.SaveChangesAsync();

            return new Response<UpdateResult>(UpdateResult.Updated);
        }
    }
}
