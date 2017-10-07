using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Govrnanza.Registry.WebApi.Model.Environment;

namespace Govrnanza.Registry.WebApi.Controllers
{
    /// <summary>
    /// View environmental and configuration data
    /// </summary>
    [ApiVersion("1")]
    [Route("api/v1/environment")]
    public class EnvironmentController : Controller
    {
        private IHostingEnvironment _hostingEnvironment;
        private IConfiguration _configuration;

        public EnvironmentController(IHostingEnvironment hostingEnvironment, IConfiguration configuration)
        {
            _hostingEnvironment = hostingEnvironment;
            _configuration = configuration;
        }

        // GET api/v1/environment
        /// <summary>
        /// Returns some environment and all configuration data
        /// </summary>
        /// <returns></returns>
        /// <remarks>## Development feature, remove later</remarks>
        [HttpGet]
        [ProducesResponseType(typeof(AppEnvironment), 200)]
        [ProducesResponseType(typeof(void), 500)]
        public ActionResult Get()
        {
            var env = new AppEnvironment();
            env.EnvironmentName = _hostingEnvironment.EnvironmentName;
            env.HostName = Environment.MachineName;
            env.Configuration = _configuration.AsEnumerable().Select(x => new ConfigItem()
            {
                Key = x.Key,
                Value = x.Value
            }).ToList();

            return Ok(env);
        }
        
    }
}
