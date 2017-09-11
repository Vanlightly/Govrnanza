using Govrnanza.Registry.Backend.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Govrnanza.Registry.Backend.Requests.Tags
{
    public class AddTag : IRequest<Response<CreateResult>>
    {
        public string TagName { get; set; }
        public string ApiName { get; set; }
    }
}
