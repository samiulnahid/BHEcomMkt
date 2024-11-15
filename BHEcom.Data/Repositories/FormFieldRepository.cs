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
    public class FormFieldRepository : IFormFieldRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<FormFieldRepository> _logger;
        public FormFieldRepository(ApplicationDbContext context, ILogger<FormFieldRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task AddAsync(FormField formField)
        {
            try
            {
                await _context.FormFields.AddAsync(formField);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {

               _logger.LogError(ex, "An error occurred while adding a formField.");
            }
        }

        public async Task<FormField> GetByIdAsync(Guid id)
        {
            return await _context.FormFields.FindAsync(id);
        }

        public async Task<IEnumerable<FormField>> GetAllAsync()
        {
            return await _context.FormFields.ToListAsync();
        }

        public async Task UpdateAsync(FormField formField)
        {
            try
            {
                _context.FormFields.Update(formField);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {

               _logger.LogError(ex, "An error occurred while updating a formField.");
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            var formField = await _context.FormFields.FindAsync(id);
            if (formField != null)
            {
                _context.FormFields.Remove(formField);
                await _context.SaveChangesAsync();
            }
        }
    }

}
