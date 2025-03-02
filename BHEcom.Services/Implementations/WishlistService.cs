using BHEcom.Common.Models;
using BHEcom.Data.Repositories;
using BHEcom.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHEcom.Services.Implementations
{
    public class WishlistService : IWishlistService
    {
        private readonly IWishlistRepository _wishlistRepository;

        public WishlistService(IWishlistRepository wishlistRepository)
        {
            _wishlistRepository = wishlistRepository;
        }

        public async Task<IEnumerable<Wishlist>> GetAllWishlistsAsync()
        {
            return await _wishlistRepository.GetAllAsync();
        }
         public async Task<IEnumerable<WishlistManager>> GetByUserIdAsync(Guid userId)
        {
            return await _wishlistRepository.GetByUserIdAsync(userId);
        }

        public async Task<Wishlist> GetWishlistByIdAsync(Guid id)
        {
            return await _wishlistRepository.GetByIdAsync(id);
        }
        public async Task<IEnumerable<WishlistManager>> GetListWithItemsByIdAsync(Guid id)
        {
            return await _wishlistRepository.GetListWithItemsByIdAsync(id);
        }

        public async Task<Guid> AddWishlistAsync(Wishlist wishlist)
        {
            return await _wishlistRepository.AddAsync(wishlist);
        }

        public async Task<bool> UpdateWishlistAsync(Wishlist wishlist)
        {
           return await _wishlistRepository.UpdateAsync(wishlist);
        }

        public async Task<bool> DeleteWishlistAsync(Guid id)
        {
          return   await _wishlistRepository.DeleteAsync(id);
        }
         public async Task ClearWishlistAsync(Guid id)
        {
            await _wishlistRepository.ClearWishlistAsync(id);
        }

        public async Task<WishlistItem> GetWishlistItemByWishlistAndProductIdAsync(Guid wishlistId, Guid productId)
        {
            return await _wishlistRepository.GetWishlistItemByWishlistAndProductIdAsync(wishlistId, productId);
        }
        public async Task<WishlistItem> GetWishlistItemByIdAsync(Guid id)
        {
            return await _wishlistRepository.GetWishlistItemByIdAsync(id);
        }

        public async Task<Wishlist> GetWishlistByUserIdAsync(Guid id)
        {
            return await _wishlistRepository.GetWishlistByUserIdAsync(id);
        }
        
        public async Task<Guid> AddWishlistItemAsync(WishlistItem wishlistItem)
        {
            return await _wishlistRepository.AddWishlistItemAsync(wishlistItem);
        }

        public async Task<bool> UpdateWishlistItemAsync(WishlistItem wishlistItem)
        {
            return await _wishlistRepository.UpdateWishlistItemAsync(wishlistItem);
        } 
        public async Task<bool> DeleteWishlistByUserId(Guid wishlistId)
        {
            return await _wishlistRepository.DeleteWishlistByUserId(wishlistId);
        }
        public async Task<bool> DeleteWishlistItemAsync(Guid id)
        {
            return await _wishlistRepository.DeleteWishlistItemAsync(id);
        }
         public async Task<bool> DeleteFullWishlistAsync(Guid wishlistID)
        {
            return await _wishlistRepository.DeleteFullWishlistAsync(wishlistID);
        }


    }

}
