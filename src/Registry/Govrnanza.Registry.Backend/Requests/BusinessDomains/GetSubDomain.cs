using Govrnanza.Registry.Backend.Responses;
using Govrnanza.Registry.Core.Model;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Govrnanza.Registry.Backend.Requests.BusinessDomains
{
    public class GetSubDomain : IRequest<GetResponse<BusinessSubDomain>>
    {
        public string Name { get; set; }
    }
}
