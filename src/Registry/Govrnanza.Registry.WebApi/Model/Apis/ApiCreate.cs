using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Govrnanza.Registry.WebApi.Model.Apis
{
    /// <summary>
    /// An API command object for creating a new API
    /// </summary>
    public class ApiCreate
    {
        /// <summary>
        /// MANDATORY. The name of API, used as an identifier
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// MANDATORY. Title of the API for display purposes
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// MANDATORY. Short description about the API
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// MANDATORY. The Business Sub Domain which owns this API.
        /// </summary>
        public string SubDomainName { get; set; }

        /// <summary>
        /// MANDATORY. The person on the business side who owns this API
        /// </summary>
        public string BusinessOwner { get; set; }

        /// <summary>
        /// MANDATORY. The person on the technical side who owns this API
        /// </summary>
        public string TechnicalOwner { get; set; }

        /// <summary>
        /// The tags to associate to this new API
        /// </summary>
        public IList<string> Tags { get; set; }
    }
}
