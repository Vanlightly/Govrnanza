using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Govrnanza.Registry.WebApi.Model;
using Govrnanza.Registry.WebApi.ServiceContracts;
using Govrnanza.Registry.WebApi.ServiceContracts.Responses;
using Govrnanza.Registry.WebApi.Docs;
using Govrnanza.Registry.WebApi.Model.Internal;
using Govrnanza.Registry.WebApi.Model.Mappers;
using Govrnanza.Registry.WebApi.Model.External;

namespace Govrnanza.Registry.WebApi.Controllers
{
    [ApiVersion("1")]
    [Route("api/v1/business-sub-domains")]
    public class BusinessSubDomainsController : Controller
    {
        private readonly IBusinessDomainsService _businessDomainsService;

        public BusinessSubDomainsController(IBusinessDomainsService businessDomainsService)
        {
            _businessDomainsService = businessDomainsService;
        }

        // GET api/v1/business-sub-domains
        [HttpGet]
        public async Task<ActionResult> GetAsync()
        {
            var results = await _businessDomainsService.GetSubDomainsAsync();
            var externalResults = Map.ToExternal(results);
            return Ok(externalResults);
        }

        // GET api/v1/business-sub-domains/payroll
        [HttpGet("{subDomainName}")]
        public async Task<ActionResult> GetAsync(string subDomainName)
        {
            var subDomain = await _businessDomainsService.GetSubDomainAsync(subDomainName);
            if (subDomain.Result == GetResult.NotFound)
                return NotFound();

            var externalResult = Map.ToExternal(subDomain.Data);
            return Ok(externalResult);
        }

        // POST api/v1/business-sub-domains
        [HttpPost]
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
        [HttpPost("{businessSubDomain}/move/{businessDomain}")]
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
        [HttpPut]
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
        [HttpDelete("{subDomainName}")]
        public async Task<ActionResult> DeleteAsync(string subDomainName)
        {
            var deleteResult = await _businessDomainsService.DeleteSubDomainAsync(subDomainName);
            if (deleteResult == DeleteResult.NotFound)
                return NotFound();

            return Ok();
        }
    }
}
