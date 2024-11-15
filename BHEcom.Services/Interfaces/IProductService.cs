using BHEcom.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHEcom.Services.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetAllProductsByStoreIdAsync(Guid id);
        Task<IEnumerable<Product>> GetAllProductsAsync();
        Task<IEnumerable<Product>> GetRandomProductsAsync(int num);
       
        Task<Product> GetProductByIdAsync(Guid id);
        Task<IEnumerable<Product>> GetProductByCategoryIdAsync(Guid id);
        Task<Guid> AddProductAsync(Product product);
        Task<bool> UpdateProductAsync(Product product);
        Task DeleteProductAsync(Guid id);
    }

}
