using System.Collections.Generic;
using System.Threading.Tasks;
using BHEcom.Common.Models;
using BHEcom.Data.Repositories;
using BHEcom.Services.Interfaces;

namespace BHEcom.Services.Implementations
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            return await _categoryRepository.GetAllAsync();
        }
         public async Task<IEnumerable<Category>> GetSubCategoriesByParentCategoryIDAsync(Guid id)
        {
            return await _categoryRepository.GetSubCategoriesAsync(id);
        }

        public async Task<Category> GetCategoryByIdAsync(Guid id)
        {
            return await _categoryRepository.GetByIdAsync(id);
        }

        public async Task AddCategoryAsync(Category category)
        {
            await _categoryRepository.AddAsync(category);
        }

        public async Task UpdateCategoryAsync(Category category)
        {
            await _categoryRepository.UpdateAsync(category);
        }

        public async Task DeleteCategoryAsync(Guid id)
        {
            await _categoryRepository.DeleteAsync(id);
        }
    }
}
