using System.Collections.Generic;
using System.Threading.Tasks;
using BHEcom.Common.Models;

namespace BHEcom.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<Category>> GetAllCategoriesAsync();
        Task<IEnumerable<Category>> GetSubCategoriesByParentCategoryIDAsync(Guid id);
        Task<Category> GetCategoryByIdAsync(Guid id);
        Task<(Guid id, bool isUnique)> AddCategoryAsync(Category category);
        Task<(bool isUpdated, string? oldImageUrl, bool isUniqueName)> UpdateCategoryAsync(Category category);
        Task DeleteCategoryAsync(Guid id);
    }
}
