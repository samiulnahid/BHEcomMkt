using BHEcom.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHEcom.Services.Interfaces
{

    public interface ISubscribeService
    {
        Task<IEnumerable<Subscribe>> GetAllSubscribesAsync();
        Task<Subscribe> GetSubscribeByIdAsync(Guid id);
        Task<Guid> AddSubscribeAsync(Subscribe subscribe);
        Task<bool> UpdateSubscribeAsync(Subscribe subscribe);
        Task<bool> DeleteSubscribeAsync(Guid id);
    }
}
