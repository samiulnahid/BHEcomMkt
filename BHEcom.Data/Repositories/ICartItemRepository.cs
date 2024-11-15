using BHEcom.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHEcom.Data.Repositories
{
    public interface ICartItemRepository
    {
        Task AddAsync(CartItem cartItem);
        Task<CartItem> GetByIdAsync(Guid id);
        Task<IEnumerable<CartItem>> GetAllAsync();
        Task UpdateAsync(CartItem cartItem);
        Task DeleteAsync(Guid id);
    }

}
