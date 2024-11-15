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
        Task AddCartAsync(Cart cart);
        Task UpdateCartAsync(Cart cart);
        Task DeleteCartAsync(Guid id);
    }

}
