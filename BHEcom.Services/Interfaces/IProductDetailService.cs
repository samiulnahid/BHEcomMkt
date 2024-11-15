using BHEcom.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHEcom.Services.Interfaces
{
    public interface IProductDetailService
    {
        Task<IEnumerable<ProductDetail>> GetAllProductDetailsAsync();
        Task<IEnumerable<ProductDetail>> GetAllProductDetailByProductId(Guid id);
        Task<ProductDetail> GetProductDetailByIdAsync(Guid id);
        Task<bool> AddProductDetailAsync(ProductDetail productDetail);
        Task UpdateProductDetailAsync(ProductDetail productDetail);
        Task DeleteProductDetailAsync(Guid id);
    }

}
