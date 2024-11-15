using BHEcom.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHEcom.Services.Interfaces
{
    public interface IFormSubmissionFieldService
    {
        Task<IEnumerable<FormSubmissionField>> GetAllFormSubmissionFieldsAsync();
        Task<FormSubmissionField> GetFormSubmissionFieldByIdAsync(Guid id);
        Task AddFormSubmissionFieldAsync(FormSubmissionField formSubmissionField);
        Task UpdateFormSubmissionFieldAsync(FormSubmissionField formSubmissionField);
        Task DeleteFormSubmissionFieldAsync(Guid id);
    }
}
