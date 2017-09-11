using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Govrnanza.Registry.WebApi.Docs;
using Govrnanza.Registry.WebApi.Model.Apis;
using Govrnanza.Registry.WebApi.Model.Mappers;
using Govrnanza.Registry.Backend.Requests.Apis;


using MediatR;
using Govrnanza.Registry.Backend.Responses;
using Govrnanza.Registry.Core.Model;
using Govrnanza.Registry.Backend.Requests.BusinessDomains;
using Govrnanza.Registry.WebApi.Model;

namespace Govrnanza.Registry.WebApi.Controllers
{
    /// <summary>
    /// CRUD for API objects
    /// </summary>
    [ApiVersion("1")]
    [Route("api/v1/apis")]
    public class ApisController : Controller
    {
        private readonly IMediator _mediator;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mediator"></param>
        public ApisController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET api/v1/apis
        /// <summary>
        /// Returns all API objects
        /// </summary>
        /// <returns>List of all API objects</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ApiFull>), 200)]
        [ProducesResponseType(typeof(void), 500)]
        public async Task<ActionResult> GetApisAsync()
        {
            var getApisRequest = new GetApis();
            var response = await _mediator.Send(getApisRequest);

            var externalResults = Map.ToExternal(response.Result);
            return Ok(externalResults);
        }

        // GET api/v1/apis/{apiName}
        /// <summary>
        /// Get the API object for a given API name.
        /// </summary>
        /// <param name="apiName">The name of the requested API</param>
        /// <returns>API object</returns>
        /// <remarks>If no API matches the name given then a 404 is returned.</remarks>
        [HttpGet("{apiName}")]
        [ProducesResponseType(typeof(ApiFull), 200)]
        [ProducesResponseType(typeof(void), 404)]
        [ProducesResponseType(typeof(void), 500)]
        public async Task<ActionResult> GetApiAsync(string apiName)
        {
            var getApisRequest = new GetApi()
            {
                ApiName = apiName
            };
            var response = await _mediator.Send(getApisRequest);
            if (response.Result == GetResult.NotFound)
                return NotFound();

            var externalResult = Map.ToExternal(response.Data);
            return Ok(externalResult);
        }

        // POST api/v1/apis
        /// <summary>
        /// Creates a new API with a 0.1 version in the Inception status
        /// </summary>
        /// <param name="api">The API object</param>
        /// <returns></returns>
        /// <remarks>The SubDomainName property is required and if no sub domain is found that matches the SubDomainName value then a 400 Bad Request is returned.</remarks>
        [HttpPost]
        [ProducesResponseType(typeof(void), 201)]
        [ProducesResponseType(typeof(void), 400)]
        [ProducesResponseType(typeof(void), 500)]
        public async Task<ActionResult> CreateApiAsync([FromBody]ApiCreate api)
        {
            var getSubDomainRequest = new GetSubDomain()
            {
                Name = api.SubDomainName
            };
            var subDomainResonse = await _mediator.Send(getSubDomainRequest);
            if (subDomainResonse.Result == GetResult.NotFound)
                return BadRequest("No business sub domain was found with the supplied name");

            var createApi = new CreateApi()
            {
                BusinessOwner = api.BusinessOwner,
                BusinessSubDomainId = subDomainResonse.Data.Id,
                Description = api.Description,
                Name = api.Name,
                TechnicalOwner = api.TechnicalOwner,
                Title = api.Title,
                Tags = api.Tags
            };

            var response = await _mediator.Send(createApi);
            if (response.Result == CreateResult.Created)
                return Created($"api/v1/apis/{api.Name}", api);
            else
                return StatusCode(500);
        }

