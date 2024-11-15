using System.Collections.Generic;
using System.Threading.Tasks;
using BHEcom.Common.Models;

namespace BHEcom.Data.Repositories
{
    public interface ICategoryRepository
    {
        // Task<IEnumerable<Category>> GetCategoriesAsync();
        // Other methods if needed

        Task AddAsync(Category category);
        Task<Category> GetByIdAsync(Guid id);
        Task<IEnumerable<Category>> GetAllAsync();
        Task<IEnumerable<Category>> GetSubCategoriesAsync(Guid id);
        Task UpdateAsync(Category category);
        Task DeleteAsync(Guid id);
    }
}
