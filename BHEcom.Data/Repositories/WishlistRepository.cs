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

        public async Task<Guid> AddAsync(Wishlist wishlist)
        {
            await _context.Wishlist.AddAsync(wishlist);
            await _context.SaveChangesAsync();
            return wishlist.WishlistID;
        }

        public async Task<Wishlist> GetByIdAsync(Guid id)
        {
            return await _context.Wishlist.FindAsync(id);
        }
        public async Task<IEnumerable<WishlistManager>> GetListWithItemsByIdAsync(Guid id)
        {
            // return await _context.Wishlist.FindAsync(id);
            return await _context.vw_EcommerceWishlist
                                      .Where(wishlist => wishlist.WishlistID == id)
                                      .ToListAsync();
        }
        public async Task<IEnumerable<Wishlist>> GetAllAsync()
        {
            return await _context.Wishlist.ToListAsync();
        }

        public async Task<bool> UpdateAsync(Wishlist wishlist)
        {
            try
            {
                _context.Wishlist.Update(wishlist);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating a wishlist.");
                return false;
            }
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var wishlist = await _context.Wishlist.FindAsync(id);
            if (wishlist != null)
            {
                _context.Wishlist.Remove(wishlist);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        } 
        
        public async Task ClearWishlistAsync(Guid wishlistId)
        {
            var wishlistItems = _context.WishlistItems.Where(wi => wi.WishlistID == wishlistId);
            _context.WishlistItems.RemoveRange(wishlistItems);
            await _context.SaveChangesAsync();
        }
        public async Task<Wishlist> GetWishlistByUserIdAsync(Guid id)
        {
            return await _context.Wishlist.FirstOrDefaultAsync(wishlist => wishlist.UserID == id);
        }


        public async Task<bool> DeleteFullWishlistAsync(Guid wishlistID)
        {
            var wishlistItems = _context.WishlistItems.Where(wi => wi.WishlistID == wishlistID);
            _context.WishlistItems.RemoveRange(wishlistItems);
            await _context.SaveChangesAsync();

            var wishlist = await _context.Wishlist.FindAsync(wishlistID);
            if (wishlist != null)
            {
                _context.Wishlist.Remove(wishlist);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }



        //public async Task<IEnumerable<WishlistManager>> GetWishlistItemsByUserIdAsync(Guid userId)
        //{
        //    return await _context.vw_EcommerceWishlist
        //                         .Where(wishlist => wishlist.UserID == userId)
        //                         .ToListAsync();
        //}



        public async Task<bool> DeleteWishlistByUserId(Guid userId)
        {
            var wishlist = await _context.Wishlist.FirstOrDefaultAsync(wishlist => wishlist.UserID == userId); 
            if (wishlist != null)
            {
                var wishlistItems = _context.WishlistItems.Where(wi => wi.WishlistID == wishlist.WishlistID);
                _context.WishlistItems.RemoveRange(wishlistItems);
                await _context.SaveChangesAsync();

              //  var wishlist = await _context.Wishlist.FindAsync(userId);
            
                _context.Wishlist.Remove(wishlist);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        //public async Task<List<WishlistManager>> GetWishlistManagerByWishlistIdAsync(Guid wishlistId)
        //{
        //    var wishlistItems = await _context.vw_EcommerceWishlist
        //                                       .Where(wm => wm.WishlistID == wishlistId)
        //                                       .ToListAsync();
        //    return wishlistItems;
        //}
   
        public async Task<Guid> AddWishlistItemAsync(WishlistItem wishlistItem)
        {
            try
            {
                await _context.WishlistItems.AddAsync(wishlistItem);
                await _context.SaveChangesAsync();
                return wishlistItem.WishlistItemID;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding a wishlist item.");
                return Guid.Empty;
            }
        }

        public async Task<WishlistItem> GetWishlistItemByIdAsync(Guid id)
        {
            return await _context.WishlistItems.FindAsync(id);
        }

        public async Task<WishlistItem> GetWishlistItemByWishlistAndProductIdAsync(Guid wishlistId, Guid productId)
        {
            return await _context.WishlistItems
                                  .FirstOrDefaultAsync(wi => wi.WishlistID == wishlistId && wi.ProductID == productId);
        }

        public async Task<IEnumerable<WishlistItem>> GetAllWishlistItemAsync()
        {
            return await _context.WishlistItems.ToListAsync();
        }

        public async Task<bool> UpdateWishlistItemAsync(WishlistItem wishlistItem)
        {
            try
            {
                _context.WishlistItems.Update(wishlistItem);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating a wishlist item.");
                return false;
            }
        }

        public async Task<bool> DeleteWishlistItemAsync(Guid id)
        {
            var wishlistItem = await _context.WishlistItems.FindAsync(id);
            if (wishlistItem != null)
            {
                _context.WishlistItems.Remove(wishlistItem);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
      
        public async Task<IEnumerable<WishlistManager>> GetByUserIdAsync(Guid userId)
        {

            return await _context.vw_EcommerceWishlist
                                       .Where(wishlist => wishlist.UserID == userId)
                                       .ToListAsync();
        }

    }

}
