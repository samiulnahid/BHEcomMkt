using BHEcom.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHEcom.Services.Interfaces
{
    public interface IOrderService
    {
        Task<IEnumerable<Order>> GetAllOrdersAsync();
        Task<Order> GetOrderByIdAsync(Guid id);
        Task<(Guid orderId, string orderNumber)> AddOrderAsync(Order order);
        Task<bool> UpdateOrderAsync(Guid orderId, string status);
        Task DeleteOrderAsync(Guid id);

        Task<Coupon> GetCouponById(Guid? id);
        Task<Guid> AddCouponAsync(Coupon coupon);
        Task<bool> UpdateCouponAsync(Coupon coupon);
        Task<(Coupon? coupon, string result)> ValidateCouponAsync(string code);

        Task<Guid> AddDeliveryDetailsAsync(DeliveryDetails deliveryDetails);
        Task<IEnumerable<OrderManager>> GetAllByUserIdAsync(Guid userId);
    }
}
