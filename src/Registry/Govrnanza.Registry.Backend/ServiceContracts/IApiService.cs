using Govrnanza.Registry.Backend.ServiceContracts.Responses;
using Govrnanza.Registry.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Govrnanza.Registry.Backend.ServiceContracts
{
    public interface IApiService
    {
        Task<IEnumerable<Api>> GetApisAsync();
        Task<GetPayload<Api>> GetApiAsync(string name);

        Task AddApiAsync(Api api);
        Task<UpdateResult> UpdateApiAsync(Api api, bool insertIfNotExists);
        Task<DeleteResult> DeleteApiAsync(string apiName);
    }
}
