using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Govrnanza.Registry.Core.Model
{
    public class BusinessDomain
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<BusinessSubDomain> SubDomains { get; set; }
    }
}
