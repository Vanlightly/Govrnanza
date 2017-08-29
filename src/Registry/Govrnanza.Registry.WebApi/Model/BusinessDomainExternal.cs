using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Govrnanza.Registry.WebApi.Model
{
    /// <summary>
    /// Business domain
    /// </summary>
    public class BusinessDomainExternal
    {
        /// <summary>
        /// Name that identifies the domain
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// Child sub domains
        /// </summary>
        public List<BusinessSubDomainExternal> SubDomains { get; set; }
    }
}
