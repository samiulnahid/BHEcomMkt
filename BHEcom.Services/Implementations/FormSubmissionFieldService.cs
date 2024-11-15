using BHEcom.Common.Models;
using BHEcom.Data.Repositories;
using BHEcom.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHEcom.Services.Implementations
{
    public class FormSubmissionFieldService : IFormSubmissionFieldService
    {
        private readonly IFormSubmissionFieldRepository _formSubmissionFieldRepository;

        public FormSubmissionFieldService(IFormSubmissionFieldRepository formSubmissionFieldRepository)
        {
            _formSubmissionFieldRepository = formSubmissionFieldRepository;
        }

        public async Task<IEnumerable<FormSubmissionField>> GetAllFormSubmissionFieldsAsync()
        {
            return await _formSubmissionFieldRepository.GetAllAsync();
        }

        public async Task<FormSubmissionField> GetFormSubmissionFieldByIdAsync(Guid id)
        {
            return await _formSubmissionFieldRepository.GetByIdAsync(id);
        }

        public async Task AddFormSubmissionFieldAsync(FormSubmissionField formSubmissionField)
        {
            await _formSubmissionFieldRepository.AddAsync(formSubmissionField);
        }

        public async Task UpdateFormSubmissionFieldAsync(FormSubmissionField formSubmissionField)
        {
            await _formSubmissionFieldRepository.UpdateAsync(formSubmissionField);
        }

        public async Task DeleteFormSubmissionFieldAsync(Guid id)
        {
            await _formSubmissionFieldRepository.DeleteAsync(id);
        }
    }
}
