using Internal = Govrnanza.Registry.Core.Model;
using External = Govrnanza.Registry.WebApi.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Govrnanza.Registry.WebApi.Model.Mappers
{
    //internal partial class Map
    //{
    //    internal static Internal.BusinessDomain ToInternal(External.BusinessDomains.BusinessDomain domainExternal)
    //    {
    //        var domain = new Internal.BusinessDomain()
    //        {
    //            Name = domainExternal.Name,
    //        };

    //        if (domainExternal.SubDomains != null && domainExternal.SubDomains.Any())
    //        {
    //            domain.SubDomains = domainExternal.SubDomains.Select(x => new Internal.BusinessSubDomain()
    //            {
    //                Name = x.Name,
    //                Description = x.Description
                    
    //            }).ToList();
    //        }

    //        return domain;
    //    }

    //    internal static Internal.BusinessSubDomain ToInternal(External.BusinessDomains.BusinessSubDomain subDomainExternal)
    //    {
    //        return new Internal.BusinessSubDomain()
    //        {
    //            Description = subDomainExternal.Description,
    //            Name = subDomainExternal.Name
    //        };
    //    }

    //    internal static Internal.Api ToInternal(External.Apis.ApiCreate apiExternal, Guid businessDomainId)
    //    {
    //        return new Internal.Api()
    //        {
    //            Name = apiExternal.Name,
    //            Description = apiExternal.Description,
    //            BusinessSubDomainId = businessDomainId
    //        };
    //    }
    //}
}
