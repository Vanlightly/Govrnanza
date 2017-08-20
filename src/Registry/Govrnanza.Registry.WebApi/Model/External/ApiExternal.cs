using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Govrnanza.Registry.WebApi.Model.External
{
    public class ApiExternal
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string SubDomainName { get; set; }
        public List<string> Tags { get; set; }
    }
}
