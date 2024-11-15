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
    public class CartItemRepository : ICartItemRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<CartItemRepository> _logger;

        public CartItemRepository(ApplicationDbContext context, ILogger<CartItemRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task AddAsync(CartItem cartItem)
        {
            try
            {
                await _context.CartItems.AddAsync(cartItem);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {

               _logger.LogError(ex, "An error occurred while adding a cartItem.");
            }
        }

        public async Task<CartItem> GetByIdAsync(Guid id)
        {
            return await _context.CartItems.FindAsync(id);
        }

        public async Task<IEnumerable<CartItem>> GetAllAsync()
        {
            return await _context.CartItems.ToListAsync();
        }

        public async Task UpdateAsync(CartItem cartItem)
        {
            try
            {
                _context.CartItems.Update(cartItem);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {

               _logger.LogError(ex, "An error occurred while updating a cartItem.");
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            var cartItem = await _context.CartItems.FindAsync(id);
            if (cartItem != null)
            {
                _context.CartItems.Remove(cartItem);
                await _context.SaveChangesAsync();
            }
        }
    }

}
