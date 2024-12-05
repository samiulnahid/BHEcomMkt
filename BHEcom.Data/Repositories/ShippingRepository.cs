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

    public class ShippingRepository : IShippingRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ShippingRepository> _logger;
        public ShippingRepository(ApplicationDbContext context, ILogger<ShippingRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task AddAsync(Shipping shipping)
        {
            try
            {
                await _context.Shipping.AddAsync(shipping);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // Handle the exception as needed
               _logger.LogError(ex, "An error occurred while adding a shipping.");
            }
        }

        public async Task<Shipping> GetByIdAsync(Guid id)
        {
            return await _context.Shipping.FindAsync(id);
        }

        public async Task<IEnumerable<Shipping>> GetAllAsync()
        {
            return await _context.Shipping.ToListAsync();
        }

        public async Task UpdateAsync(Shipping shipping)
        {
            try
            {
                _context.Shipping.Update(shipping);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // Handle the exception as needed
               _logger.LogError(ex, "An error occurred while updating a shipping.");
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            var shipping = await _context.Shipping.FindAsync(id);
            if (shipping != null)
            {
                _context.Shipping.Remove(shipping);
                await _context.SaveChangesAsync();
            }
        }

        public async Task CreateOrUpdateAsync(Shipping shipping)
        {
            try
            {
                var existingShipping = await _context.Shipping.FindAsync(shipping.ShippingID);
                if (existingShipping != null)
                {
                    // Update existing record
                    existingShipping.OrderID = shipping.OrderID;
                    existingShipping.ShippingAddressID = shipping.ShippingAddressID;
                    existingShipping.ShippingMethod = shipping.ShippingMethod;
                    existingShipping.ShippingCost = shipping.ShippingCost;
                    existingShipping.ShippingStatus = shipping.ShippingStatus;
                    existingShipping.ShippedDate = shipping.ShippedDate;
                    existingShipping.EstimatedDeliveryDate = shipping.EstimatedDeliveryDate;

                    _context.Shipping.Update(existingShipping);
                }
                else
                {
                    // Create new record
                    await _context.Shipping.AddAsync(shipping);
                }

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // Handle the exception as needed
                _logger.LogError(ex, "An error occurred while creating or updating a shipping.");
                throw; // Re-throw the exception to ensure it's handled elsewhere
            }
        }

    }
}
