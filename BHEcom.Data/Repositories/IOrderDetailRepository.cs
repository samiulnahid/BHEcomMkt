using BHEcom.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHEcom.Data.Repositories
{
    public interface IOrderDetailRepository
    {
        Task<Guid> AddAsync(OrderDetail orderDetail);
        Task<OrderDetail> GetByIdAsync(Guid id);
        Task<IEnumerable<OrderDetail>> GetAllAsync();
        Task UpdateAsync(OrderDetail orderDetail);
        Task DeleteAsync(Guid id);
    }
}
