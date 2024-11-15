using BHEcom.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHEcom.Data.Repositories
{
    public interface ISEORepository
    {
        Task AddAsync(SEO seoModel);
        Task<SEO> GetByIdAsync(Guid id);
        Task<IEnumerable<SEO>> GetAllAsync();
        Task UpdateAsync(SEO seoModel);
        Task DeleteAsync(Guid id);
    }
}
