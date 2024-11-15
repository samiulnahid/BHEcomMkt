using BHEcom.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHEcom.Data.Repositories
{
    public interface IFormSubmissionFieldRepository
    {
        Task AddAsync(FormSubmissionField formSubmissionField);
        Task<FormSubmissionField> GetByIdAsync(Guid id);
        Task<IEnumerable<FormSubmissionField>> GetAllAsync();
        Task UpdateAsync(FormSubmissionField formSubmissionField);
        Task DeleteAsync(Guid id);
    }

}
