using BHEcom.Common.Models;
using BHEcom.Data.Repositories;
using BHEcom.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BHEcom.Services.Implementations
{
    public class StoreService : IStoreService
    {
        private readonly IStoreRepository _storeRepository;

        public StoreService(IStoreRepository storeRepository)
        {
            _storeRepository = storeRepository;
        }
        public async Task<IEnumerable<StoreManager>> GetAllStoreManagersAsync()
        {
            return await _storeRepository.GetAllStoreManagersAsync();
        }
        public async Task<IEnumerable<Store>> GetAllStoresAsync()
        {
            return await _storeRepository.GetAllStoresAsync();
        }

        public async Task<StoreManager> GetStoreByIdAsync(Guid id)
        {
            return await _storeRepository.GetStoreManagerByStoreIdAsync(id);
        }

        public async Task<Guid> AddStoreAsync(Store store)
        {
           return await _storeRepository.AddAsync(store);
        }

        public async Task<(bool isUpdated, string? oldImageUrl)> UpdateStoreAsync(Store store)
        {
           return await _storeRepository.UpdateAsync(store);
        }

         public async Task<bool> DeleteStoreAsync(Guid id)
        {
             return await _storeRepository.DeleteAsync(id);
        }
      
        public async Task<StoreConfig> GetStoreConfigAsync(Guid id)
        {
            return await _storeRepository.GetBrandAndCategoryByStoreIdAsync(id);
        }
        public async Task<bool> DeleteStoreConfigAsync(Guid id)
        {
             return await _storeRepository.DeletetBrandAndCategoryByStoreIdAsync(id);
        }
         public async Task<bool> CreateStoreConfigAsync(StoreConfig stores)
        {
             return await _storeRepository.CreateBrandAndCategoryByStoreIdAsync(stores);
        }

        public async Task<bool> CreateStoreProductFieldAsync(List<StoreProductField> storeProductFieldList)
        {
             return await _storeRepository.CreateProductFieldAsync(storeProductFieldList);
        }
         public async Task<bool> UpdateProductFieldsAsync(List<StoreProductField> storeProductFieldList)
        {
             return await _storeRepository.UpdateProductFieldsAsync(storeProductFieldList);
        }
       
        public async Task<List<StoreProductField>> GetStoreProductFieldsByStoreId(Guid id)
        {
            return await _storeRepository.GetProductFieldsByStoreId(id);
        } 
        public async Task<List<StoreProductField>> GetProductFieldsByCategoryId(Guid id)
        {
            return await _storeRepository.GetProductFieldsByCategoryId(id);
        }

        public async Task<bool> DeleteStoreProductFieldAsync(Guid id)
        {
            return await _storeRepository.DeleteStoreProductField(id);
        }
        public async Task<(bool Success, string Message, List<Guid>? CategoryIds)> DeleteStoreBrandandStoreCategoryAsync(StoreConfig storeConfig)
        {
            return await _storeRepository.DeleteStoreBrandandStoreCategoryAsync(storeConfig);
        }

    }
}

