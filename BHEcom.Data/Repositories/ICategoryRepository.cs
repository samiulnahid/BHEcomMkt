using System.Collections.Generic;
using System.Threading.Tasks;
using BHEcom.Common.Models;

namespace BHEcom.Data.Repositories
{
    public interface ICategoryRepository
    {
        
        Task<(Guid id, bool isUnique)> AddAsync(Category category);
        Task<Category> GetByIdAsync(Guid id);
        Task<IEnumerable<Category>> GetAllAsync();
        Task<IEnumerable<Category>> GetSubCategoriesAsync(Guid id);
        Task<(bool isUpdated, string? oldImageUrl, bool isUniqueName)> UpdateAsync(Category category);
        Task DeleteAsync(Guid id);
    }
}
