using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Govrnanza.Registry.WebApi.Model.BusinessDomains
{
    /// <summary>
    /// Business sub domain
    /// </summary>
    public class BusinessSubDomain
    {
        /// <summary>
        /// Name that identifies the sub domain
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Short description of the sub domain
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The name of the business domain that owns this sub domain
        /// </summary>
        public string ParentBusinessDomain { get; set; }
    }
}
