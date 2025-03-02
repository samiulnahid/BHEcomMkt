﻿using BHEcom.Common.Models;
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

        public async Task<Guid> AddAsync(OrderDetail orderDetail)
        {
            try
            {
                await _context.OrderDetails.AddAsync(orderDetail);
                await _context.SaveChangesAsync();
                return orderDetail.OrderDetailID;
            }
            catch (Exception ex)
            {
               _logger.LogError(ex, "An error occurred while adding a orderDetail.");
                return Guid.Empty;
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
        public async Task DeleteDetailsByOrderIdAsync(Guid id)
        {
            var orderDetails = await _context.OrderDetails
                                              .Where(od => od.OrderID == id)
                                              .ToListAsync();
            if (orderDetails != null)
            {
                _context.OrderDetails.RemoveRange(orderDetails);
                await _context.SaveChangesAsync();
            }
        }
    }
}
