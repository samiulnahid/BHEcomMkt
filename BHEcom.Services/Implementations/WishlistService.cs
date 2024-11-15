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

        public async Task<Wishlist> GetWishlistByIdAsync(Guid id)
        {
            return await _wishlistRepository.GetByIdAsync(id);
        }

        public async Task AddWishlistAsync(Wishlist wishlist)
        {
            await _wishlistRepository.AddAsync(wishlist);
        }

        public async Task UpdateWishlistAsync(Wishlist wishlist)
        {
            await _wishlistRepository.UpdateAsync(wishlist);
        }

        public async Task DeleteWishlistAsync(Guid id)
        {
            await _wishlistRepository.DeleteAsync(id);
        }
    }

}
