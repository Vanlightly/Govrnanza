using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Govrnanza.Registry.WebApi.Model.Internal
{
    public class Api
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public Guid BusinessSubDomainId { get; set; }
        public BusinessSubDomain BusinessSubDomain { get; set; }
    }
}
