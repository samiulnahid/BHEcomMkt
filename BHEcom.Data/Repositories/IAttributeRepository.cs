using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BHEcom.Common.Models;

namespace BHEcom.Data.Repositories
{
    public interface IAttributeRepository
    {
        Task AddAsync(Attributes attribute);
        Task<Attributes> GetByIdAsync(Guid id);
        Task<IEnumerable<Attributes>> GetAllAsync();
        Task UpdateAsync(Attributes attribute);
        Task DeleteAsync(Guid id);
    }

}
