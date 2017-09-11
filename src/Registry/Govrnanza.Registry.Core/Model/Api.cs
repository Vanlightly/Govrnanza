using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Govrnanza.Registry.Core.Model
{
    public class Api
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string BusinessOwner { get; set; }
        public string TechnicalOwner { get; set; }
        public IList<ApiVersion> Versions { get; set; }
        public IList<ApiTag> ApiTags { get; set; }
        public Guid BusinessSubDomainId { get; set; }
        public BusinessSubDomain BusinessSubDomain { get; set; }
    }
}
