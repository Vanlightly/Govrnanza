using System;
using System.Collections.Generic;
using System.Text;

namespace Govrnanza.Registry.Backend.Responses
{
    public class Response<TResult>
    {
        public Response(TResult result)
        {
            Result = result;
        }

        public Response(TResult result, string description)
        {
            Result = result;
            Description = description;
        }

        public TResult Result { get; set; }
        public string Description { get; set; }
    }
}
