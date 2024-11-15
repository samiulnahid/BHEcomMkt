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
    public class SEORepository : ISEORepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<SEORepository> _logger;
        public SEORepository(ApplicationDbContext context, ILogger<SEORepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task AddAsync(SEO seoModel)
        {
            try
            {
                await _context.SEO.AddAsync(seoModel);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // Handle the exception as needed
               _logger.LogError(ex, "An error occurred while adding a SEO.");
            }
        }

        public async Task<SEO> GetByIdAsync(Guid id)
        {
            return await _context.SEO.FindAsync(id);
        }

        public async Task<IEnumerable<SEO>> GetAllAsync()
        {
            return await _context.SEO.ToListAsync();
        }

        public async Task UpdateAsync(SEO seoModel)
        {
            try
            {
                _context.SEO.Update(seoModel);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // Handle the exception as needed
               _logger.LogError(ex, "An error occurred while updating a SEO.");
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            var seoModel = await _context.SEO.FindAsync(id);
            if (seoModel != null)
            {
                _context.SEO.Remove(seoModel);
                await _context.SaveChangesAsync();
            }
        }
    }
}
