using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BHEcom.Common.Models;
using Microsoft.Extensions.Logging;


namespace BHEcom.Data.Repositories
{
  
    public class PageRepository : IPageRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<PageRepository> _logger;
        public PageRepository(ApplicationDbContext context, ILogger<PageRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task AddAsync(Page page)
        {
            try
            {
                await _context.Pages.AddAsync(page);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {

               _logger.LogError(ex, "An error occurred while adding a page.");
            }
        }

        public async Task<Page> GetByIdAsync(Guid id)
        {
            return await _context.Pages.FindAsync(id);
        }

        public async Task<IEnumerable<Page>> GetAllAsync()
        {
            return await _context.Pages.ToListAsync();
        }

        public async Task UpdateAsync(Page page)
        {
            try
            {
                _context.Pages.Update(page);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {

               _logger.LogError(ex, "An error occurred while updating a page.");
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            var page = await _context.Pages.FindAsync(id);
            if (page != null)
            {
                _context.Pages.Remove(page);
                await _context.SaveChangesAsync();
            }
        }
    }

}
