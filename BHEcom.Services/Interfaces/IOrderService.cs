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
        Task<Guid> AddOrderAsync(Order order);
        Task<bool> UpdateOrderAsync(Guid orderId, string status);
        Task DeleteOrderAsync(Guid id);
    }
}
