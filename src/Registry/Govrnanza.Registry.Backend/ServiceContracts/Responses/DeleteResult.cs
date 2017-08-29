using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Govrnanza.Registry.Backend.ServiceContracts.Responses
{
    public enum DeleteResult
    {
        NotFound,
        Deleted,
        NotDeletedDueToDependentObjects
    }
}
