using BHEcom.Common.Models;
using BHEcom.Data.Repositories;
using BHEcom.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BHEcom.Services.Implementations
{
    public class AddressService : IAddressService
    {
        private readonly IAddressRepository _addressRepository;

        public AddressService(IAddressRepository addressRepository)
        {
            _addressRepository = addressRepository;
        }

        public async Task<IEnumerable<Address>> GetAllAddressesAsync()
        {
            return await _addressRepository.GetAllAsync();
        }

        public async Task<Address> GetAddressByIdAsync(Guid id)
        {
            return await _addressRepository.GetByIdAsync(id);
        }
         public async Task<Address?> GetAddressByUserIdAsync(Guid id)
        {
            return await _addressRepository.GetByUserIdAsync(id);
        }

        public async Task<Guid> AddAddressAsync(Address address)
        {
           return await _addressRepository.AddAsync(address);
        }

        public async Task<bool> UpdateAddressAsync(Address address)
        {
           return await _addressRepository.UpdateAsync(address);
        }

        public async Task<bool> DeleteAddressAsync(Guid id)
        {
             return await _addressRepository.DeleteAsync(id);
        }
    }
}
