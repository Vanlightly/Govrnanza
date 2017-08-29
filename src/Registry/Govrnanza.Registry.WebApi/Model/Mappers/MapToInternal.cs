using Govrnanza.Registry.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Govrnanza.Registry.WebApi.Model.Mappers
{
    internal partial class Map
    {
        internal static BusinessDomain ToInternal(BusinessDomainExternal domainExternal)
        {
            var domain = new BusinessDomain()
            {
                Name = domainExternal.Name,
            };

            if (domainExternal.SubDomains != null && domainExternal.SubDomains.Any())
            {
                domain.SubDomains = domainExternal.SubDomains.Select(x => new BusinessSubDomain()
                {
                    Name = x.Name,
                    Description = x.Description
                    
                }).ToList();
            }

            return domain;
        }

        internal static BusinessSubDomain ToInternal(BusinessSubDomainExternal subDomainExternal)
        {
            return new BusinessSubDomain()
            {
                Description = subDomainExternal.Description,
                Name = subDomainExternal.Name
            };
        }

        internal static Api ToInternal(ApiExternal apiExternal, Guid businessDomainId)
        {
            return new Api()
            {
                Name = apiExternal.Name,
                Description = apiExternal.Description,
                BusinessSubDomainId = businessDomainId
            };
        }
    }
}
