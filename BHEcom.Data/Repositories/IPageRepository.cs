using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BHEcom.Common.Models;

namespace BHEcom.Data.Repositories
{
    public interface IPageRepository
    {
        Task AddAsync(Page page);
        Task<Page> GetByIdAsync(Guid id);
        Task<IEnumerable<Page>> GetAllAsync();
        Task UpdateAsync(Page page);
        Task DeleteAsync(Guid id);
    }
}
