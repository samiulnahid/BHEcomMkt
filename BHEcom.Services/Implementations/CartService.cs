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
   
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;

        public CartService(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }

        public async Task<IEnumerable<Cart>> GetAllCartsAsync()
        {
            return await _cartRepository.GetAllAsync();
        }

        public async Task<Cart> GetCartByIdAsync(Guid id)
        {
            return await _cartRepository.GetByIdAsync(id);
        }

        public async Task<Guid> AddCartAsync(Cart cart)
        {
            return await _cartRepository.AddAsync(cart);
        }

        public async Task<bool> UpdateCartAsync(Cart cart)
        {
            return await _cartRepository.UpdateAsync(cart);
        }

        public async Task<bool> DeleteCartAsync(Guid id)
        {
            return await _cartRepository.DeleteAsync(id);
        }
        public async Task<IEnumerable<CartManager>> GetCartItemsByUserIdAsync(Guid userId)
        {
            return await _cartRepository.GetCartItemsByUserIdAsync(userId);
        }
         public async Task<List<CartManager>> GetCartManagerByCartIdAsync(Guid cartId)
        {
            return await _cartRepository.GetCartManagerByCartIdAsync(cartId);
        }

        public async Task ClearCartAsync(Guid cartId)
        {
            await _cartRepository.ClearCartAsync(cartId);
        }
    }
}
