using BHEcom.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHEcom.Data.Repositories
{
    public interface IWishlistRepository
    {
        Task<Guid> AddAsync(Wishlist wishlist);
        Task<Wishlist> GetByIdAsync(Guid id);
        Task<IEnumerable<WishlistManager>> GetListWithItemsByIdAsync(Guid id);
        Task<IEnumerable<Wishlist>> GetAllAsync();
        Task<IEnumerable<WishlistManager>> GetByUserIdAsync(Guid userId);
        Task<bool> UpdateAsync(Wishlist wishlist);
        Task<bool> DeleteAsync(Guid id);
        Task<Wishlist> GetWishlistByUserIdAsync(Guid id);
        Task ClearWishlistAsync(Guid cartId);

        Task<WishlistItem> GetWishlistItemByWishlistAndProductIdAsync(Guid wishlistId, Guid productId);
        Task<WishlistItem> GetWishlistItemByIdAsync(Guid id);
        Task<Guid> AddWishlistItemAsync(WishlistItem wishlistItem);
        Task<bool> UpdateWishlistItemAsync(WishlistItem wishlistItem);
        Task<bool> DeleteWishlistByUserId(Guid wishlistId);
        Task<bool> DeleteFullWishlistAsync(Guid wishlistID);
        Task<bool> DeleteWishlistItemAsync(Guid id);
    }
}
