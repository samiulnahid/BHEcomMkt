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
    public class CartRepository : ICartRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<CartRepository> _logger;

        public CartRepository(ApplicationDbContext context, ILogger<CartRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Guid> AddAsync(Cart cart)
        {
            try
            {
                await _context.Cart.AddAsync(cart);
                await _context.SaveChangesAsync();
                return cart.CartID;
            }
            catch (Exception ex)
            {
               _logger.LogError(ex, "An error occurred while adding a cart.");
                return Guid.Empty;
            }
        }

        public async Task<Cart> GetByIdAsync(Guid id)
        {
            return await _context.Cart.FindAsync(id);
        }

        public async Task<IEnumerable<Cart>> GetAllAsync()
        {
            return await  _context.Cart.ToListAsync();
        }

        public async Task<bool> UpdateAsync(Cart cart)
        {
            try
            {
                _context.Cart.Update(cart);
                await _context.SaveChangesAsync();
                return true; // Return true if update succeeds
            }
            catch (Exception ex)
            {

               _logger.LogError(ex, "An error occurred while updating a cart.");
                return false;
            }
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var cart = await _context.Cart.FindAsync(id);
            if (cart != null)
            {
                _context.Cart.Remove(cart);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<IEnumerable<CartManager>> GetCartItemsByUserIdAsync(Guid userId)
        {

            return await _context.vw_EcommerceCart
                                       .Where(cart => cart.UserID == userId)
                                       .ToListAsync();
        }

        public async Task ClearCartAsync(Guid cartId)
        {
            var cartItems = _context.CartItems.Where(ci => ci.CartID == cartId);
            _context.CartItems.RemoveRange(cartItems);
            await _context.SaveChangesAsync();
        }

        public async Task<List<CartManager>> GetCartManagerByCartIdAsync(Guid cartId)
        {
            
            //return await _context.vw_EcommerceCart
            //                           .Where(cart => cart.CartItemID == cartId)
            //                           .ToListAsync();

            var cartItems = await _context.vw_EcommerceCart
                                .Where(cm => cm.CartID == cartId)
                                .ToListAsync();
            return cartItems;
        }


    }

}
