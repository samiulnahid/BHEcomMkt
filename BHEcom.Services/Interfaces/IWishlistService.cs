using BHEcom.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHEcom.Services.Interfaces
{
    public interface IWishlistService
    {
        Task<IEnumerable<Wishlist>> GetAllWishlistsAsync();
        Task<Wishlist> GetWishlistByIdAsync(Guid id);
        Task AddWishlistAsync(Wishlist wishlist);
        Task UpdateWishlistAsync(Wishlist wishlist);
        Task DeleteWishlistAsync(Guid id);
    }

}
