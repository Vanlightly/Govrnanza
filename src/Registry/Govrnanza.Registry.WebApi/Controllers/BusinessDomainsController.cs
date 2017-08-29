using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Govrnanza.Registry.WebApi.Model;
using Govrnanza.Registry.WebApi.Docs;
using Govrnanza.Registry.WebApi.Model.Mappers;
using Govrnanza.Registry.Backend.ServiceContracts;
using Govrnanza.Registry.Backend.ServiceContracts.Responses;

namespace Govrnanza.Registry.WebApi.Controllers
{
    /// <summary>
    /// CRUD operations on BusinessDomain objects
    /// </summary>
    [ApiVersion("1")]
    [Route("api/v1/business-domains")]
    public class BusinessDomainsController : Controller
    {
        private readonly IBusinessDomainsService _businessDomainsService;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="businessDomainsService"></param>
        public BusinessDomainsController(IBusinessDomainsService businessDomainsService)
        {
            _businessDomainsService = businessDomainsService;
        }

        // GET api/v1/business-domains
        /// <summary>
        /// Returns all business domains including their sub domains
        /// </summary>
        /// <returns>Business Domains</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<BusinessDomainExternal>), 200)]
        [ProducesResponseType(typeof(void), 500)]
        public async Task<ActionResult> GetAsync()
        {
            var results = await _businessDomainsService.GetDomainsAsync();
            var externalResults = Map.ToExternal(results);
            return Ok(externalResults);
        }

        // GET api/v1/business-domains/sales
        /// <summary>
        /// Returns the business domain object with the given name
        /// </summary>
        /// <param name="domainName">The name of the business domain to be returned</param>
        /// <returns>The business domain</returns>
        [HttpGet("{domainName}")]
        [ProducesResponseType(typeof(BusinessDomainExternal), 200)]
        [ProducesResponseType(typeof(void), 404)]
        [ProducesResponseType(typeof(void), 500)]
        public async Task<ActionResult> GetAsync(string domainName)
        {
            var domain = await _businessDomainsService.GetDomainAsync(domainName);
            if (domain.Result == GetResult.NotFound)
                return NotFound();

            var externalResult = Map.ToExternal(domain.Data);
            return Ok(externalResult);
        }

        // POST api/v1/business-domains
        /// <summary>
        /// Create a new business domain
        /// </summary>
        /// <param name="domainExternal"></param>
        /// <returns></returns>
        /// <remarks>
        /// ### REMARKS ###
        /// Any business sub domains included in the request will also be created. If any sub domains already exist
        /// for another business domain then the request will fail
        /// </remarks>
        [HttpPost]
        [ProducesResponseType(typeof(void), 201)]
        [ProducesResponseType(typeof(void), 500)]
        public async Task<ActionResult> PostAsync([FromBody]BusinessDomainExternal domainExternal)
        {
            var domain = Map.ToInternal(domainExternal);
            await _businessDomainsService.AddDomainAsync(domain);
            return Created($"api/business-domains/{domain.Name}", domain);
        }

        // PUT api/v1/business-domains
        /// <summary>
        /// Creates or updates a business domain object.
        /// </summary>
        /// <param name="domainExternal">A business domain object</param>
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
        public async Task<ActionResult> PutAsync([FromBody]BusinessDomainExternal domainExternal)
        {
            var domain = Map.ToInternal(domainExternal);
            var updateResult = await _businessDomainsService.UpdateDomainAsync(domain, true);

            if (updateResult == UpdateResult.Inserted)
                return Created($"api/business-domains/{domain.Name}", domain);
            else if (updateResult == UpdateResult.Updated)
                return Ok();
            else if (updateResult == UpdateResult.NotFound)
                return NotFound();
            else
                return Ok();
        }

        // DELETE api/v1/business-domains/sales
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
            var deleteResult = await _businessDomainsService.DeleteDomainAsync(domainName);
            if (deleteResult == DeleteResult.NotFound)
                return NotFound();
            else if (deleteResult == DeleteResult.NotDeletedDueToDependentObjects)
                return BadRequest();
            
            return Ok();
        }
    }
}
