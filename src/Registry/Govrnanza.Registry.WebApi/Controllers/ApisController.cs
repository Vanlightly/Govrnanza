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
    [Route("api/v1/apis")]
    public class ApisController : Controller
    {
        private readonly IApiService _apiService;
        private readonly ITagService _tagService;
        private readonly IBusinessDomainsService _businessDomainsService;

        public ApisController(IApiService apiService,
            ITagService tagService,
            IBusinessDomainsService businessDomainsService)
        {
            _apiService = apiService;
            _tagService = tagService;
            _businessDomainsService = businessDomainsService;
        }

        // GET api/v1/apis
        [HttpGet]
        public async Task<ActionResult> GetAsync()
        {
            var apis = await _apiService.GetApisAsync();
            var apiTags = await _tagService.GetApiTagsAsync();
            var externalResults = Map.ToExternal(apis, apiTags);
            return Ok(externalResults);
        }

        // GET api/v1/apis/checkout
        [HttpGet("{apiName}")]
        public async Task<ActionResult> GetAsync(string apiName)
        {
            var api = await _apiService.GetApiAsync(apiName);
            if (api.Result == GetResult.NotFound)
                return NotFound();

            var tags = await _tagService.GetTagsOfApiAsync(apiName);

            var externalResult = Map.ToExternal(api.Data, tags);
            return Ok(externalResult);
        }

        // POST api/v1/apis
        [HttpPost]
        public async Task<ActionResult> PostAsync([FromBody]ApiExternal apiExternal)
        {
            var businessSubDomainResult = await _businessDomainsService.GetSubDomainAsync(apiExternal.SubDomainName);
            if (businessSubDomainResult.Result == GetResult.NotFound)
                return BadRequest();

            var api = Map.ToInternal(apiExternal, businessSubDomainResult.Data.Id);
            await _apiService.AddApiAsync(api);
            await _tagService.UpdateTagsOfApiAsync(api.Name, apiExternal.Tags);
            
            return Created($"api/v1/apis/{api.Name}", apiExternal);
        }

        // PUT api/v1/apis
        [HttpPut]
        public async Task<ActionResult> PutAsync([FromBody]ApiExternal apiExternal)
        {
            var businessSubDomainResult = await _businessDomainsService.GetSubDomainAsync(apiExternal.SubDomainName);
            if (businessSubDomainResult.Result == GetResult.NotFound)
                return BadRequest();

            var api = Map.ToInternal(apiExternal, businessSubDomainResult.Data.Id);
            var updateResult = await _apiService.UpdateApiAsync(api, true);
            await _tagService.UpdateTagsOfApiAsync(api.Name, apiExternal.Tags);

            if (updateResult == UpdateResult.Inserted)
                return Created($"api/v1/apis/{api.Name}", apiExternal);
            else if (updateResult == UpdateResult.Updated)
                return Ok();
            else if (updateResult == UpdateResult.NotFound)
                return NotFound();
            else
                return Ok();
        }

        // DELETE api/v1/apis/checkout
        [HttpDelete("{apiName}")]
        public async Task<ActionResult> DeleteAsync(string apiName)
        {
            var tags = await _tagService.GetTagsOfApiAsync(apiName);
            await _tagService.DeleteTagsOfApiAsync(apiName, tags.Select(x => x.Name));
            var deleteResult = await _apiService.DeleteApiAsync(apiName);
            if (deleteResult == DeleteResult.NotFound)
                return NotFound();

            return Ok();
        }
    }
}
