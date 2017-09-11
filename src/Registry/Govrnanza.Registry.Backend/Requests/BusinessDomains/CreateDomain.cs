using Govrnanza.Registry.Backend.Responses;
using Govrnanza.Registry.Core.Model;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Govrnanza.Registry.Backend.Requests.BusinessDomains
{
    public class CreateDomain : IRequest<Response<CreateResult>>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public IList<CreateSubDomain> SubDomains { get; set; }
    }
}
