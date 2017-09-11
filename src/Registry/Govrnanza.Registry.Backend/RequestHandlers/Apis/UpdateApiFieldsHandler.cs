using Govrnanza.Registry.Backend.Infrastructure.Database;
using Govrnanza.Registry.Backend.Requests.Apis;
using Govrnanza.Registry.Backend.Responses;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Govrnanza.Registry.Backend.RequestHandlers.Apis
{
    public class UpdateApiFieldsHandler : IAsyncRequestHandler<UpdateApiFields, Response<UpdateResult>>
    {
        private readonly RegistryDbContext _context;
        private readonly IMediator _mediator;

        public UpdateApiFieldsHandler(RegistryDbContext context,
            IMediator mediator)
        {
            _context = context;
            _mediator = mediator;
        }

        public async Task<Response<UpdateResult>> Handle(UpdateApiFields request)
        {
            var dbApi = await _context.Apis.FirstOrDefaultAsync(x => x.Name.Equals(request.Name));
            if (dbApi == null)
            {
                return new Response<UpdateResult>(UpdateResult.NotFound);
            }
            else
            {
                if(!string.IsNullOrEmpty(request.NewName))
                    dbApi.Name = request.NewName;

                if (!string.IsNullOrEmpty(request.NewTitle))
                    dbApi.Title = request.NewTitle;

                if (!string.IsNullOrEmpty(request.NewDescription))
                    dbApi.Description = request.NewDescription;

                if (!string.IsNullOrEmpty(request.NewBusinessOwner))
                    dbApi.BusinessOwner = request.NewBusinessOwner;

                if (!string.IsNullOrEmpty(request.NewTechnicalOwner))
                    dbApi.TechnicalOwner = request.NewTechnicalOwner;

                if (request.NewTags != null)
                {
                    var tagsResponse = await SetApiTagsAsync(request);

                    if (tagsResponse.Result == UpdateResult.Updated)
                    {
                        await _context.SaveChangesAsync();
                        return new Response<UpdateResult>(UpdateResult.Updated);
                    }
                    else
                    {
                        return new Response<UpdateResult>(UpdateResult.NotUpdated,
                            $"Update aborted - could not update tags. Message is {tagsResponse.Description}");
                    }
                }
                else
                {
                    await _context.SaveChangesAsync();

                    return new Response<UpdateResult>(UpdateResult.Updated);
                }
            }
        }

        private async Task<Response<UpdateResult>> SetApiTagsAsync(UpdateApiFields request)
        {
            var setApiTags = new SetApiTags()
            {
                ApiName = request.Name,
                Tags = request.NewTags,
                Commit = false,
                UseLocal = true
            };

            return await _mediator.Send(setApiTags);
        }
    }
}
