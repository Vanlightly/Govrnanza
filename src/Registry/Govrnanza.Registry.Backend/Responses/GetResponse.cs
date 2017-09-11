using System;
using System.Collections.Generic;
using System.Text;

namespace Govrnanza.Registry.Backend.Responses
{
    public class GetResponse<T>
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
