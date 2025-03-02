using BHEcom.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHEcom.Services.Interfaces
{
    public interface IBannerService
    {
        Task<IEnumerable<Banner>> GetAllBannersAsync();
        Task<Banner> GetBannerByIdAsync(Guid id);
        Task<Guid> AddBannerAsync(Banner banner);
        Task<(bool isUpdated, string? oldImageUrl)> UpdateBannerAsync(Banner banner);
        Task DeleteBannerAsync(Guid id);
    }

}
