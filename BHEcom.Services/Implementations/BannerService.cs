using System.Collections.Generic;
using System.Threading.Tasks;
using BHEcom.Common.Models;
using BHEcom.Data.Repositories;
using BHEcom.Services.Interfaces;

namespace BHEcom.Services.Implementations
{
    public class BannerService : IBannerService
    {
        private readonly IBannerRepository _bannerRepository;

        public BannerService(IBannerRepository bannerRepository)
        {
            _bannerRepository = bannerRepository;
        }

        public async Task<IEnumerable<Banner>> GetAllBannersAsync()
        {
            return await _bannerRepository.GetAllAsync();
        }

        public async Task<Banner> GetBannerByIdAsync(Guid id)
        {
            return await _bannerRepository.GetByIdAsync(id);
        }

        public async Task<Guid> AddBannerAsync(Banner banner)
        {
            return await _bannerRepository.AddAsync(banner);
        }

        public async Task<(bool isUpdated, string? oldImageUrl)> UpdateBannerAsync(Banner banner)
        {
            return await _bannerRepository.UpdateAsync(banner);
        }

        public async Task DeleteBannerAsync(Guid id)
        {
            await _bannerRepository.DeleteAsync(id);
        }
    }

}
