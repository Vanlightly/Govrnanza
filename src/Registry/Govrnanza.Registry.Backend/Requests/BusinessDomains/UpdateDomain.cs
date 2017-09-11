using Govrnanza.Registry.Backend.Responses;
using Govrnanza.Registry.Core.Model;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Govrnanza.Registry.Backend.Requests.BusinessDomains
{
    /// <summary>
    /// Name is mandatory
    /// All other properties are optional, set only those fields which should be updated
    /// </summary>
    public class UpdateDomain : IRequest<Response<UpdateResult>>
    {
        public string Name { get; set; }

        public string NewName { get; set; }
        public IList<UpdateSubDomain> UpdatedSubDomains { get; set; }
    }
}
