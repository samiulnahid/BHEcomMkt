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
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;

        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<IEnumerable<Order>> GetAllOrdersAsync()
        {
            return await _orderRepository.GetAllAsync();
        }

        public async Task<Order> GetOrderByIdAsync(Guid id)
        {
            return await _orderRepository.GetByIdAsync(id);
        }

        public async Task<(Guid orderId, string orderNumber)> AddOrderAsync(Order order)
        {
            return await _orderRepository.AddAsync(order);
        }

        public async Task<bool> UpdateOrderAsync(Guid orderId, string status)
        {
            return await _orderRepository.UpdateAsync(orderId, status);
        }

        public async Task DeleteOrderAsync(Guid id)
        {
            await _orderRepository.DeleteAsync(id);
        }


        public async Task<Coupon> GetCouponById(Guid? id)
        {
            return await _orderRepository.GetCouponById(id);
        }

        public async Task<Guid> AddCouponAsync(Coupon coupon)
        {
            return await _orderRepository.AddCouponAsync(coupon);
        }
      

        public async Task<bool> UpdateCouponAsync(Coupon coupon)
        {
            return await _orderRepository.UpdateCouponAsync(coupon);
        }
         public async Task<(Coupon? coupon, string result)> ValidateCouponAsync(string code)
        {
            return await _orderRepository.ValidateCouponAsync(code);
        }
        public async Task<Guid> AddDeliveryDetailsAsync(DeliveryDetails deliveryDetails)
        {
            return await _orderRepository.AddDeliveryDetailsAsync(deliveryDetails);
        }

        public async Task<IEnumerable<OrderManager>> GetAllByUserIdAsync(Guid userId)
        {
            return await _orderRepository.GetAllByUserIdAsync(userId);
        }
    }
}
