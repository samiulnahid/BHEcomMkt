using System.Collections.Generic;
using System.Threading.Tasks;
using BHEcom.Common.Models;
using BHEcom.Data.Repositories;
using BHEcom.Services.Interfaces;

namespace BHEcom.Services.Implementations
{
    public class BrandService : IBrandService
    {
        private readonly IBrandRepository _brandRepository;

        public BrandService(IBrandRepository brandRepository)
        {
            _brandRepository = brandRepository;
        }

        public async Task<IEnumerable<Brand>> GetAllBrandsAsync()
        {
            return await _brandRepository.GetAllAsync();
        }

        public async Task<Brand> GetBrandByIdAsync(Guid id)
        {
            return await _brandRepository.GetByIdAsync(id);
        }

        public async Task AddBrandAsync(Brand brand)
        {
            await _brandRepository.AddAsync(brand);
        }

        public async Task UpdateBrandAsync(Brand brand)
        {
            await _brandRepository.UpdateAsync(brand);
        }

        public async Task DeleteBrandAsync(Guid id)
        {
            await _brandRepository.DeleteAsync(id);
        }
    }
}
