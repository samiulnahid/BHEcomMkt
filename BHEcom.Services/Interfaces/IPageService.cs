using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BHEcom.Common.Models;


namespace BHEcom.Services.Interfaces
{
    public interface IPageService
    {
        Task<IEnumerable<Page>> GetAllPagesAsync();
        Task<Page> GetPageByIdAsync(Guid id);
        Task AddPageAsync(Page page);
        Task UpdatePageAsync(Page page);
        Task DeletePageAsync(Guid id);
    }

}
