using BHEcom.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHEcom.Data.Repositories
{
    public interface IPaymentRepository
    {
        Task AddAsync(Payment payment);
        Task<Payment> GetByIdAsync(Guid id);
        Task<IEnumerable<Payment>> GetAllAsync();
        Task UpdateAsync(Payment payment);
        Task DeleteAsync(Guid id);
    }
}
