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

        public async Task AddCartItemAsync(CartItem cartItem)
        {
            await _cartItemRepository.AddAsync(cartItem);
        }

        public async Task UpdateCartItemAsync(CartItem cartItem)
        {
            await _cartItemRepository.UpdateAsync(cartItem);
        }

        public async Task DeleteCartItemAsync(Guid id)
        {
            await _cartItemRepository.DeleteAsync(id);
        }
    }
}
