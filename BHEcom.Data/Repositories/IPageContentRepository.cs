using BHEcom.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHEcom.Data.Repositories
{
    public interface IPageContentRepository
    {
        Task AddAsync(PageContent pageContent);
        Task<PageContent> GetByIdAsync(Guid id);
        Task<IEnumerable<PageContent>> GetAllAsync();
        Task UpdateAsync(PageContent pageContent);
        Task DeleteAsync(Guid id);
    }
}
