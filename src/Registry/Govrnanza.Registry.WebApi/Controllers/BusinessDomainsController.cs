using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Govrnanza.Registry.WebApi.Docs;
using Govrnanza.Registry.WebApi.Model.Mappers;
using MediatR;
using Govrnanza.Registry.Backend.Requests.BusinessDomains;
using Govrnanza.Registry.Backend.Responses;
using Govrnanza.Registry.WebApi.Model.BusinessDomains;

namespace Govrnanza.Registry.WebApi.Controllers
{
    /// <summary>
    /// CRUD operations on BusinessDomain objects
    /// </summary>
    [ApiVersion("1")]
    [Route("api/v1/business-domains")]
    public class BusinessDomainsController : Controller
    {
        private readonly IMediator _mediator;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mediator"></param>
        public BusinessDomainsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET api/v1/business-domains
        /// <summary>
        /// Returns all business domains including their sub domains
        /// </summary>
        /// <returns>Business Domains</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<BusinessDomain>), 200)]
        [ProducesResponseType(typeof(void), 500)]
        public async Task<ActionResult> GetAsync()
        {
            var getDomainsRequest = new GetDomains();
            var resonse = await _mediator.Send(getDomainsRequest);
            var externalResults = Map.ToExternal(resonse.Result);
            return Ok(externalResults);
        }

        // GET api/v1/business-domains/sales
        /// <summary>
        /// Returns the business domain object with the given name
        /// </summary>
        /// <param name="domainName">The name of the business domain to be returned</param>
        /// <returns>The business domain</returns>
        [HttpGet("{domainName}")]
        [ProducesResponseType(typeof(BusinessDomain), 200)]
        [ProducesResponseType(typeof(void), 404)]
        [ProducesResponseType(typeof(void), 500)]
        public async Task<ActionResult> GetAsync(string domainName)
        {
            var getDomainRequest = new GetDomain() { Name = domainName };
            var response = await _mediator.Send(getDomainRequest);
            if (response.Result == GetResult.NotFound)
                return NotFound();

            var externalResult = Map.ToExternal(response.Data);
            return Ok(externalResult);
        }

        // POST api/v1/business-domains
        /// <summary>
        /// Create a new business domain
        /// </summary>
        /// <param name="businessDomain"></param>
        /// <returns></returns>
        /// <remarks>
        /// ### REMARKS ###
        /// Any business sub domains included in the request will also be created. If any sub domains already exist
        /// for another business domain then the request will fail
        /// </remarks>
        [HttpPost]
        [ProducesResponseType(typeof(void), 201)]
        [ProducesResponseType(typeof(void), 500)]
        public async Task<ActionResult> PostAsync([FromBody]BusinessDomain businessDomain)
        {
            var createDomain = new CreateDomain()
            {
                Name = businessDomain.Name,
                Description = businessDomain.Description,
                SubDomains = businessDomain.SubDomains.Select(x => new CreateSubDomain()
                {
                    Name = x.Name,
                    Description = x.Description,
                    ParentDomainName = businessDomain.Name
                }).ToList()
            };

            var response = await _mediator.Send(createDomain);
            if (response.Result == CreateResult.Created)
                return Created($"api/business-domains/{businessDomain.Name}", null);
            else
                return StatusCode(500);

        }

        // PUT api/v1/business-domains
        /// <summary>
        /// Updates an existing business domain object.
        /// </summary>
        /// <param name="businessDomainUpdate">A business domain update request</param>
        /// <returns></returns>
        /// <remarks>
        /// ### REMARKS ###
        /// The following codes are returned
        /// - 200 - Updated an existing business domain object
        /// - 201 - Created a new business domain object</remarks>
        [HttpPut]
        [ProducesResponseType(typeof(void), 200)]
        [ProducesResponseType(typeof(void), 201)]
        [ProducesResponseType(typeof(void), 500)]
        public async Task<ActionResult> PutAsync([FromBody]BusinessDomainUpdate businessDomainUpdate)
        {
            var updateDomain = new UpdateDomain()
            {
                Name = businessDomainUpdate.Name,
                NewName = businessDomainUpdate.NewName,
                UpdatedSubDomains = businessDomainUpdate.SubDomains.Select(x => new UpdateSubDomain()
                {
                    Name = x.Name,
                    NewDescription = x.NewDescription,
                    NewName = x.NewName
                }).ToList()
            };
            var updateResponse = await _mediator.Send(updateDomain);

            if (updateResponse.Result == UpdateResult.Updated)
                return Ok();
            else if (updateResponse.Result == UpdateResult.NotFound)
                return NotFound();
            else
                return StatusCode(500); // result no contemplated
        }

        // DELETE api/v1/business-domains/{domainName}
        /// <summary>
        /// Deletes a given business domain
        /// </summary>
        /// <param name="domainName">The name of the business domain to delete</param>
        /// <returns></returns>
        /// <remarks>
        /// ### REMARKS ###
        /// The following codes are returned
        /// - 200 - The business domain was deleted
        /// - 400 - The business domain has sub domains and so could not be deleted
        /// - 404 - The business domain does not exist
        /// </remarks>
        [HttpDelete("{domainName}")]
        [ProducesResponseType(typeof(void), 200)]
        [ProducesResponseType(typeof(void), 400)]
        [ProducesResponseType(typeof(void), 404)]
        [ProducesResponseType(typeof(void), 500)]
        public async Task<ActionResult> DeleteAsync(string domainName)
        {
            var deleteDomain = new DeleteDomain()
            {
                Name = domainName
            };
            var deleteResponse = await _mediator.Send(deleteDomain);
            if (deleteResponse.Result == DeleteResult.NotFound)
                return NotFound();
            else if (deleteResponse.Result == DeleteResult.NotDeletedDueToDependentObjects)
                return BadRequest(deleteResponse.Description);
            
            return Ok();
        }
    }
}
