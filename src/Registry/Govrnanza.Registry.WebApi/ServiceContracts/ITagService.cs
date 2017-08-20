using Govrnanza.Registry.WebApi.Model.Internal;
using Govrnanza.Registry.WebApi.ServiceContracts.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Govrnanza.Registry.WebApi.ServiceContracts
{
    public interface ITagService
    {
        Task<IEnumerable<ApiTag>> GetApiTagsAsync();
        Task<IEnumerable<Tag>> GetTagsAsync();
        Task<IEnumerable<Tag>> GetTagsAsync(IEnumerable<string> tags);
        Task<IEnumerable<Tag>> GetTagsOfApiAsync(string apiName);
        Task<UpdateResult> AddTagToApiAsync(string apiName, string tagName);
        Task<UpdateResult> UpdateTagsOfApiAsync(string apiName, IEnumerable<string> tags);
        Task<DeleteResult> DeleteTagsOfApiAsync(string apiName, IEnumerable<string> tags);
    }
}
