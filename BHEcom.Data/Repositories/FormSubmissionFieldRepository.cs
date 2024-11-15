using BHEcom.Common.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHEcom.Data.Repositories
{
    public class FormSubmissionFieldRepository : IFormSubmissionFieldRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<FormSubmissionFieldRepository> _logger;
        public FormSubmissionFieldRepository(ApplicationDbContext context, ILogger<FormSubmissionFieldRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task AddAsync(FormSubmissionField formSubmissionField)
        {
            try
            {
                await _context.FormSubmissionFields.AddAsync(formSubmissionField);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {

               _logger.LogError(ex, "An error occurred while adding a formSubmissionField.");
            }
        }

        public async Task<FormSubmissionField> GetByIdAsync(Guid id)
        {
            return await _context.FormSubmissionFields.FindAsync(id);
        }

        public async Task<IEnumerable<FormSubmissionField>> GetAllAsync()
        {
            return await _context.FormSubmissionFields.ToListAsync();
        }

        public async Task UpdateAsync(FormSubmissionField formSubmissionField)
        {
            try
            {
                _context.FormSubmissionFields.Update(formSubmissionField);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {

               _logger.LogError(ex, "An error occurred while updating a formSubmissionField.");
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            var formSubmissionField = await _context.FormSubmissionFields.FindAsync(id);
            if (formSubmissionField != null)
            {
                _context.FormSubmissionFields.Remove(formSubmissionField);
                await _context.SaveChangesAsync();
            }
        }
    }

}
