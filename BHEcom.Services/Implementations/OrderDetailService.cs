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
    public class OrderDetailService : IOrderDetailService
    {
        private readonly IOrderDetailRepository _orderDetailRepository;

        public OrderDetailService(IOrderDetailRepository orderDetailRepository)
        {
            _orderDetailRepository = orderDetailRepository;
        }

        public async Task<IEnumerable<OrderDetail>> GetAllOrderDetailsAsync()
        {
            return await _orderDetailRepository.GetAllAsync();
        }

        public async Task<OrderDetail> GetOrderDetailByIdAsync(Guid id)
        {
            return await _orderDetailRepository.GetByIdAsync(id);
        }

        public async Task<Guid> AddOrderDetailAsync(OrderDetail orderDetail)
        {
           return await _orderDetailRepository.AddAsync(orderDetail);
        }

        public async Task UpdateOrderDetailAsync(OrderDetail orderDetail)
        {
            await _orderDetailRepository.UpdateAsync(orderDetail);
        }

        public async Task DeleteOrderDetailAsync(Guid id)
        {
            await _orderDetailRepository.DeleteAsync(id);
        } 
        public async Task DeleteDetailsByOrderIdAsync(Guid id)
        {
            await _orderDetailRepository.DeleteDetailsByOrderIdAsync(id);
        }
    }
}
