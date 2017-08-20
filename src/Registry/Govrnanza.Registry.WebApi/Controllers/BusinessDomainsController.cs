using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Govrnanza.Registry.WebApi.Model;
using Govrnanza.Registry.WebApi.ServiceContracts;
using Govrnanza.Registry.WebApi.ServiceContracts.Responses;
using Govrnanza.Registry.WebApi.Docs;
using Govrnanza.Registry.WebApi.Model.External;
using Govrnanza.Registry.WebApi.Model.Internal;
using Govrnanza.Registry.WebApi.Model.Mappers;

namespace Govrnanza.Registry.WebApi.Controllers
{
    [ApiVersion("1")]
    [Route("api/v1/business-domains")]
    public class BusinessDomainsController : Controller
    {
        private readonly IBusinessDomainsService _businessDomainsService;

        public BusinessDomainsController(IBusinessDomainsService businessDomainsService)
        {
            _businessDomainsService = businessDomainsService;
        }

        // GET api/v1/business-domains
        [HttpGet]
        public async Task<ActionResult> GetAsync()
        {
            var results = await _businessDomainsService.GetDomainsAsync();
            var externalResults = Map.ToExternal(results);
            return Ok(externalResults);
        }

        // GET api/v1/business-domains/sales
        [HttpGet("{domainName}")]
        public async Task<ActionResult> GetAsync(string domainName)
        {
            var domain = await _businessDomainsService.GetDomainAsync(domainName);
            if (domain.Result == GetResult.NotFound)
                return NotFound();

            var externalResult = Map.ToExternal(domain.Data);
            return Ok(externalResult);
        }

        // POST api/v1/business-domains
        [HttpPost]
        public async Task<ActionResult> PostAsync([FromBody]BusinessDomainExternal domainExternal)
        {
            var domain = Map.ToInternal(domainExternal);
            await _businessDomainsService.AddDomainAsync(domain);
            return Created($"api/business-domains/{domain.Name}", domain);
        }

        // PUT api/v1/business-domains
        [HttpPut]
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
        [HttpDelete("{domainName}")]
        public async Task<ActionResult> DeleteAsync(string domainName)
        {
            var deleteResult = await _businessDomainsService.DeleteDomainAsync(domainName);
            if (deleteResult == DeleteResult.NotFound)
                return NotFound();

            return Ok();
        }
    }
}
