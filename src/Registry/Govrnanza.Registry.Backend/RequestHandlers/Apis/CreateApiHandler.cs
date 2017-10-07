using Govrnanza.Registry.Backend.Infrastructure.Database;
using Govrnanza.Registry.Backend.Requests.Apis;
using Govrnanza.Registry.Backend.Responses;
using Govrnanza.Registry.Core.Model;
using MediatR;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Govrnanza.Registry.Backend.RequestHandlers.Apis
{
    public class CreateApiHandler : IAsyncRequestHandler<CreateApi, Response<CreateResult>>
    {
        private readonly IMediator _mediator;
        private readonly RegistryDbContext _context;
        private readonly IConfiguration _configuration;

        public CreateApiHandler(RegistryDbContext context,
            IMediator mediator,
            IConfiguration configuration)
        {
            _context = context;
            _mediator = mediator;
            _configuration = configuration;
        }

        public async Task<Response<CreateResult>> Handle(CreateApi request)
        {
            await AddApiAsync(request);
            var tagsResponse = await SetApiTagsAsync(request);
            
            if (tagsResponse.Result == UpdateResult.Updated)
            {
                await _context.SaveChangesAsync();
                return new Response<CreateResult>(CreateResult.Created);
            }
            else
            {
                return new Response<CreateResult>(CreateResult.NotCreated,
                    $"Create aborted - could not add tags. Message is {tagsResponse.Description}");
            }
        }

        private async Task AddApiAsync(CreateApi request)
        {
            var apiId = Guid.NewGuid();

            var api = new Api()
            {
                Id = apiId,
                Name = request.Name,
                Title = request.Title,
                Description = request.Description,
                BusinessOwner = request.BusinessOwner,
                TechnicalOwner = request.TechnicalOwner,
                BusinessSubDomainId = request.BusinessSubDomainId,
                Versions = new List<ApiVersion>()
                {
                    new ApiVersion()
                    {
                        ApiId = apiId,
                        MajorVersion = _configuration.GetValue<int>("InceptionMajorVersion"),
                        MinorVersion = _configuration.GetValue<int>("InceptionMinorVersion"),
                        Status = VersionStatus.Inception
                    }
                }
            };

            await _context.Apis.AddAsync(api);
        }

        private async Task<Response<UpdateResult>> SetApiTagsAsync(CreateApi request)
        {
            var setApiTags = new SetApiTags()
            {
                ApiName = request.Name,
                Tags = request.Tags,
                Commit = false,
                UseLocal = true
            };

            return await _mediator.Send(setApiTags);
        }
    }
}
