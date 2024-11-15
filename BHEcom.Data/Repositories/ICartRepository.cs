using BHEcom.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHEcom.Data.Repositories
{
    public interface ICartRepository
    {
        Task AddAsync(Cart cart);
        Task<Cart> GetByIdAsync(Guid id);
        Task<IEnumerable<Cart>> GetAllAsync();
        Task UpdateAsync(Cart cart);
        Task DeleteAsync(Guid id);
    }

}
