using Govrnanza.Registry.Backend.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Govrnanza.Registry.Backend.Requests.Apis
{
    public class UpdateApiFields : IRequest<Response<UpdateResult>>
    {
        public UpdateApiFields()
        {
            RequestId = Guid.NewGuid();
        }

        public Guid RequestId { get; private set; }
        
        public string Name { get; set; }

        public string NewName { get; set; } 
        public string NewTitle { get; set; }
        public string NewDescription { get; set; }
        public string NewBusinessOwner { get; set; }
        public string NewTechnicalOwner { get; set; }
        public IList<string> NewTags { get; set; }
    }
}
