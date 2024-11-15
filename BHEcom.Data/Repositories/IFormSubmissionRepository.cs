using BHEcom.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHEcom.Data.Repositories
{
    public interface IFormSubmissionRepository
    {
        Task AddAsync(FormSubmission formSubmission);
        Task<FormSubmission> GetByIdAsync(Guid id);
        Task<IEnumerable<FormSubmission>> GetAllAsync();
        Task UpdateAsync(FormSubmission formSubmission);
        Task DeleteAsync(Guid id);
    }

}
