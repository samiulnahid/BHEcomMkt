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
    public class FormsRepository : IFormsRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<FormsRepository> _logger;
        public FormsRepository(ApplicationDbContext context, ILogger<FormsRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task AddAsync(Forms form)
        {
            try
            {
                await _context.Forms.AddAsync(form);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {

               _logger.LogError(ex, "An error occurred while adding a forms.");
            }
        }

        public async Task<Forms> GetByIdAsync(Guid id)
        {
            return await _context.Forms.FindAsync(id);
        }

        public async Task<IEnumerable<Forms>> GetAllAsync()
        {
            return await _context.Forms.ToListAsync();
        }

        public async Task UpdateAsync(Forms form)
        {
            try
            {
                _context.Forms.Update(form);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {

               _logger.LogError(ex, "An error occurred while updating a forms.");
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            var form = await _context.Forms.FindAsync(id);
            if (form != null)
            {
                _context.Forms.Remove(form);
                await _context.SaveChangesAsync();
            }
        }
    }

}
