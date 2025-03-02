using BHEcom.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHEcom.Data.Repositories
{

    public interface IProductDetailRepository
    {
        Task<bool> AddAsync(ProductDetail productDetail);
        Task<ProductDetail> GetByIdAsync(Guid id);
        Task<IEnumerable<ProductDetail>> GetAllAsync();
        Task<bool> UpdateAsync(ProductDetail productDetail);
        Task DeleteAsync(Guid id);
        Task<IEnumerable<ProductDetail>> GetAllByProductIdAsync(Guid id);
    }
}
