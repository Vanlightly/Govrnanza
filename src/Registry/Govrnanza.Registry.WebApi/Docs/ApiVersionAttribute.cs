using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Govrnanza.Registry.WebApi.Docs
{
    /// <summary>
    /// Temporary workaround until Microsoft.AspNetCore.Mvc.Versioning is ported to 2.0
    /// </summary>
    public class ApiVersionAttribute : Attribute
    {
        /// <summary>
        /// Versions
        /// </summary>
        public List<string> Versions { get; set; }

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="version"></param>
        public ApiVersionAttribute(string version)
        {
            Versions = new List<string>();
            Versions.Add(version);
        }

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="versions"></param>
        public ApiVersionAttribute(params string[] versions)
        {
            Versions = versions.ToList();
        }
    }
}
