using BHEcom.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHEcom.Services.Interfaces
{
    public interface ICartService
    {
        Task<IEnumerable<Cart>> GetAllCartsAsync();
        Task<Cart> GetCartByIdAsync(Guid id);
        Task<Cart> GetCartByuserIdAsync(Guid id);
        Task<Guid> AddCartAsync(Cart cart);
        Task<bool> UpdateCartAsync(Cart cart);
        Task<bool> DeleteCartAsync(Guid id);
        Task<IEnumerable<CartManager>> GetCartItemsByUserIdAsync(Guid userId);
        Task ClearCartAsync(Guid cartId);
        Task<bool> DeleteFullCartAsync(Guid cartId);
        Task<List<CartManager>> GetCartManagerByCartIdAsync(Guid cartId);
    }

}
