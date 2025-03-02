using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BHEcom.Common.Models;

namespace BHEcom.Data.Repositories
{
    public interface IOrderRepository
    {
        Task<(Guid orderId, string orderNumber)> AddAsync(Order order);
        Task<Order> GetByIdAsync(Guid id);
        Task<IEnumerable<Order>> GetAllAsync();
        Task<bool> UpdateAsync(Guid orderId, string status);
        Task DeleteAsync(Guid id);

        Task<Coupon> GetCouponById(Guid? id);
        Task<Guid> AddCouponAsync(Coupon coupon);
        Task<bool> UpdateCouponAsync(Coupon coupon);
        Task<(Coupon? coupon, string result)> ValidateCouponAsync(string code);


        Task<Guid> AddDeliveryDetailsAsync(DeliveryDetails deliveryDetails);
        Task<IEnumerable<OrderManager>> GetAllByUserIdAsync(Guid userId);
    }

}
