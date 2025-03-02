using BHEcom.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHEcom.Services.Interfaces
{
    public interface ICartItemService
    {
        Task<IEnumerable<CartItem>> GetAllCartItemsAsync();
        Task<CartItem> GetCartItemByIdAsync(Guid id);
        // Task<CartItem> GetCartItemByProductIdAsync(Guid id);
        Task<CartItem> GetCartItemByCartandProductIdAsync(Guid cartId, Guid productId);
        Task<Guid> AddCartItemAsync(CartItem cartItem);
        Task<bool> UpdateCartItemAsync(CartItem cartItem);
        Task<bool> DeleteCartItemAsync(Guid id);
        Task<(bool Success, string Message)> UpdateCartItemQuantityAsync(Guid cartItemId, string operation);
    }
}
