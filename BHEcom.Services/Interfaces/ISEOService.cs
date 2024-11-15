using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BHEcom.Common.Models;


namespace BHEcom.Services.Interfaces
{
    public interface ISEOService
    {
        Task<IEnumerable<SEO>> GetAllSEOAsync();
        Task<SEO> GetSEOByIdAsync(Guid id);
        Task AddSEOAsync(SEO seo);
        Task UpdateSEOAsync(SEO seo);
        Task DeleteSEOAsync(Guid id);
    }

}
