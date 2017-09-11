using System;
using System.Collections.Generic;
using System.Text;

namespace Govrnanza.Registry.Backend.Responses
{
    public enum CreateResult
    {
        Created,
        NotCreated,
        DependentObjectNotFound
    }
}
