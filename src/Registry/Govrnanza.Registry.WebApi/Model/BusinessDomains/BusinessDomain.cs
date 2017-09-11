using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Govrnanza.Registry.WebApi.Model.BusinessDomains
{
    /// <summary>
    /// Business domain
    /// </summary>
    public class BusinessDomain
    {
        /// <summary>
        /// Name that identifies the domain
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// A description of the business domain
        /// </summary>
        public string Description { get; set; }
        
        /// <summary>
        /// Child sub domains
        /// </summary>
        public List<BusinessSubDomain> SubDomains { get; set; }
    }
}
