using System.Collections.Generic;
using System.Threading.Tasks;
using BHEcom.Common.Models;

namespace BHEcom.Services.Interfaces
{
    public interface IBrandService
    {
        Task<IEnumerable<Brand>> GetAllBrandsAsync();
        Task<Brand> GetBrandByIdAsync(Guid id);
        Task AddBrandAsync(Brand brand);
        Task UpdateBrandAsync(Brand brand);
        Task DeleteBrandAsync(Guid id);
    }
}
