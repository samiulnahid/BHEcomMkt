using BHEcom.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHEcom.Services.Interfaces
{
    public interface IWishlistService
    {


        //  Task<Wishlist> GetCartByuserIdAsync(Guid id);

        //  Task<bool> DeleteFullCartAsync(Guid cartId);
        //   Task<List<WishlistManager>> GetCartManagerByCartIdAsync(Guid cartId);

         
        //  Task<IEnumerable<WishlistManager>> GetWishlistItemByIdAsync(Guid id);

        Task<IEnumerable<Wishlist>> GetAllWishlistsAsync();
        Task<IEnumerable<WishlistManager>> GetByUserIdAsync(Guid userId);
        Task<Wishlist> GetWishlistByIdAsync(Guid id);
        Task<IEnumerable<WishlistManager>> GetListWithItemsByIdAsync(Guid id);
        Task<Guid> AddWishlistAsync(Wishlist wishlist);
        Task<bool> UpdateWishlistAsync(Wishlist wishlist);

        Task<bool> DeleteWishlistAsync(Guid id);


        Task ClearWishlistAsync(Guid cartId);
        Task<WishlistItem> GetWishlistItemByWishlistAndProductIdAsync(Guid wishlistId, Guid productId);
        Task<WishlistItem> GetWishlistItemByIdAsync(Guid id);
        Task<Wishlist> GetWishlistByUserIdAsync(Guid id);
        Task<Guid> AddWishlistItemAsync(WishlistItem wishlistItem);
        Task<bool> UpdateWishlistItemAsync(WishlistItem wishlistItem);
        Task<bool> DeleteWishlistByUserId(Guid wishlistId);
        Task<bool> DeleteFullWishlistAsync(Guid wishlistID);
        Task<bool> DeleteWishlistItemAsync(Guid id);
    }

}
