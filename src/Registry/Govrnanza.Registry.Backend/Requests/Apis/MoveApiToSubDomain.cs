using Govrnanza.Registry.Backend.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Govrnanza.Registry.Backend.Requests.Apis
{
    public class MoveApiToSubDomain : IRequest<Response<MoveResult>>
    {
        public MoveApiToSubDomain()
        {
            RequestId = Guid.NewGuid();
        }

        public Guid RequestId { get; private set; }
        public string ApiName { get; set; }

        public string CurrentBusinessSubDomainName { get; set; }
        public string DestinationBusinessSubDomainName { get; set; }
    }
}
