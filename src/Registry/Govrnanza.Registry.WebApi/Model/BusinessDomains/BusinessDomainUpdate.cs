using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Govrnanza.Registry.WebApi.Model.BusinessDomains
{
    /// <summary>
    /// Update Business domain request
    /// </summary>
    public class BusinessDomainUpdate
    {
        /// <summary>
        /// MANDATORY. Name that identifies the domain
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// OPTIONAL. The new name that identifies the domain
        /// </summary>
        public string NewName { get; set; }

        /// <summary>
        /// OPTIONAL. The updated list of child sub domains
        /// </summary>
        public List<BusinessSubDomainUpdate> SubDomains { get; set; }
    }
}
