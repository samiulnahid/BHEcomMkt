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
        Task AddCartItemAsync(CartItem cartItem);
        Task UpdateCartItemAsync(CartItem cartItem);
        Task DeleteCartItemAsync(Guid id);
    }
}
