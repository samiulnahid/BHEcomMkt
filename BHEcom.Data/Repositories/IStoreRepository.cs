using BHEcom.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHEcom.Data.Repositories
{
    public interface IStoreRepository
    {
        Task<Guid> AddAsync(Store store);
        Task<Store> GetByIdAsync(Guid id);
        //Task<Store> GetByUserIdAsync(Guid id);
        //Task<IEnumerable<Store>> GetAllAsync();
        Task<(bool isUpdated, string? oldImageUrl)> UpdateAsync(Store store);
        Task<bool> DeleteAsync(Guid id);
        Task<IEnumerable<StoreManager>> GetAllStoreManagersAsync();
        Task<IEnumerable<Store>> GetAllStoresAsync();
        Task<StoreManager> GetStoreManagerByStoreIdAsync(Guid storeId);
        Task<StoreConfig> GetBrandAndCategoryByStoreIdAsync(Guid storeId);
        Task<bool> DeletetBrandAndCategoryByStoreIdAsync(Guid storeId);
        Task<bool> CreateBrandAndCategoryByStoreIdAsync(StoreConfig stores);
        Task<bool> CreateProductFieldAsync(List<StoreProductField> productFieldList);
        Task<bool> UpdateProductFieldsAsync(List<StoreProductField> productFieldList);
        Task<List<StoreProductField>> GetProductFieldsByStoreId(Guid id);
        Task<List<StoreProductField>> GetProductFieldsByCategoryId(Guid id);
        Task<bool> DeleteStoreProductField(Guid id);
        Task<(bool Success, string Message, List<Guid>? CategoryIds)> DeleteStoreBrandandStoreCategoryAsync(StoreConfig storeConfig);

    }

}
