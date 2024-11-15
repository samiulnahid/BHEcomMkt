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
    public class ProductDetailService : IProductDetailService
    {
        private readonly IProductDetailRepository _productDetailRepository;

        public ProductDetailService(IProductDetailRepository productDetailRepository)
        {
            _productDetailRepository = productDetailRepository;
        }

        public async Task<IEnumerable<ProductDetail>> GetAllProductDetailsAsync()
        {
            return await _productDetailRepository.GetAllAsync();
        }

        public async Task<ProductDetail> GetProductDetailByIdAsync(Guid id)
        {
            return await _productDetailRepository.GetByIdAsync(id);
        }

        public async Task<bool> AddProductDetailAsync(ProductDetail productDetail)
        {
           return await _productDetailRepository.AddAsync(productDetail);
        }

        public async Task UpdateProductDetailAsync(ProductDetail productDetail)
        {
            await _productDetailRepository.UpdateAsync(productDetail);
        }

        public async Task DeleteProductDetailAsync(Guid id)
        {
            await _productDetailRepository.DeleteAsync(id);
        } 
        public async Task<IEnumerable<ProductDetail>> GetAllProductDetailByProductId(Guid id)
        {
            return await _productDetailRepository.GetAllByProductIdAsync(id);
        }
    }

}
