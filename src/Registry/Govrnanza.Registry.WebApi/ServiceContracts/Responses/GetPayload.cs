using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Govrnanza.Registry.WebApi.ServiceContracts.Responses
{
    public class GetPayload<T>
    {
        public GetResult Result { get; set; }
        public T Data { get; set; }
    }

    public enum GetResult
    {
        NotFound,
        Retrieved
    }
}
