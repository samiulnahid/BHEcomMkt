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
    public class FormSubmissionRepository : IFormSubmissionRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<FormSubmissionRepository> _logger;
        public FormSubmissionRepository(ApplicationDbContext context, ILogger<FormSubmissionRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task AddAsync(FormSubmission formSubmission)
        {
            try
            {
                await _context.FormSubmissions.AddAsync(formSubmission);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {

               _logger.LogError(ex, "An error occurred while adding a formSubmission.");
            }
        }

        public async Task<FormSubmission> GetByIdAsync(Guid id)
        {
            return await _context.FormSubmissions.FindAsync(id);
        }

        public async Task<IEnumerable<FormSubmission>> GetAllAsync()
        {
            return await _context.FormSubmissions.ToListAsync();
        }

        public async Task UpdateAsync(FormSubmission formSubmission)
        {
            try
            {
                _context.FormSubmissions.Update(formSubmission);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {

               _logger.LogError(ex, "An error occurred while updating a formSubmission.");
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            var formSubmission = await _context.FormSubmissions.FindAsync(id);
            if (formSubmission != null)
            {
                _context.FormSubmissions.Remove(formSubmission);
                await _context.SaveChangesAsync();
            }
        }
    }

}
