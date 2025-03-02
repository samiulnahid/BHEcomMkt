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
        Task<(IEnumerable<Product> Products, int TotalCount)> GetByCategoryIdAsync(Guid id, int pageNumber, int pageSize);
        Task<(IEnumerable<Product> Products, int TotalCount)> GetByBrandIdAsync(Guid id, int pageNumber, int pageSize);
        Task<IEnumerable<Product>> GetAllAsync();
        Task<IEnumerable<Product>> GetAllByStoreIdAsync(Guid id);
        Task<IEnumerable<Product>> GetRandomProductsAsync(int num);
        Task<IEnumerable<Product>> GetLatestProductsAsync(int num);
        Task<(bool isUpdated, string? oldImageUrl)> UpdateAsync(Product product);
        Task<bool> UpdateDescripAsync(Product product);
        Task<(bool isDelete, string? oldImageUrl)> DeleteAsync(Guid id);
        Task<(IEnumerable<Product>? Products, int? TotalCount)> FilterAllProduct(ProductFilter filterEntity);
        Task<List<Product>> GetTopSellingRandomProductsAsync(int featureCount, int randomCount);
        Task<(List<Product> TopSellingProducts, int TotalCount)> GetAllTopSellingProductsAsync(int featureCount,  ProductFilter filter);
       // Task<(List<Product>? Products, int? TotalCount)> SearchProductsAsync(string searchTerm, ProductFilter filter);
        Task<(List<Product>? Products, int? TotalCount)> SearchProductsAsync( ProductFilter filter);
        Task<PageVisit> GetPageVisitAsync(Guid productId, string type, string currentMonth);
        Task<bool> CreatePageVisitAsync(PageVisit pageVisit);
        Task<bool> UpdatePageVisitAsync(PageVisit pageVisit);


    }
}
