using BHEcom.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHEcom.Data.Repositories
{
    public interface ISubscribeRepository
    {
        Task<Guid> AddAsync(Subscribe subscribe);
        Task<Subscribe> GetByIdAsync(Guid id);
        Task<IEnumerable<Subscribe>> GetAllAsync();
        Task<bool> UpdateAsync(Subscribe subscribe);
        Task<bool> DeleteAsync(Guid id);
    }
}
