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
        Task<Guid> AddAsync(Cart cart);
        Task<Cart> GetByIdAsync(Guid id);
        Task<Cart> GetByUserIdAsync(Guid id);
        Task<IEnumerable<Cart>> GetAllAsync();
        Task<bool> UpdateAsync(Cart cart);
        Task<bool> DeleteAsync(Guid id);
        Task<IEnumerable<CartManager>> GetCartItemsByUserIdAsync(Guid userId);
        Task ClearCartAsync(Guid cartId);
        Task<bool> DeleteCartAsync(Guid cartId);
        Task<List<CartManager>> GetCartManagerByCartIdAsync(Guid cartId);

    }

}
