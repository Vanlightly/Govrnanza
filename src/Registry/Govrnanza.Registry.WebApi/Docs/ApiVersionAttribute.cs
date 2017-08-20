using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Govrnanza.Registry.WebApi.Docs
{
    public class ApiVersionAttribute : Attribute
    {
        public List<string> Versions { get; set; }

        public ApiVersionAttribute(string version)
        {
            Versions = new List<string>();
            Versions.Add(version);
        }

        public ApiVersionAttribute(params string[] versions)
        {
            Versions = versions.ToList();
        }
    }
}
