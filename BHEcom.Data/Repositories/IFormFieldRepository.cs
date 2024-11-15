using BHEcom.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHEcom.Data.Repositories
{
    public interface IFormFieldRepository
    {
        Task AddAsync(FormField formField);
        Task<FormField> GetByIdAsync(Guid id);
        Task<IEnumerable<FormField>> GetAllAsync();
        Task UpdateAsync(FormField formField);
        Task DeleteAsync(Guid id);
    }

}
