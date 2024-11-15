using BHEcom.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHEcom.Data.Repositories
{
    public interface IAdminRepository
    {
        Task AddAsync(Useres user);
        Task<Useres> GetByIdAsync(Guid id);
        Task<IEnumerable<Useres>> GetAllAsync();
        Task UpdateAsync(Useres user);
        Task DeleteAsync(Guid id);
    }

}
