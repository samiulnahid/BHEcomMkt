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
    public class FormsService : IFormsService
    {
        private readonly IFormsRepository _formsRepository;

        public FormsService(IFormsRepository formsRepository)
        {
            _formsRepository = formsRepository;
        }

        public async Task<IEnumerable<Forms>> GetAllFormssAsync()
        {
            return await _formsRepository.GetAllAsync();
        }

        public async Task<Forms> GetFormsByIdAsync(Guid id)
        {
            return await _formsRepository.GetByIdAsync(id);
        }

        public async Task AddFormsAsync(Forms forms)
        {
            await _formsRepository.AddAsync(forms);
        }

        public async Task UpdateFormsAsync(Forms forms)
        {
            await _formsRepository.UpdateAsync(forms);
        }

        public async Task DeleteFormsAsync(Guid id)
        {
            await _formsRepository.DeleteAsync(id);
        }
    }
}
