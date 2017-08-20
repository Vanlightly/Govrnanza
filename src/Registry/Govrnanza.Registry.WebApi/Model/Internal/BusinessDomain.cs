using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Govrnanza.Registry.WebApi.Model.Internal
{
    public class BusinessDomain
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<BusinessSubDomain> SubDomains { get; set; }
    }
}
