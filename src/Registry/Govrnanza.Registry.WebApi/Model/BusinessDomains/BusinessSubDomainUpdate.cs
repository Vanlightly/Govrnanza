using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Govrnanza.Registry.WebApi.Model.BusinessDomains
{
    /// <summary>
    /// Business sub domain update request
    /// </summary>
    public class BusinessSubDomainUpdate
    {
        /// <summary>
        /// MANDATORY. Name that identifies the sub domain
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// OPTIONAL. The new name that identifies the sub domain
        /// </summary>
        public string NewName { get; set; }

        /// <summary>
        /// OPTIONAL. New short description of the sub domain
        /// </summary>
        public string NewDescription { get; set; }
    }
}
