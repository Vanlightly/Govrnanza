using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Govrnanza.Registry.WebApi.Model.Apis
{
    /// <summary>
    /// An API command object for updating core API fields. Only fields that are set will be updated.
    /// </summary>
    public class ApiUpdate
    {
        /// <summary>
        /// MANDATORY. The name of API, used as an identifier
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// OPTIONAL. The new name of API, used as an identifier
        /// </summary>
        public string NewName { get; set; }

        /// <summary>
        /// OPTIONAL. New title of the API for display purposes
        /// </summary>
        public string NewTitle { get; set; }

        /// <summary>
        /// OPTIONAL. New short description about the API
        /// </summary>
        public string NewDescription { get; set; }

        /// <summary>
        /// OPTIONAL. The person on the business side who owns this API
        /// </summary>
        public string NewBusinessOwner { get; set; }

        /// <summary>
        /// OPTIONAL. The person on the technical side who owns this API
        /// </summary>
        public string NewTechnicalOwner { get; set; }

        /// <summary>
        /// OPTIONAL. The tags that should be associated with this API
        /// </summary>
        public IList<string> NewTags { get; set; }
    }
}
