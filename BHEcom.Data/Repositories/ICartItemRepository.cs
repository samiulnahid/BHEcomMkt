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
        Task<Guid> AddAsync(CartItem cartItem);
        Task<CartItem> GetByIdAsync(Guid id);
       // Task<CartItem> GetByProductIdAsync(Guid id);
        Task<CartItem> GetByCartandProductIdAsync(Guid cartId, Guid productId);
        Task<IEnumerable<CartItem>> GetAllAsync();
        Task<bool> UpdateAsync(CartItem cartItem);
        Task<bool> DeleteAsync(Guid id);

        Task<(bool Success, string Message)> UpdateCartItemQuantityAsync(Guid cartItemId, string operation);
    }

}
