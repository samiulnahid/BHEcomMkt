using BHEcom.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHEcom.Data.Repositories
{
    public interface IFormsRepository
    {
        Task AddAsync(Forms form);
        Task<Forms> GetByIdAsync(Guid id);
        Task<IEnumerable<Forms>> GetAllAsync();
        Task UpdateAsync(Forms form);
        Task DeleteAsync(Guid id);
    }

}
