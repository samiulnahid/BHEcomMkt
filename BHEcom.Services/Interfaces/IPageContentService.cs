using BHEcom.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHEcom.Services.Interfaces
{
    public interface IPageContentService
    {
        Task<IEnumerable<PageContent>> GetAllPageContentsAsync();
        Task<PageContent> GetPageContentByIdAsync(Guid id);
        Task AddPageContentAsync(PageContent pageContent);
        Task UpdatePageContentAsync(PageContent pageContent);
        Task DeletePageContentAsync(Guid id);
    }

}
