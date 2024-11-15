using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using BHEcom.Common.Models;
using Microsoft.Extensions.Logging;

namespace BHEcom.Data.Repositories
{
    public class BrandRepository : IBrandRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<BrandRepository> _logger;

        public BrandRepository(ApplicationDbContext context, ILogger<BrandRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<Brand>> GetAllAsync()
        {
            return await _context.Brands.ToListAsync();
        }

        public async Task<Brand> GetByIdAsync(Guid id)
        {
            return await _context.Brands.FindAsync(id);
        }

        public async Task AddAsync(Brand brand)
        {
            try
            {
                await _context.Brands.AddAsync(brand);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "An error occurred while adding a brand.");
            }
        }

        public async Task UpdateAsync(Brand brand)
        {
            try
            {
                _context.Brands.Update(brand);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {

               _logger.LogError(ex, "An error occurred while updating a brand.");
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            var brand = await _context.Brands.FindAsync(id);
            if (brand != null)
            {
                _context.Brands.Remove(brand);
                await _context.SaveChangesAsync();
            }
        }
    }
}
