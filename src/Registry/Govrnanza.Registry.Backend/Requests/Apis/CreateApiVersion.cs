using Govrnanza.Registry.Backend.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Govrnanza.Registry.Backend.Requests.Apis
{
    public class CreateApiVersion : IRequest<Response<CreateResult>>
    {
        public CreateApiVersion()
        {
            RequestId = Guid.NewGuid();
        }

        public Guid RequestId { get; private set; }
        public int MajorVersion { get; set; }
        public int MinorVersion { get; set; }
        public string ApiName { get; set; }
    }
}
