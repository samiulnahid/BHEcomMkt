using BHEcom.Common.Models;
using BHEcom.Data.Repositories;
using BHEcom.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHEcom.Services.Implementations
{
    public class ShippingService : IShippingService
    {
        private readonly IShippingRepository _shippingRepository;

        public ShippingService(IShippingRepository shippingRepository)
        {
            _shippingRepository = shippingRepository;
        }

        public async Task<IEnumerable<Shipping>> GetAllShippingsAsync()
        {
            return await _shippingRepository.GetAllAsync();
        }

        public async Task<Shipping> GetShippingByIdAsync(Guid id)
        {
            return await _shippingRepository.GetByIdAsync(id);
        }

        public async Task AddShippingAsync(Shipping shipping)
        {
            await _shippingRepository.AddAsync(shipping);
        }

        public async Task UpdateShippingAsync(Shipping shipping)
        {
            await _shippingRepository.UpdateAsync(shipping);
        }

        public async Task DeleteShippingAsync(Guid id)
        {
            await _shippingRepository.DeleteAsync(id);
        } 
        public async Task CreateOrUpdateAsync(Shipping shipping)
        {
            await _shippingRepository.CreateOrUpdateAsync(shipping);
        }
    }

}
