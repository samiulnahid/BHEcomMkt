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

        public async Task AddAsync(Cart cart)
        {
            try
            {
                await _context.Cart.AddAsync(cart);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {

               _logger.LogError(ex, "An error occurred while adding a cart.");
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

        public async Task UpdateAsync(Cart cart)
        {
            try
            {
                _context.Cart.Update(cart);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {

               _logger.LogError(ex, "An error occurred while updating a cart.");
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            var cart = await _context.Cart.FindAsync(id);
            if (cart != null)
            {
                _context.Cart.Remove(cart);
                await _context.SaveChangesAsync();
            }
        }
    }

}
