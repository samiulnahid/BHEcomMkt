using BHEcom.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHEcom.Data.Repositories
{
    public interface IWishlistRepository
    {
        Task AddAsync(Wishlist wishlist);
        Task<Wishlist> GetByIdAsync(Guid id);
        Task<IEnumerable<Wishlist>> GetAllAsync();
        Task UpdateAsync(Wishlist wishlist);
        Task DeleteAsync(Guid id);
    }
}