        // PATCH api/v1/apis/
        /// <summary>
        /// Updates the main fields of an API object. You cannot change the sub domain with this action, instead use the move sub resource
        /// </summary>
        /// <param name="api"></param>
        /// <returns></returns>
        /// <remarks>
        /// ### REMARKS ###
        /// The following codes are returned
        /// - 400 - No sub domain is found that matches the SubDomainName property
        /// - 200 - Updated an existing API object</remarks>
        [HttpPatch]
        [ProducesResponseType(typeof(void), 200)]
        [ProducesResponseType(typeof(void), 400)]
        [ProducesResponseType(typeof(void), 500)]
        public async Task<ActionResult> UpdateApiFieldsAsync([FromBody]ApiUpdate api)
        {
            var updateApiFields = new UpdateApiFields()
            {
                Name = api.Name,
                NewBusinessOwner = api.NewBusinessOwner,
                NewDescription = api.NewDescription,
                NewName = api.NewName,
                NewTechnicalOwner = api.NewTechnicalOwner,
                NewTitle = api.NewTitle,
                NewTags = api.NewTags
            };

            var response = await _mediator.Send(updateApiFields);

            if (response.Result == UpdateResult.Updated)
                return Ok();
            else if (response.Result == UpdateResult.NotFound)
                return NotFound();
            else if(response.Result == UpdateResult.NotUpdated)
                return StatusCode(500); // TODO return meaningful result to client
            else
                return StatusCode(500); // TODO return meaningful result to client
        }

        // POST api/v1/apis/{api-name}/move
        /// <summary>
        /// Creates a new API with a 0.1 version in the Inception status
        /// </summary>
        /// <param name="apiName">The name of the API object</param>
        /// <param name="currentSubDomain">The name of the business sub domain where the API is currently situated</param>
        /// <param name="destinationSubDomain">The name of the business sub domain where the API should be moved to</param>
        /// <returns></returns>
        /// <remarks>The both the old and new sub domains must exist else a 400 Bad Request is returned.</remarks>
        [HttpPost("{apiName}/move")]
        [ProducesResponseType(typeof(void), 200)]
        [ProducesResponseType(typeof(void), 400)]
        [ProducesResponseType(typeof(void), 500)]
        public async Task<ActionResult> MoveApiAsync([FromBody]string apiName, [FromBody]string currentSubDomain, [FromBody]string destinationSubDomain)
        {
            var moveRequest = new MoveApiToSubDomain()
            {
                ApiName = apiName,
                CurrentBusinessSubDomainName = currentSubDomain,
                DestinationBusinessSubDomainName = destinationSubDomain
            };

            var moveResonse = await _mediator.Send(moveRequest);
            if (moveResonse.Result == MoveResult.NotFound)
                return BadRequest(moveResonse.Description);

            return Ok();
        }

        // DELETE api/v1/apis/{apiName}
        /// <summary>
        /// Deletes an API object.
        /// </summary>
        /// <param name="apiName">Name of API to delete</param>
        /// <returns></returns>
        /// <remarks>No associated Tags are deleted</remarks>
        [HttpDelete("{apiName}")]
        [ProducesResponseType(typeof(void), 200)]
        [ProducesResponseType(typeof(void), 400)]
        [ProducesResponseType(typeof(void), 404)]
        [ProducesResponseType(typeof(void), 500)]
        public async Task<ActionResult> DeleteAsync([FromRoute]string apiName)
        {
            var deleteApi = new DeleteApi()
            {
                ApiName = apiName
            };

            var response = await _mediator.Send(deleteApi);
            if (response.Result == DeleteResult.NotFound)
                return NotFound();
            else if (response.Result == DeleteResult.NotDeletedDueToDependentObjects)
                return BadRequest(response.Description);
            else if (response.Result == DeleteResult.InvalidRequest)
                return BadRequest(response.Description);

            return Ok();
        }

