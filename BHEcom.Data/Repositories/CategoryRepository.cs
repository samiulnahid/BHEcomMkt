using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using BHEcom.Common.Models;
using Microsoft.Extensions.Logging;

namespace BHEcom.Data.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<CategoryRepository> _logger;

        public CategoryRepository(ApplicationDbContext context, ILogger<CategoryRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        //public async Task<IEnumerable<Category>> GetCategoriesAsync()
        //{
        //    return await _context.Categories.FromSqlRaw("EXEC GetCategories").ToListAsync();
        //}

        public async Task<(Guid id, bool isUnique)> AddAsync(Category category)
        {
            try
            {
                // Check if a category with the same name already exists
                bool categoryExists = await _context.Categories
                    .AnyAsync(c => c.CategoryName == category.CategoryName);

                if (categoryExists)
                {
                    _logger.LogWarning("A category with the name '{CategoryName}' already exists.", category.CategoryName);
                    return (Guid.Empty, false ); // Return Guid.Empty to indicate failure
                }

                await _context.Categories.AddAsync(category);
                await _context.SaveChangesAsync();
                return (category.CategoryID,true);
            }
            catch (Exception ex)
            {
               _logger.LogError(ex, "An error occurred while adding a category.");
                throw;
            }
        }

        public async Task<Category> GetByIdAsync(Guid id)
        {
            return await _context.Categories.FindAsync(id);
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await _context.Categories.ToListAsync();
        }
        public async Task<IEnumerable<Category>> GetSubCategoriesAsync(Guid id)
        {
            return await _context.Categories
         .Where(c => c.ParentCategoryID == id)
         .ToListAsync() ?? new List<Category>();
        }


        public async Task<(bool isUpdated, string? oldImageUrl, bool isUniqueName)> UpdateAsync(Category category)
        {
            try
            {
                string? oldImage = string.Empty;
                // Find the existing user by UserId
                var existingCategory = await _context.Categories.FirstOrDefaultAsync(c => c.CategoryID == category.CategoryID);

                if (existingCategory == null)
                {
                    return (false, oldImage, true); 
                }

                // Check if the CategoryName has changed and is unique
                if (existingCategory.CategoryName != category.CategoryName)
                {
                    bool isCategoryNameUnique = await _context.Categories
                        .AnyAsync(c => c.CategoryName == category.CategoryName && c.CategoryID != category.CategoryID);

                    if (!isCategoryNameUnique)
                    {
                        _logger.LogWarning($"A category with the name '{category.CategoryName}' already exists.", category.CategoryName);
                        return (false, oldImage, false);
                    }
                }


                // Update the UserName
                existingCategory.CategoryName = category.CategoryName;
                existingCategory.ParentCategoryID = category.ParentCategoryID;
                existingCategory.Description = category.Description;
                existingCategory.ModifiedDate = DateTime.Now;
                existingCategory.IsActive = true;
                existingCategory.ModifiedBy = category.ModifiedBy;

                if (category.Image != null)
                {
                    oldImage = existingCategory.Image;
                    existingCategory.Image = category.Image;
                }

                _context.Categories.Update(existingCategory);
                await _context.SaveChangesAsync();
                return (true, oldImage, true);
            }
            catch (Exception ex)
            {
               _logger.LogError(ex, "An error occurred while updating a category.");
                throw;
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            var categories = await _context.Categories.FindAsync(id);
            if (categories != null)
            {
                _context.Categories.Remove(categories);
                await _context.SaveChangesAsync();
            }
        }
    }
}
