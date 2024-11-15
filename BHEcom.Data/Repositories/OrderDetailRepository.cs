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
    public class OrderDetailRepository : IOrderDetailRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<OrderDetailRepository> _logger;
        public OrderDetailRepository(ApplicationDbContext context, ILogger<OrderDetailRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task AddAsync(OrderDetail orderDetail)
        {
            try
            {
                await _context.OrderDetails.AddAsync(orderDetail);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {

               _logger.LogError(ex, "An error occurred while adding a orderDetail.");
            }
        }

        public async Task<OrderDetail> GetByIdAsync(Guid id)
        {
            return await _context.OrderDetails.FindAsync(id);
        }

        public async Task<IEnumerable<OrderDetail>> GetAllAsync()
        {
            return await _context.OrderDetails.ToListAsync();
        }

        public async Task UpdateAsync(OrderDetail orderDetail)
        {
            try
            {
                _context.OrderDetails.Update(orderDetail);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {

               _logger.LogError(ex, "An error occurred while updating a orderDetail.");
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            var orderDetail = await _context.OrderDetails.FindAsync(id);
            if (orderDetail != null)
            {
                _context.OrderDetails.Remove(orderDetail);
                await _context.SaveChangesAsync();
            }
        }
    }
}
