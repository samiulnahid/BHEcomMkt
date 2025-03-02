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

    public class SubscribeService : ISubscribeService
    {
        private readonly ISubscribeRepository _subscribeRepository;

        public SubscribeService(ISubscribeRepository subscribeRepository)
        {
            _subscribeRepository = subscribeRepository;
        }

        public async Task<IEnumerable<Subscribe>> GetAllSubscribesAsync()
        {
            return await _subscribeRepository.GetAllAsync();
        }

        public async Task<Subscribe> GetSubscribeByIdAsync(Guid id)
        {
            return await _subscribeRepository.GetByIdAsync(id);
        }

        public async Task<Guid> AddSubscribeAsync(Subscribe subscribe)
        {
            return await _subscribeRepository.AddAsync(subscribe);
        }

        public async Task<bool> UpdateSubscribeAsync(Subscribe subscribe)
        {
            return await _subscribeRepository.UpdateAsync(subscribe);
        }

        public async Task<bool> DeleteSubscribeAsync(Guid id)
        {
            return await _subscribeRepository.DeleteAsync(id);
        }
    }

}
