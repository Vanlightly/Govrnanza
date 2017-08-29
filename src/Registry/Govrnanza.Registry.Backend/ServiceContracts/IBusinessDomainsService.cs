using Govrnanza.Registry.Backend.ServiceContracts.Responses;
using Govrnanza.Registry.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Govrnanza.Registry.Backend.ServiceContracts
{
    public interface IBusinessDomainsService
    {
        Task<IEnumerable<BusinessDomain>> GetDomainsAsync();
        Task<IEnumerable<BusinessSubDomain>> GetSubDomainsAsync();
        Task<GetPayload<BusinessDomain>> GetDomainAsync(string domainName);
        Task<GetPayload<BusinessSubDomain>> GetSubDomainAsync(string subDomainName);
        
        Task AddDomainAsync(BusinessDomain businessDomain);
        Task AddSubDomainAsync(BusinessSubDomain businessSubDomain);
        
        Task<UpdateResult> UpdateDomainAsync(BusinessDomain businessDomain, bool insertIfNotExists);
        Task<UpdateResult> UpdateSubDomainAsync(BusinessSubDomain businessSubDomain, bool insertIfNotExists);
        
        Task<DeleteResult> DeleteDomainAsync(string domainName);
        Task<DeleteResult> DeleteSubDomainAsync(string subDomainName);
    }
}
