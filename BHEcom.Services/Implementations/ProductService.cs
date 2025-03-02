using BHEcom.Common.Models;
using BHEcom.Data.Repositories;
using BHEcom.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHEcom.Services.Implementations
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<IEnumerable<Product>> GetAllProductsByStoreIdAsync(Guid id)
        {
            return await _productRepository.GetAllByStoreIdAsync(id);
        } 
        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return await _productRepository.GetAllAsync();
        }
         public async Task<IEnumerable<Product>> GetRandomProductsAsync(int num)
        {
            return await _productRepository.GetRandomProductsAsync(num);
        }
         public async Task<IEnumerable<Product>> GetLatestProductsAsync(int num)
        {
            return await _productRepository.GetLatestProductsAsync(num);
        }

        public async Task<(IEnumerable<Product>? Products, int? TotalCount)> FilterAllProduct(ProductFilter filterEntity)
        {
            return await _productRepository.FilterAllProduct(filterEntity);
        }
        // public async Task<(List<Product>? Products, int? TotalCount)> SearchProductsAsync(string searchTerm, ProductFilter filter)
        //{
        //    return await _productRepository.SearchProductsAsync(searchTerm, filter);
        //}
        public async Task<(List<Product>? Products, int? TotalCount)> SearchProductsAsync(ProductFilter filter)
        {
            return await _productRepository.SearchProductsAsync(filter);
        }

        public async Task<Product> GetProductByIdAsync(Guid id)
        {
            return await _productRepository.GetByIdAsync(id);
        }
        public async Task<(IEnumerable<Product> Products, int TotalCount)> GetProductByCategoryIdAsync(Guid id, int pageNumber, int pageSize)
        {
            return await _productRepository.GetByCategoryIdAsync(id, pageNumber, pageSize);
        }
         public async Task<(IEnumerable<Product> Products, int TotalCount)> GetProductByBrandIdAsync(Guid id, int pageNumber, int pageSize)
        {
            return await _productRepository.GetByBrandIdAsync(id, pageNumber, pageSize);
        }

        public async Task<Guid> AddProductAsync(Product product)
        {
            return await _productRepository.AddAsync(product);
        }

        public async Task<(bool isUpdated, string? oldImageUrl)> UpdateProductAsync(Product product)
        {
            return await _productRepository.UpdateAsync(product);
        }
        public async Task<bool> UpdateProductDescripAsync(Product product)
        {
            return await _productRepository.UpdateDescripAsync(product);
        }

        public async Task<(bool isDelete, string? oldImageUrl)> DeleteProductAsync(Guid id)
        {
            return await _productRepository.DeleteAsync(id);
        } 
        public async Task<List<Product>> GetTopSellingRandomProductsAsync(int featureCount, int randomCount)
        {
            return await _productRepository.GetTopSellingRandomProductsAsync(featureCount, randomCount);
        }

        public async Task<(List<Product> TopSellingProducts, int TotalCount)> GetAllTopSellingProductsAsync(int featureCount, ProductFilter filter)
        {
            return await _productRepository.GetAllTopSellingProductsAsync(featureCount, filter);
        }


        public async Task<PageVisit> GetPageVisitAsync(Guid productId, string type, string currentMonth)
        {
            return await _productRepository.GetPageVisitAsync(productId, type, currentMonth);
        }

        public async Task<bool> CreatePageVisitAsync(PageVisit pageVisit)
        {
            return await _productRepository.CreatePageVisitAsync(pageVisit);
        }

        public async Task<bool> UpdatePageVisitAsync(PageVisit pageVisit)
        {
            return await _productRepository.UpdatePageVisitAsync(pageVisit);
        }
    }

}
