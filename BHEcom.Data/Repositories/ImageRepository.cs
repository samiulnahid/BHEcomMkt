using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using BHEcom.Common.Models;
using Microsoft.Extensions.Logging;
using static System.Net.Mime.MediaTypeNames;

namespace BHEcom.Data.Repositories
{
    public class ImageRepository : IImageRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ImageRepository> _logger;

        public ImageRepository(ApplicationDbContext context, ILogger<ImageRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<bool> AddImagelistAsync(List<Images> images)
        {
            await _context.Images.AddRangeAsync(images);
            int result =  await _context.SaveChangesAsync();
            return result > 0; // Return true if changes were successfully saved

        }
        public async Task<bool> AddImageAsync(Images image)
        {
            await _context.Images.AddAsync(image);
            int result = await _context.SaveChangesAsync();
            return result > 0; // Return true if changes were successfully saved
        }

        public async Task<List<Images>> GetImagesByXIdAsync(Guid xId, string xType)
        {
            return await _context.Images
                .Where(i => i.XID == xId && i.XType == xType)
                .ToListAsync();
        }
        public async Task<bool> DeleteImagesByXIdAsync(Guid xId, string xType)
        {
            var imagesToDelete = await _context.Images
                .Where(i => i.XID == xId && i.XType == xType)
                .ToListAsync();

            if (imagesToDelete.Any())
            {
                _context.Images.RemoveRange(imagesToDelete);
                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }

     public async Task<(bool isDeleted, string? oldImageUrl)> DeleteImagesByImageIdAsync(Guid id)
        {
            var images = await _context.Images.FindAsync(id);
            string? ImageUrl = string.Empty;
            if (images != null)
            {
                ImageUrl = images.ImagePath;
                _context.Images.Remove(images);
                await _context.SaveChangesAsync();
                return (true, ImageUrl);
            }

            return (false,null);
        }

     
    }
}
