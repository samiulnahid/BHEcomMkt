using System.Collections.Generic;
using System.Threading.Tasks;
using BHEcom.Common.Models;

namespace BHEcom.Data.Repositories
{
    public interface IBrandRepository
    {
        Task<IEnumerable<Brand>> GetAllAsync();
        Task<Brand> GetByIdAsync(Guid id);
        Task<(Guid id, bool isUnique)> AddAsync(Brand brand);
        Task<(bool isUpdated, string? oldImageUrl, bool isUniqueName)> UpdateAsync(Brand brand);
        Task DeleteAsync(Guid id);
    }
}
