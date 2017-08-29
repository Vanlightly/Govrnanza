using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Govrnanza.Registry.Core.Model
{
    public class BusinessSubDomain
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public Guid ParentId { get; set; }
        public BusinessDomain Parent { get; set; }
    }
}
