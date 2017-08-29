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
    /// CRUD operations for BusinessSubDomain objects
    /// </summary>
    [ApiVersion("1")]
    [Route("api/v1/business-sub-domains")]
    public class BusinessSubDomainsController : Controller
    {
        private readonly IBusinessDomainsService _businessDomainsService;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="businessDomainsService"></param>
        public BusinessSubDomainsController(IBusinessDomainsService businessDomainsService)
        {
            _businessDomainsService = businessDomainsService;
        }

        // GET api/v1/business-sub-domains
        /// <summary>
        /// Returns all business sub domains including their parent domains
        /// </summary>
        /// <returns>List of sub domains</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<BusinessSubDomainExternal>), 200)]
        [ProducesResponseType(typeof(void), 500)]
        public async Task<ActionResult> GetAsync()
        {
            var results = await _businessDomainsService.GetSubDomainsAsync();
            var externalResults = Map.ToExternal(results);
            return Ok(externalResults);
        }

        // GET api/v1/business-sub-domains/payroll
        /// <summary>
        /// Returns the business sub domain object with the given name
        /// </summary>
        /// <param name="subDomainName">The name of the sub domain</param>
        /// <returns>A Business Sub Domain</returns>
        [HttpGet("{subDomainName}")]
        [ProducesResponseType(typeof(BusinessSubDomainExternal), 200)]
        [ProducesResponseType(typeof(void), 404)]
        [ProducesResponseType(typeof(void), 500)]
        public async Task<ActionResult> GetAsync([FromRoute]string subDomainName)
        {
            var subDomain = await _businessDomainsService.GetSubDomainAsync(subDomainName);
            if (subDomain.Result == GetResult.NotFound)
                return NotFound();

            var externalResult = Map.ToExternal(subDomain.Data);
            return Ok(externalResult);
        }

        // POST api/v1/business-sub-domains
        /// <summary>
        /// Create a new business sub domain
        /// </summary>
        /// <param name="subDomainExternal"></param>
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
        public async Task<ActionResult> PostAsync([FromBody]BusinessSubDomainExternal subDomainExternal)
        {
            var subDomain = Map.ToInternal(subDomainExternal);
            var domain = await _businessDomainsService.GetDomainAsync(subDomainExternal.ParentBusinessDomain);
            if (domain.Result == GetResult.NotFound)
                return BadRequest();

            subDomain.ParentId = domain.Data.Id;
            await _businessDomainsService.AddSubDomainAsync(subDomain);
            return Created($"api/v1/business-sub-domains/{subDomain.Name}", subDomainExternal);
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
            var newDomain = await _businessDomainsService.GetDomainAsync(businessDomain);
            if (newDomain.Result == GetResult.NotFound)
                return NotFound();

            var subDomain = await _businessDomainsService.GetSubDomainAsync(businessSubDomain);
            if (subDomain.Result == GetResult.NotFound)
                return NotFound();

            subDomain.Data.ParentId = newDomain.Data.Id;
            subDomain.Data.Parent = newDomain.Data;
            await _businessDomainsService.UpdateSubDomainAsync(subDomain.Data, false);
            return Ok(Map.ToExternal(subDomain.Data));
        }

        // PUT api/v1/business-sub-domains
        /// <summary>
        /// Create or update a business sub domain
        /// </summary>
        /// <param name="subDomainExternal">The sub domain</param>
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
        public async Task<ActionResult> PutAsync([FromBody]BusinessSubDomainExternal subDomainExternal)
        {
            var domain = await _businessDomainsService.GetDomainAsync(subDomainExternal.ParentBusinessDomain);
            if (domain.Result == GetResult.NotFound)
                return BadRequest();

            var subDomain = Map.ToInternal(subDomainExternal);
            subDomain.ParentId = domain.Data.Id;
            var updateResult = await _businessDomainsService.UpdateSubDomainAsync(subDomain, true);
            if (updateResult == UpdateResult.Inserted)
                return Created($"api/v1/business-sub-domains/{subDomain.Name}", subDomainExternal);
            else if (updateResult == UpdateResult.Updated)
                return Ok();
            else if (updateResult == UpdateResult.NotFound)
                return NotFound();
            else
                return Ok();
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
            var deleteResult = await _businessDomainsService.DeleteSubDomainAsync(subDomainName);
            if (deleteResult == DeleteResult.NotFound)
                return NotFound();

            return Ok();
        }
    }
}
