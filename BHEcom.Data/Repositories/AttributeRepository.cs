using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BHEcom.Common.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BHEcom.Data.Repositories
{
    public class AttributeRepository : IAttributeRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<AttributeRepository> _logger;
        public AttributeRepository(ApplicationDbContext context, ILogger<AttributeRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task AddAsync(Attributes attribute)
        {
            try
            {
                await _context.Attributes.AddAsync(attribute);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {

               _logger.LogError(ex, "An error occurred while adding a attribute.");
            }
        }

        public async Task<Attributes> GetByIdAsync(Guid id)
        {
            return await _context.Attributes.FindAsync(id);
        }

        public async Task<IEnumerable<Attributes>> GetAllAsync()
        {
            return await _context.Attributes.ToListAsync();
        }

        public async Task UpdateAsync(Attributes attribute)
        {
            try
            {
                _context.Attributes.Update(attribute);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {

               _logger.LogError(ex, "An error occurred while updating a attribute.");
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            var attribute = await _context.Attributes.FindAsync(id);
            if (attribute != null)
            {
                _context.Attributes.Remove(attribute);
                await _context.SaveChangesAsync();
            }
        }
    }

}
