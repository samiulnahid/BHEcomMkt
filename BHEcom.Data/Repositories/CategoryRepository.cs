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

        public async Task AddAsync(Category category)
        {
            try
            {
                await _context.Categories.AddAsync(category);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {

               _logger.LogError(ex, "An error occurred while adding a category.");
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


        public async Task UpdateAsync(Category category)
        {
            try
            {
                _context.Categories.Update(category);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {

               _logger.LogError(ex, "An error occurred while updating a category.");
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
