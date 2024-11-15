using BHEcom.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHEcom.Services.Interfaces
{
    public interface IFormSubmissionService
    {
        Task<IEnumerable<FormSubmission>> GetAllFormSubmissionsAsync();
        Task<FormSubmission> GetFormSubmissionByIdAsync(Guid id);
        Task AddFormSubmissionAsync(FormSubmission formSubmission);
        Task UpdateFormSubmissionAsync(FormSubmission formSubmission);
        Task DeleteFormSubmissionAsync(Guid id);
    }
}
