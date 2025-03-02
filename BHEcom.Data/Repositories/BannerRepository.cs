using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using BHEcom.Common.Models;
using Microsoft.Extensions.Logging;

namespace BHEcom.Data.Repositories
{

    public class BannerRepository : IBannerRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<BannerRepository> _logger;

        public BannerRepository(ApplicationDbContext context, ILogger<BannerRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Guid> AddAsync(Banner banner)
        {
            try
            {
                
                await _context.Banners.AddAsync(banner);
                await _context.SaveChangesAsync();
                return banner.BannerID;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding a banner.");
                throw;
            }
        }

        public async Task<Banner> GetByIdAsync(Guid id)
        {
            return await _context.Banners.FindAsync(id);
        }

        public async Task<IEnumerable<Banner>> GetAllAsync()
        {
            return await _context.Banners.ToListAsync();
        }

        public async Task<(bool isUpdated, string? oldImageUrl)> UpdateAsync(Banner banner)
        {
            try
            {
                string? oldImage = null;

                var existingBanner = await _context.Banners.FirstOrDefaultAsync(b => b.BannerID == banner.BannerID);

                if (existingBanner == null)
                {
                    return (false, null);
                }

                existingBanner.Title = banner.Title;
                existingBanner.SubTitle = banner.SubTitle;
                existingBanner.ButtonText = banner.ButtonText;
                existingBanner.ButtonLink = banner.ButtonLink;
                existingBanner.IsActive = banner.IsActive;
                existingBanner.ModifiedDate = DateTime.Now;

                if (!string.IsNullOrEmpty(banner.Image))
                {
                    oldImage = existingBanner.Image;
                    existingBanner.Image = banner.Image;
                }

                _context.Banners.Update(existingBanner);
                await _context.SaveChangesAsync();
                return (true, oldImage);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating a banner.");
                return (false, null);
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            var banner = await _context.Banners.FindAsync(id);
            if (banner != null)
            {
                _context.Banners.Remove(banner);
                await _context.SaveChangesAsync();
            }
        }
    }

}
