using BHEcom.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHEcom.Services.Interfaces
{
    public interface IAddressService
    {
        Task<IEnumerable<Address>> GetAllAddressesAsync();
        Task<IEnumerable<Address>> GetAllAddressesByUserId(Guid id);
        Task<Address> GetAddressByIdAsync(Guid id);
        Task<Address?> GetAddressByUserIdAsync(Guid id, string type);
        Task<Guid> AddAddressAsync(Address address);
        Task<bool> UpdateAddressAsync(Address address);
        Task<bool> DeleteAddressAsync(Guid id);
    }
   
}
