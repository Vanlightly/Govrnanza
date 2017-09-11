using System;
using System.Collections.Generic;
using System.Text;

namespace Govrnanza.Registry.Core.Model
{
    public class ApiVersion
    {
        public int MajorVersion { get; set; }
        public int MinorVersion { get; set; }
        public VersionStatus Status { get; set; }

        public Guid ApiId { get; set; }
        public Api Api { get; set; }
    }
}