        // POST api/v1/apis/{apiName}/versions/
        /// <summary>
        /// Creates a new version of the API
        /// </summary>
        /// <param name="apiName">The name of API object</param>
        /// <param name="version">The new version to be created</param>
        /// <returns></returns>
        /// <remarks>The SubDomainName property is required and if no sub domain is found that matches the SubDomainName value then a 400 Bad Request is returned.</remarks>
        [HttpPost("{apiName}/versions")]
        [ProducesResponseType(typeof(void), 201)]
        [ProducesResponseType(typeof(void), 400)]
        [ProducesResponseType(typeof(void), 500)]
        public async Task<ActionResult> CreateVersionAsync([FromRoute] string apiName, [FromBody]VersionApi version)
        {
            var createApiVersion = new CreateApiVersion()
            {
                ApiName = apiName,
                MajorVersion = version.MajorVersion,
                MinorVersion = version.MinorVersion
            };

            var response = await _mediator.Send(createApiVersion);
            if (response.Result == CreateResult.Created)
                return Created($"api/v1/apis/{apiName}/versions/{version.MajorVersion}.{version.MinorVersion}", null);
            else
                return StatusCode(500);
        }

        // PUT api/v1/apis/{apiName}/versions/{majorVersion}.{minorVersion}
        /// <summary>
        /// Changes the status of the version of the API
        /// </summary>
        /// <param name="apiName">The name of API object to be updated</param>
        /// <param name="majorVersion">The major version number to be updated </param>
        /// <param name="minorVersion">The minor version number to be updated</param>
        /// <param name="status">The new status of the API version</param>
        /// <returns></returns>
        /// <remarks>The change of API version status is governed by a workflow, if the status change is not valid according to this workflow then a 400 Bad Request is returned</remarks>
        [HttpPut("{apiName}/versions/{majorVersion}.{minorVersion}")]
        [ProducesResponseType(typeof(void), 201)]
        [ProducesResponseType(typeof(void), 400)]
        [ProducesResponseType(typeof(void), 404)]
        [ProducesResponseType(typeof(void), 500)]
        public async Task<ActionResult> UpdateVersionStatusAsync([FromRoute] string apiName, [FromRoute]int majorVersion, [FromRoute]int minorVersion,[FromBody]VersionStatusExternal status)
        {
            var changeApiVersionStatus = new ChangeApiVersionStatus()
            {
                ApiName = apiName,
                MajorVersion = majorVersion,
                MinorVersion = minorVersion,
                NewStatus = (VersionStatus)(int)status
            };

            var response = await _mediator.Send(changeApiVersionStatus);
            if (response.Result == UpdateResult.NotFound)
                return NotFound();
            else if (response.Result == UpdateResult.NotUpdated)
                return BadRequest(response.Description);
            else if (response.Result == UpdateResult.Updated)
                return Ok();
            else
                return StatusCode(500); // result not contemplated, which is bad
        }

        // DELETE api/v1/apis/{apiName}/versions/{majorVersion}.{minorVersion}
        /// <summary>
        /// Creates a new version of the API
        /// </summary>
        /// <param name="apiName">The name of API object</param>
        /// <param name="majorVersion">The major version number to be updated </param>
        /// <param name="minorVersion">The minor version number to be updated</param>
        /// <returns></returns>
        [HttpDelete("{apiName}/versions/{majorVersion}.{minorVersion}")]
        [ProducesResponseType(typeof(void), 201)]
        [ProducesResponseType(typeof(void), 400)]
        [ProducesResponseType(typeof(void), 500)]
        public async Task<ActionResult> DeleteVersionAsync([FromRoute] string apiName, [FromRoute]int majorVersion, [FromRoute]int minorVersion)
        {
            var deleteApiVersion = new DeleteApiVersion()
            {
                ApiName = apiName,
                MajorVersion = majorVersion,
                MinorVersion = minorVersion
            };

            var response = await _mediator.Send(deleteApiVersion);
            if (response.Result == DeleteResult.Deleted)
                return Ok();
            else
                return StatusCode(500);
        }

        private async Task<GetResponse<Api>> GetApiResponseAsync(string apiName)
        {
            return await _mediator.Send(new GetApi());
        }
    }
}
