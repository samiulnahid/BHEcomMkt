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
    public class PageContentRepository : IPageContentRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<PageContentRepository> _logger;
        public PageContentRepository(ApplicationDbContext context, ILogger<PageContentRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task AddAsync(PageContent pageContent)
        {
            try
            {
                await _context.PageContents.AddAsync(pageContent);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {

               _logger.LogError(ex, "An error occurred while adding a pageContent.");
            }
        }

        public async Task<PageContent> GetByIdAsync(Guid id)
        {
            return await _context.PageContents.FindAsync(id);
        }

        public async Task<IEnumerable<PageContent>> GetAllAsync()
        {
            return await _context.PageContents.ToListAsync();
        }

        public async Task UpdateAsync(PageContent pageContent)
        {
            try
            {
                _context.PageContents.Update(pageContent);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {

               _logger.LogError(ex, "An error occurred while updating a pageContent.");
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            var pageContent = await _context.PageContents.FindAsync(id);
            if (pageContent != null)
            {
                _context.PageContents.Remove(pageContent);
                await _context.SaveChangesAsync();
            }
        }
    }
}
