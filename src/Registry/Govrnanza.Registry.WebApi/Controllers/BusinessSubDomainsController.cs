using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Govrnanza.Registry.WebApi.Docs;
using Govrnanza.Registry.WebApi.Model.Mappers;
using MediatR;
using Govrnanza.Registry.WebApi.Model.BusinessDomains;
using Govrnanza.Registry.Backend.Requests.BusinessDomains;
using Govrnanza.Registry.Backend.Responses;

namespace Govrnanza.Registry.WebApi.Controllers
{
    /// <summary>
    /// CRUD operations for BusinessSubDomain objects
    /// </summary>
    [ApiVersion("1")]
    [Route("api/v1/business-sub-domains")]
    public class BusinessSubDomainsController : Controller
    {
        private readonly IMediator _mediator;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mediator"></param>
        public BusinessSubDomainsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET api/v1/business-sub-domains
        /// <summary>
        /// Returns all business sub domains including their parent domains
        /// </summary>
        /// <returns>List of sub domains</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<BusinessSubDomain>), 200)]
        [ProducesResponseType(typeof(void), 500)]
        public async Task<ActionResult> GetAsync()
        {
            var getRequest = new GetSubDomains();
            var response = await _mediator.Send(getRequest);
            var externalResults = Map.ToExternal(response.Result);
            return Ok(externalResults);
        }

        // GET api/v1/business-sub-domains/payroll
        /// <summary>
        /// Returns the business sub domain object with the given name
        /// </summary>
        /// <param name="subDomainName">The name of the sub domain</param>
        /// <returns>A Business Sub Domain</returns>
        [HttpGet("{subDomainName}")]
        [ProducesResponseType(typeof(BusinessSubDomain), 200)]
        [ProducesResponseType(typeof(void), 404)]
        [ProducesResponseType(typeof(void), 500)]
        public async Task<ActionResult> GetAsync([FromRoute]string subDomainName)
        {
            var getRequest = new GetSubDomain()
            {
                Name = subDomainName
            };
            var response = await _mediator.Send(getRequest);
            if (response.Result == GetResult.NotFound)
                return NotFound();

            var externalResult = Map.ToExternal(response.Data);
            return Ok(externalResult);
        }

        // POST api/v1/business-sub-domains
        /// <summary>
        /// Create a new business sub domain
        /// </summary>
        /// <param name="subDomain"></param>
        /// <returns></returns>
        /// <remarks>
        /// ### REMARKS ###
        /// The following codes are returned
        /// - 201 - Created the business sub domain object
        /// - 400 - The parent domain does not exist</remarks>
        [HttpPost]
        [ProducesResponseType(typeof(void), 201)]
        [ProducesResponseType(typeof(void), 400)]
        [ProducesResponseType(typeof(void), 500)]
        public async Task<ActionResult> CreateDomainAsync([FromBody]BusinessSubDomain subDomain)
        {
            var createSubDomain = new CreateSubDomain()
            {
                Name = subDomain.Name,
                Description = subDomain.Description,
                ParentDomainName = subDomain.ParentBusinessDomain
            };
            
            var response = await _mediator.Send(createSubDomain);
            if (response.Result == CreateResult.DependentObjectNotFound)
                return BadRequest();
            else if (response.Result == CreateResult.Created)
                return Created($"api/v1/business-sub-domains/{subDomain.Name}", null);
            else
                return StatusCode(500); //  result not contemplated
        }

        // POST api/v1/business-sub-domains/supplierinvoicing/move/invoicing
        /// <summary>
        /// Move a business sub domain from one parent domain to another
        /// </summary>
        /// <param name="businessSubDomain">Name of the sub domain to move</param>
        /// <param name="businessDomain">The name of the new parent domain</param>
        /// <returns></returns>
        /// <remarks>
        /// ### REMARKS ###
        /// The following response codes are returned
        /// - 200 - Move successful
        /// - 404 - Either the sub domain or the new parent domain do not exist
        /// </remarks>
        [HttpPost("{businessSubDomain}/move/{businessDomain}")]
        [ProducesResponseType(typeof(void), 200)]
        [ProducesResponseType(typeof(void), 404)]
        [ProducesResponseType(typeof(void), 500)]
        public async Task<ActionResult> PostAsync([FromRoute]string businessSubDomain, [FromRoute] string businessDomain)
        {
            var moveRequest = new MoveSubDomain()
            {
                SubDomainName = businessSubDomain,
                DestinationDomainName = businessDomain
            };

            var response = await _mediator.Send(moveRequest);
            if (response.Result == MoveResult.NotFound)
                return NotFound();
            else if (response.Result == MoveResult.Moved)
                return Ok();

            return StatusCode(500); // result not contemplated
        }

        // PUT api/v1/business-sub-domains
        /// <summary>
        /// Updates an existing business sub domain
        /// </summary>
        /// <param name="subDomainUpdate">The sub domain</param>
        /// <returns></returns>
        /// <remarks>
        /// ### REMARKS ###
        /// The following response codes are returned
        /// - 200 - Updated an existing sub domain
        /// - 201 - Created a new sub domain
        /// - 400 - The parent domain does not exist
        /// </remarks>
        [HttpPut]
        [ProducesResponseType(typeof(void), 200)]
        [ProducesResponseType(typeof(void), 404)]
        [ProducesResponseType(typeof(void), 500)]
        public async Task<ActionResult> PutAsync([FromBody]BusinessSubDomainUpdate subDomainUpdate)
        {
            var updateSubDomain = new UpdateSubDomain()
            {
                Name = subDomainUpdate.Name,
                NewName = subDomainUpdate.NewName,
                NewDescription = subDomainUpdate.NewDescription
            };

            var response = await _mediator.Send(updateSubDomain);
            if (response.Result == UpdateResult.NotFound)
                return NotFound();
            else if (response.Result == UpdateResult.Updated)
                return Ok();
            else
                return StatusCode(500); // result not contemplated;
        }

        // DELETE api/v1/business-sub-domains/sales
        /// <summary>
        /// Deletes the business sub domain
        /// </summary>
        /// <param name="subDomainName">Name of the sub domain to delete</param>
        /// <returns></returns>
        [HttpDelete("{subDomainName}")]
        [ProducesResponseType(typeof(void), 200)]
        [ProducesResponseType(typeof(void), 404)]
        [ProducesResponseType(typeof(void), 500)]
        public async Task<ActionResult> DeleteAsync([FromRoute]string subDomainName)
        {
            var deleteSubDomain = new DeleteSubDomain()
            {
                Name = subDomainName
            };

            var response = await _mediator.Send(deleteSubDomain);
            if (response.Result == DeleteResult.NotFound)
                return NotFound();
            else if (response.Result == DeleteResult.NotDeletedDueToDependentObjects)
                return BadRequest(response.Description);
            else if (response.Result == DeleteResult.Deleted)
                return Ok();
            else
                return StatusCode(500); // result not contemplated
        }
    }
}
