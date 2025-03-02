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

        public async Task<(Guid id, bool isUnique)> AddAsync(Brand brand)
        {
            try
            {
                // Check if a brand with the same name already exists
                bool brandExists = await _context.Brands
                    .AnyAsync(b => b.BrandName == brand.BrandName);

                if (brandExists)
                {
                    _logger.LogWarning($"A brand with the name '{brand.BrandName}' already exists.", brand.BrandName);
                    return (Guid.Empty,false); // Return Guid.Empty to indicate failure
                }
                await _context.Brands.AddAsync(brand);
                await _context.SaveChangesAsync();
                return (brand.BrandID,true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding a brand.");
                throw;
            }
        }

        public async Task<(bool isUpdated, string? oldImageUrl, bool isUniqueName)> UpdateAsync(Brand brand)
        {
            try
            {
                string? oldImage = string.Empty;
                // Find the existing user by UserId
                var existingBrand = await _context.Brands.FirstOrDefaultAsync(b => b.BrandID == brand.BrandID);

                if (existingBrand == null)
                {
                    return (false, oldImage, true); 
                }

                // Check if the BrandName has changed and is unique
                if (existingBrand.BrandName != brand.BrandName)
                {
                    bool isBrandNameUnique = await _context.Brands
                        .AnyAsync(b => b.BrandName == brand.BrandName && b.BrandID != brand.BrandID);

                    if (isBrandNameUnique)
                    {
                        _logger.LogWarning($"A brand with the name '{brand.BrandName}' already exists.", brand.BrandName);
                        return (false, oldImage, false);
                    }
                }

                // Update the UserName
                existingBrand.BrandName = brand.BrandName;
                existingBrand.Description = brand.Description;
                existingBrand.ModifiedBy = brand.ModifiedBy;
                existingBrand.ModifiedDate = DateTime.Now;
                existingBrand.IsActive = true;

                if (brand.Image != null)
                {
                    oldImage = existingBrand.Image;
                    existingBrand.Image = brand.Image;
                }

                _context.Brands.Update(brand);
                await _context.SaveChangesAsync();
                return (true, oldImage, true);
            }
            catch (Exception ex)
            {
               _logger.LogError(ex, "An error occurred while updating a brand.");
                throw;
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
