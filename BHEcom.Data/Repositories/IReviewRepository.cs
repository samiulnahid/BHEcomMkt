using BHEcom.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHEcom.Data.Repositories
{
    public interface IReviewRepository
    {
        Task AddAsync(Review review);
        Task<Review> GetByIdAsync(Guid id);
        Task<IEnumerable<Review>> GetAllAsync();
        Task UpdateAsync(Review review);
        Task DeleteAsync(Guid id);
    }
}
