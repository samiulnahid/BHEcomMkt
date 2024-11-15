using BHEcom.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHEcom.Services.Interfaces
{
    public interface IStoreService
    {
        Task<IEnumerable<Store>> GetAllStoresAsync();
        Task<IEnumerable<StoreManager>> GetAllStoreManagersAsync();
        Task<StoreManager> GetStoreByIdAsync(Guid id);
        Task<Guid> AddStoreAsync(Store storte);
        Task<bool> UpdateStoreAsync(Store store);
        Task<bool> DeleteStoreAsync(Guid id);
        Task<StoreConfig> GetStoreConfigAsync(Guid id);
        Task<bool> DeleteStoreConfigAsync(Guid id);
        Task<bool> CreateStoreConfigAsync(StoreConfig stores);
        
        Task<bool> CreateStoreProductFieldAsync(List<StoreProductField> storeProductFieldList);
        Task<List<StoreProductField>> GetStoreProductFieldsByStoreId(Guid id);
        Task<List<StoreProductField>> GetProductFieldsByCategoryId(Guid id);
        Task<bool> DeleteStoreProductFieldAsync(Guid id);
    }
}
