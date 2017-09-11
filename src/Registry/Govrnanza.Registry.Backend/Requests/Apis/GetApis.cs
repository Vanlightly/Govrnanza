using Govrnanza.Registry.Backend.Responses;
using Govrnanza.Registry.Core.Model;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Govrnanza.Registry.Backend.Requests.Apis
{
    public class GetApis : IRequest<Response<IEnumerable<Api>>>
    {
        public string ApiName { get; set; }
    }
}
