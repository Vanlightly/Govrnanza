using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Govrnanza.Registry.WebApi.Model.Apis
{
    /// <summary>
    /// A complete API object with all child elements
    /// </summary>
    public class ApiFull
    {
        /// <summary>
        /// The name of API, used as an identifier
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The friendly name of the API
        /// </summary>
        public string Title { get; set; }
        
        /// <summary>
        /// Short description about the API
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The business owner of the API, like a line of business manager
        /// </summary>
        public string BusinessOwner { get; set; }

        /// <summary>
        /// The technical owner, like a technical team manager or tech lead
        /// </summary>
        public string TechnicalOwner { get; set; }
        
        /// <summary>
        /// The Business Sub Domain which owns this API. A mandatory field.
        /// </summary>
        public string SubDomainName { get; set; }
        
        /// <summary>
        /// List of tags used to classify the API on further dimensions.
        /// </summary>
        public List<string> Tags { get; set; }
        
        /// <summary>
        /// All versions of the API
        /// </summary>
        public List<VersionApi> Versions { get; set; }
    }
}
