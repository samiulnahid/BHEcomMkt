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

        public async Task<Product> GetProductByIdAsync(Guid id)
        {
            return await _productRepository.GetByIdAsync(id);
        }
         public async Task<IEnumerable<Product>> GetProductByCategoryIdAsync(Guid id)
        {
            return await _productRepository.GetByCategoryIdAsync(id);
        }

        public async Task<Guid> AddProductAsync(Product product)
        {
            return await _productRepository.AddAsync(product);
        }

        public async Task<bool> UpdateProductAsync(Product product)
        {
            return await _productRepository.UpdateAsync(product);
        }

        public async Task DeleteProductAsync(Guid id)
        {
            await _productRepository.DeleteAsync(id);
        }
    }

}
