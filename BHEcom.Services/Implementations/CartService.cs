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

        public async Task AddCartAsync(Cart cart)
        {
            await _cartRepository.AddAsync(cart);
        }

        public async Task UpdateCartAsync(Cart cart)
        {
            await _cartRepository.UpdateAsync(cart);
        }

        public async Task DeleteCartAsync(Guid id)
        {
            await _cartRepository.DeleteAsync(id);
        }
    }
}
