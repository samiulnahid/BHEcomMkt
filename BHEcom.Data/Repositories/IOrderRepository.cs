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
        Task<Guid> AddAsync(Order order);
        Task<Order> GetByIdAsync(Guid id);
        Task<IEnumerable<Order>> GetAllAsync();
        Task<bool> UpdateAsync(Guid orderId, string status);
        Task DeleteAsync(Guid id);
    }

}
