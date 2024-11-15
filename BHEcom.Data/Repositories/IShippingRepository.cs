using BHEcom.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHEcom.Data.Repositories
{
    public interface IShippingRepository
    {
        Task AddAsync(Shipping shipping);
        Task<Shipping> GetByIdAsync(Guid id);
        Task<IEnumerable<Shipping>> GetAllAsync();
        Task UpdateAsync(Shipping shipping);
        Task DeleteAsync(Guid id);
    }
}
