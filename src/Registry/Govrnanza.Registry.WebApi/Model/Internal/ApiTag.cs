using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Govrnanza.Registry.WebApi.Model.Internal
{
    public class ApiTag
    {
        public Guid Id { get; set; }
        public Guid ApiId { get; set; }
        public Api Api { get; set; }

        public Guid TagId { get; set; }
        public Tag Tag { get; set; }
    }
}
