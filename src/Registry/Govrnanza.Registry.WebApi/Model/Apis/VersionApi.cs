using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Govrnanza.Registry.WebApi.Model
{
    /// <summary>
    /// Represents the version of an API
    /// </summary>
    public class VersionApi
    {
        /// <summary>
        /// The name of the API
        /// </summary>
        public string ApiName { get; set; }
        /// <summary>
        /// Major version
        /// </summary>
        public int MajorVersion { get; set; }
        /// <summary>
        /// Minor version
        /// </summary>
        public int MinorVersion { get; set; }
        /// <summary>
        /// Current status of the version
        /// </summary>
        public VersionStatusExternal Status { get; set; }
    }
}
