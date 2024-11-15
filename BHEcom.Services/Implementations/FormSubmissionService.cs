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
    public class FormSubmissionService : IFormSubmissionService
    {
        private readonly IFormSubmissionRepository _formSubmissionRepository;

        public FormSubmissionService(IFormSubmissionRepository formSubmissionRepository)
        {
            _formSubmissionRepository = formSubmissionRepository;
        }

        public async Task<IEnumerable<FormSubmission>> GetAllFormSubmissionsAsync()
        {
            return await _formSubmissionRepository.GetAllAsync();
        }

        public async Task<FormSubmission> GetFormSubmissionByIdAsync(Guid id)
        {
            return await _formSubmissionRepository.GetByIdAsync(id);
        }

        public async Task AddFormSubmissionAsync(FormSubmission formSubmission)
        {
            await _formSubmissionRepository.AddAsync(formSubmission);
        }

        public async Task UpdateFormSubmissionAsync(FormSubmission formSubmission)
        {
            await _formSubmissionRepository.UpdateAsync(formSubmission);
        }

        public async Task DeleteFormSubmissionAsync(Guid id)
        {
            await _formSubmissionRepository.DeleteAsync(id);
        }
    }
}
