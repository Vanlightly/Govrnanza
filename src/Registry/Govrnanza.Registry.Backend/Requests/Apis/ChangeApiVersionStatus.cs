using Govrnanza.Registry.Backend.Responses;
using Govrnanza.Registry.Core.Model;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Govrnanza.Registry.Backend.Requests.Apis
{
    public class ChangeApiVersionStatus : IRequest<Response<UpdateResult>>
    {
        public ChangeApiVersionStatus()
        {
            RequestId = Guid.NewGuid();
        }

        public Guid RequestId { get; private set; }
        public string ApiName { get; set; }
        public int MajorVersion { get; set; }
        public int MinorVersion { get; set; }

        public VersionStatus CurrentStatus { get; set; }
        public VersionStatus NewStatus { get; set; }
    }
}
