using Internal = Govrnanza.Registry.Core.Model;
using External = Govrnanza.Registry.WebApi.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Govrnanza.Registry.WebApi.Model.Mappers
{
    internal partial class Map
    {
        internal static External.BusinessDomains.BusinessDomain ToExternal(Internal.BusinessDomain domain) => new External.BusinessDomains.BusinessDomain()
        {
            Name = domain.Name,
            Description = domain.Description,
            SubDomains = domain.SubDomains.Select(x => ToExternal(x)).ToList()
        };

        internal static IEnumerable<External.BusinessDomains.BusinessDomain> ToExternal(IEnumerable<Internal.BusinessDomain> domains) => domains.Select(x => ToExternal(x));

        internal static External.BusinessDomains.BusinessSubDomain ToExternal(Internal.BusinessSubDomain subDomain) => new External.BusinessDomains.BusinessSubDomain()
        {
            Name = subDomain.Name,
            Description = subDomain.Description,
            ParentBusinessDomain = subDomain.Parent.Name
        };

        internal static IEnumerable<External.BusinessDomains.BusinessSubDomain> ToExternal(IEnumerable<Internal.BusinessSubDomain> subDomains) => subDomains.Select(x => ToExternal(x));

        internal static External.Apis.ApiFull ToExternal(Internal.Api api)
        {
            var apiExternal = new External.Apis.ApiFull()
            {
                BusinessOwner = api.BusinessOwner,
                Description = api.Description,
                Name = api.Name,
                SubDomainName = api.BusinessSubDomain.Name,
                Tags = api.ApiTags.Select(x => x.Tag.Name).ToList(),
                TechnicalOwner = api.TechnicalOwner,
                Title = api.Title,
                Versions = api.Versions.Select(x => new VersionApi()
                {
                    ApiName = api.Name,
                    MajorVersion = x.MajorVersion,
                    MinorVersion = x.MinorVersion,
                    Status = (VersionStatusExternal)(int)x.Status
                }).ToList()
            };

            return apiExternal;
        }

        internal static IEnumerable<External.Apis.ApiFull> ToExternal(IEnumerable<Internal.Api> apis) => apis.Select(x => ToExternal(x));
    }
}
