using Govrnanza.Registry.Backend.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Govrnanza.Registry.Backend.Requests.Apis
{
    public class DeleteApiVersion : IRequest<Response<DeleteResult>>
    {
        public DeleteApiVersion()
        {
            RequestId = Guid.NewGuid();
        }

        public Guid RequestId { get; private set; }
        public int MajorVersion { get; set; }
        public int MinorVersion { get; set; }
        public string ApiName { get; set; }
    }
}
