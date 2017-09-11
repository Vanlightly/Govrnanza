using Govrnanza.Registry.Backend.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Govrnanza.Registry.Backend.Requests.Apis
{
    public class SetApiTags : IRequest<Response<UpdateResult>>
    {
        public SetApiTags()
        {
            Tags = new List<string>();
        }

        public Guid RequestId { get; private set; } = Guid.NewGuid();
        public string ApiName { get; set; }
        public IList<string> Tags { get; set; }

        public bool Commit { get; set; } = true;
        public bool UseLocal { get; set; } = false;
    }
}
