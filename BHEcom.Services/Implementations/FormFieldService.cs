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
    public class FormFieldService : IFormFieldService
    {
        private readonly IFormFieldRepository _formFieldRepository;

        public FormFieldService(IFormFieldRepository formFieldRepository)
        {
            _formFieldRepository = formFieldRepository;
        }

        public async Task<IEnumerable<FormField>> GetAllFormFieldsAsync()
        {
            return await _formFieldRepository.GetAllAsync();
        }

        public async Task<FormField> GetFormFieldByIdAsync(Guid id)
        {
            return await _formFieldRepository.GetByIdAsync(id);
        }

        public async Task AddFormFieldAsync(FormField formField)
        {
            await _formFieldRepository.AddAsync(formField);
        }

        public async Task UpdateFormFieldAsync(FormField formField)
        {
            await _formFieldRepository.UpdateAsync(formField);
        }

        public async Task DeleteFormFieldAsync(Guid id)
        {
            await _formFieldRepository.DeleteAsync(id);
        }
    }
}
