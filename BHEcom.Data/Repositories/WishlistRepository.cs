using BHEcom.Common.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHEcom.Data.Repositories
{

    public class WishlistRepository : IWishlistRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<WishlistRepository> _logger;
        public WishlistRepository(ApplicationDbContext context, ILogger<WishlistRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task AddAsync(Wishlist wishlist)
        {
            try
            {
                await _context.Wishlist.AddAsync(wishlist);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // Handle the exception as needed
               _logger.LogError(ex, "An error occurred while adding a wishlistItem.");
            }
        }

        public async Task<Wishlist> GetByIdAsync(Guid id)
        {
            return await _context.Wishlist.FindAsync(id);
        }

        public async Task<IEnumerable<Wishlist>> GetAllAsync()
        {
            return await _context.Wishlist.ToListAsync();
        }

        public async Task UpdateAsync(Wishlist wishlist)
        {
            try
            {
                _context.Wishlist.Update(wishlist);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // Handle the exception as needed
               _logger.LogError(ex, "An error occurred while updating a wishlistItem.");
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            var wishlist = await _context.Wishlist.FindAsync(id);
            if (wishlist != null)
            {
                _context.Wishlist.Remove(wishlist);
                await _context.SaveChangesAsync();
            }
        }
    }
}
