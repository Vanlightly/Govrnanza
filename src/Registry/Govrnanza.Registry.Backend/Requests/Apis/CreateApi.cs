using Govrnanza.Registry.Backend.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Govrnanza.Registry.Backend.Requests.Apis
{
    public class CreateApi : IRequest<Response<CreateResult>>
    {
        public CreateApi()
        {
            RequestId = Guid.NewGuid();
        }

        public Guid RequestId { get; private set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string BusinessOwner { get; set; }
        public string TechnicalOwner { get; set; }
        public Guid BusinessSubDomainId { get; set; }
        public IList<string> Tags { get; set; }
    }
}
