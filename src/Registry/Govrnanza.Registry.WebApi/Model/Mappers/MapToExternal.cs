using Govrnanza.Registry.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Govrnanza.Registry.WebApi.Model.Mappers
{
    internal partial class Map
    {
        internal static BusinessDomainExternal ToExternal(BusinessDomain domain)
        {
            return new BusinessDomainExternal()
            {
                Name = domain.Name,
                SubDomains = domain.SubDomains.Select(x => ToExternal(x)).ToList()
            };
        }

        internal static IEnumerable<BusinessDomainExternal> ToExternal(IEnumerable<BusinessDomain> domains)
        {
            return domains.Select(x => ToExternal(x));
        }

        internal static BusinessSubDomainExternal ToExternal(BusinessSubDomain subDomain)
        {
            return new BusinessSubDomainExternal()
            {
                Name = subDomain.Name,
                Description = subDomain.Description,
                ParentBusinessDomain = subDomain.Parent.Name
            };
        }

        internal static IEnumerable<BusinessSubDomainExternal> ToExternal(IEnumerable<BusinessSubDomain> subDomains)
        {
            return subDomains.Select(x => ToExternal(x));
        }

        internal static ApiExternal ToExternal(Api api, IEnumerable<Tag> tags)
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

        internal static IEnumerable<ApiExternal> ToExternal(IEnumerable<Api> apis, IEnumerable<ApiTag> apiTags)
        {
            foreach(var api in apis)
            {
                var apiTagsOfApi = apiTags.Where(x => x.ApiId.Equals(api.Id))
                    .Select(x => x.Tag.Name)
                    .ToList();

                yield return new ApiExternal()
                {
                    Description = api.Description,
                    Name = api.Name,
                    SubDomainName = api.BusinessSubDomain.Name,
                    Tags = apiTagsOfApi
                };
            }
        }
    }
}
