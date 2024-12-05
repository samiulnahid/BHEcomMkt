using BHEcom.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHEcom.Services.Interfaces
{
    public interface IShippingService
    {
        Task<IEnumerable<Shipping>> GetAllShippingsAsync();
        Task<Shipping> GetShippingByIdAsync(Guid id);
        Task AddShippingAsync(Shipping shipping);
        Task UpdateShippingAsync(Shipping shipping);
        Task DeleteShippingAsync(Guid id);
        Task CreateOrUpdateAsync(Shipping shipping);
    }

}
