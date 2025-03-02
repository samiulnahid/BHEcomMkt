using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BHEcom.Common.Models;

namespace BHEcom.Data.Repositories
{
    public interface IBannerRepository
    {
        Task<Guid> AddAsync(Banner banner);
        Task<Banner> GetByIdAsync(Guid id);
        Task<IEnumerable<Banner>> GetAllAsync();
        Task<(bool isUpdated, string? oldImageUrl)> UpdateAsync(Banner banner);
        Task DeleteAsync(Guid id);
    }


}
