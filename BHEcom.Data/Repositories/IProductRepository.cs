using BHEcom.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHEcom.Data.Repositories
{
    public interface IProductRepository
    {
        Task<Guid> AddAsync(Product product);
        Task<Product> GetByIdAsync(Guid id);
        Task<IEnumerable<Product>> GetByCategoryIdAsync(Guid id);
        Task<IEnumerable<Product>> GetAllAsync();
        Task<IEnumerable<Product>> GetAllByStoreIdAsync(Guid id);
        Task<IEnumerable<Product>> GetRandomProductsAsync(int num);
        Task<IEnumerable<Product>> GetLatestProductsAsync(int num);
        Task<bool> UpdateAsync(Product product);
        Task DeleteAsync(Guid id);
        Task<(IEnumerable<Product>? Products, int? TotalCount)> FilterAllProduct(ProductFilter filterEntity);


    }
}
