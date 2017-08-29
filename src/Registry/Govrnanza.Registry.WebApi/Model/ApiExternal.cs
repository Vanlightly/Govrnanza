using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Govrnanza.Registry.WebApi.Model
{
    /// <summary>
    /// API object
    /// </summary>
    public class ApiExternal
    {
        /// <summary>
        /// The name of API, used as an identifier
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Short description about the API
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// The Business Sub Domain which owns this API. A mandatory field.
        /// </summary>
        public string SubDomainName { get; set; }
        /// <summary>
        /// List of tags used to classify the API on further dimensions.
        /// </summary>
        public List<string> Tags { get; set; }
    }
}
