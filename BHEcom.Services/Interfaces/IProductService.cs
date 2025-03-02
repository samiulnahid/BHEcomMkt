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
        Task<IEnumerable<Product>> GetLatestProductsAsync(int num);
        Task<(IEnumerable<Product>? Products, int? TotalCount)> FilterAllProduct(ProductFilter filterEntity);
        Task<Product> GetProductByIdAsync(Guid id);
        Task<(IEnumerable<Product> Products, int TotalCount)> GetProductByCategoryIdAsync(Guid id, int pageNumber, int pageSize);
        Task<(IEnumerable<Product> Products, int TotalCount)> GetProductByBrandIdAsync(Guid id, int pageNumber, int pageSize);
        Task<Guid> AddProductAsync(Product product);
        Task<(bool isUpdated, string? oldImageUrl)> UpdateProductAsync(Product product);
        Task<bool> UpdateProductDescripAsync(Product product);

        Task<(bool isDelete, string? oldImageUrl)> DeleteProductAsync(Guid id);
        Task<List<Product>> GetTopSellingRandomProductsAsync(int featureCount, int randomCount);
       // Task<(List<Product>? Products, int? TotalCount)> SearchProductsAsync(string searchTerm, ProductFilter filter);
        Task<(List<Product>? Products, int? TotalCount)> SearchProductsAsync( ProductFilter filter);
        Task<(List<Product> TopSellingProducts, int TotalCount)> GetAllTopSellingProductsAsync(int featureCount, ProductFilter filter);

        Task<PageVisit> GetPageVisitAsync(Guid productId, string type, string currentMonth);
        Task<bool> CreatePageVisitAsync(PageVisit pageVisit);
        Task<bool> UpdatePageVisitAsync(PageVisit pageVisit);

    }

}
