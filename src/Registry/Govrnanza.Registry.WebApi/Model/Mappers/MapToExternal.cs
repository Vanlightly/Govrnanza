using Govrnanza.Registry.WebApi.Model.External;
using Govrnanza.Registry.WebApi.Model.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Govrnanza.Registry.WebApi.Model.Mappers
{
    public partial class Map
    {
        public static BusinessDomainExternal ToExternal(BusinessDomain domain)
        {
            return new BusinessDomainExternal()
            {
                Name = domain.Name,
                SubDomains = domain.SubDomains.Select(x => ToExternal(x)).ToList()
            };
        }

        public static IEnumerable<BusinessDomainExternal> ToExternal(IEnumerable<BusinessDomain> domains)
        {
            return domains.Select(x => ToExternal(x));
        }

        public static BusinessSubDomainExternal ToExternal(BusinessSubDomain subDomain)
        {
            return new BusinessSubDomainExternal()
            {
                Name = subDomain.Name,
                Description = subDomain.Description,
                ParentBusinessDomain = subDomain.Parent.Name
            };
        }

        public static IEnumerable<BusinessSubDomainExternal> ToExternal(IEnumerable<BusinessSubDomain> subDomains)
        {
            return subDomains.Select(x => ToExternal(x));
        }

        public static ApiExternal ToExternal(Api api, IEnumerable<Tag> tags)
        {
            var apiExternal = new ApiExternal()
            {
                Description = api.Description,
                Name = api.Name,
                SubDomainName = api.BusinessSubDomain.Name,
                Tags = tags.Select(x => x.Name).ToList()
            };

            return apiExternal;
        }

        public static IEnumerable<ApiExternal> ToExternal(IEnumerable<Api> apis, IEnumerable<ApiTag> apiTags)
        {
            var groupedByApi = apiTags.GroupBy(x => x.ApiId);
                                
            foreach(var group in groupedByApi)
            {
                var api = apis.First(x => x.Id.Equals(group.Key));
                yield return new ApiExternal()
                {
                    Description = api.Description,
                    Name = api.Name,
                    SubDomainName = api.BusinessSubDomain.Name,
                    Tags = group.Select(x => x.Tag.Name).ToList()
                };
            }
        }
    }
}
