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
    public class CartItemService : ICartItemService
    {
        private readonly ICartItemRepository _cartItemRepository;

        public CartItemService(ICartItemRepository cartItemRepository)
        {
            _cartItemRepository = cartItemRepository;
        }

        public async Task<IEnumerable<CartItem>> GetAllCartItemsAsync()
        {
            return await _cartItemRepository.GetAllAsync();
        }

        public async Task<CartItem> GetCartItemByIdAsync(Guid id)
        {
            return await _cartItemRepository.GetByIdAsync(id);
        }
        //public async Task<CartItem> GetCartItemByProductIdAsync(Guid id)
        //{
        //    return await _cartItemRepository.GetByProductIdAsync(id);
        //}
          public async Task<CartItem> GetCartItemByCartandProductIdAsync(Guid cartId, Guid productId)
        {
            return await _cartItemRepository.GetByCartandProductIdAsync(cartId, productId);
        }

        public async Task<Guid> AddCartItemAsync(CartItem cartItem)
        {
           return await _cartItemRepository.AddAsync(cartItem);
        }

        public async Task<bool> UpdateCartItemAsync(CartItem cartItem)
        {
           return await _cartItemRepository.UpdateAsync(cartItem);
        }

        public async Task<bool> DeleteCartItemAsync(Guid id)
        {
           return await _cartItemRepository.DeleteAsync(id);
        }
         public async Task<(bool Success, string Message)> UpdateCartItemQuantityAsync(Guid cartItemId, string operation)
        {
           return await _cartItemRepository.UpdateCartItemQuantityAsync(cartItemId, operation);
        }

    }
}
