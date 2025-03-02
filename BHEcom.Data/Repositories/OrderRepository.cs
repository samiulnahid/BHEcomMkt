using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BHEcom.Common.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace BHEcom.Data.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<OrderRepository> _logger;
        private readonly IConfiguration _configuration;
        public OrderRepository(ApplicationDbContext context, ILogger<OrderRepository> logger, IConfiguration configuration)
        {
            _context = context;
            _logger = logger;
            _configuration = configuration;
        }

        public async Task<(Guid orderId, string orderNumber)> AddAsync(Order order)
        {
            // Generate unique OrderNumber
            order.OrderNumber = await CreateOrderNumberAsync();

            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
            return (order.OrderID, order.OrderNumber);
            
        }

        public async Task<Order> GetByIdAsync(Guid id)
        {
            return await _context.Orders.FindAsync(id);
        }

        public async Task<IEnumerable<Order>> GetAllAsync()
        {
            return await _context.Orders.ToListAsync();
        }

        public async Task<bool> UpdateAsync(Guid orderId, string status)
        {
            //_context.Orders.Update(order);
            //await _context.SaveChangesAsync();

            var order = await _context.Orders.FindAsync(orderId);
            if (order == null)
            {
                _logger.LogWarning("Order with ID {OrderId} not found.", orderId);
                return false;
            }

            order.Status = status; // Update only the Status field
            _context.Entry(order).Property(o => o.Status).IsModified = true;

            await _context.SaveChangesAsync();
            return true;    

        }

        public async Task DeleteAsync(Guid id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order != null)
            {
                _context.Orders.Remove(order);
                await _context.SaveChangesAsync();
            }
        }

        // 🟢 Get Coupon by ID
        public async Task<Coupon> GetCouponById(Guid? id)
        {
            return await _context.Coupons.FindAsync(id);
        }

        // 🟢 Create a new Coupon (Returns Guid)
        public async Task<Guid> AddCouponAsync(Coupon coupon)
        {
            coupon.CouponId = Guid.NewGuid();
            coupon.CreatedAt = DateTime.UtcNow;

            await _context.Coupons.AddAsync(coupon);
            await _context.SaveChangesAsync();
            return coupon.CouponId;
        }

        // 🟢 Update an existing Coupon (Returns bool)
        public async Task<bool> UpdateCouponAsync(Coupon coupon)
        {
            var existingCoupon = await _context.Coupons.FindAsync(coupon.CouponId);
            if (existingCoupon == null)
            {
                return false;
            }

            // Update only if the new value is not null or default
            if (!string.IsNullOrEmpty(coupon.Code))
                existingCoupon.Code = coupon.Code;

            if (coupon.DiscountPercentage != default(decimal))
                existingCoupon.DiscountPercentage = coupon.DiscountPercentage;

            if (coupon.DiscountAmount != default(decimal))
                existingCoupon.DiscountAmount = coupon.DiscountAmount;

            if (coupon.MinimumOrderAmount != default(decimal))
                existingCoupon.MinimumOrderAmount = coupon.MinimumOrderAmount;

            if (coupon.IsActive != default(bool))
                existingCoupon.IsActive = coupon.IsActive;


            existingCoupon.ExpirationDate = coupon.ExpirationDate;
            existingCoupon.StoreId = coupon.StoreId;
            existingCoupon.UpdatedAt = DateTime.UtcNow;

            _context.Coupons.Update(existingCoupon);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<(Coupon? coupon, string result)> ValidateCouponAsync(string code)
        {
            
            // Case-sensitive search for the coupon code
            var coupon = await _context.Coupons
                .FirstOrDefaultAsync(c => EF.Functions.Collate(c.Code, "SQL_Latin1_General_CP1_CS_AS") == code);

            // 1️. Check if the coupon code exists
            if (coupon == null)
            {
                return (coupon,"Incorrect Coupon code");
            }

            // 2️. Check if the coupon is active
            if (coupon.IsActive == null || coupon.IsActive == false )
            {
                return (null, "This code is Inactive");
            }

            // 3️. Check if the coupon is expired
            if (coupon.ExpirationDate < DateTime.UtcNow)
            {
                return (null, "The date is expired.");
            }

            // Return the valid coupon data
            return (coupon,"Coupon is valid");
        }

        public async Task<Guid> AddDeliveryDetailsAsync(DeliveryDetails deliveryDetails)
        {
            deliveryDetails.CreatedDate = DateTime.UtcNow;
            await _context.DeliveryDetails.AddAsync(deliveryDetails);
            await _context.SaveChangesAsync();
            return deliveryDetails.DeliveryID;
        }

        public async Task AddDeliveryLogsAsync(DeliveryLog deliveryLog)
        {
            await _context.DeliveryLogs.AddAsync(deliveryLog);
            await _context.SaveChangesAsync();
        }


        private async Task<string> CreateOrderNumberAsync()
        {
            // Fetch application-specific string from appsettings.json
            string appString = _configuration["AppSettings:OrderPrefix"] ?? "AP"; // Ensure appsettings.json contains this key

            
            // Generate date-related components
            string year = DateTime.UtcNow.ToString("yy");
            string month = DateTime.UtcNow.ToString("MM");
            string day = DateTime.UtcNow.ToString("dd");

            // Base OrderNumber without numbering
            string baseOrderNumber = $"{appString}{year}{month}{day}";

            // Check for existing orders with similar OrderNumber and increment number if necessary
            int sequenceNumber = 1;
            string finalOrderNumber;
            do
            {
                finalOrderNumber = $"{baseOrderNumber}{sequenceNumber:D4}";
                sequenceNumber++;
            } while (await _context.Orders.AnyAsync(o => o.OrderNumber == finalOrderNumber));

            return finalOrderNumber;
        }

        public async Task<IEnumerable<OrderManager>> GetAllByUserIdAsync(Guid userId)
        {

            return await _context.vw_OrderListByUser
                                            .Where(orderList => orderList.UserID == userId)
                                            .ToListAsync();
        }
    }

}
