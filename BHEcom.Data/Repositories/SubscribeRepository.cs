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

    public class SubscribeRepository : ISubscribeRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<SubscribeRepository> _logger;

        public SubscribeRepository(ApplicationDbContext context, ILogger<SubscribeRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Guid> AddAsync(Subscribe subscribe)
        {
            try
            {
                await _context.Subscribe.AddAsync(subscribe);
                await _context.SaveChangesAsync();
                return subscribe.SubscribeID;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding a subscription.");
                return Guid.Empty;
            }
        }

        public async Task<Subscribe> GetByIdAsync(Guid id)
        {
            return await _context.Subscribe.FindAsync(id);
        }

        public async Task<IEnumerable<Subscribe>> GetAllAsync()
        {
            return await _context.Subscribe.ToListAsync();
        }

        public async Task<bool> UpdateAsync(Subscribe subscribe)
        {
            try
            {
                _context.Subscribe.Update(subscribe);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating a subscription.");
                return false;
            }
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            try
            {
                var subscribe = await _context.Subscribe.FindAsync(id);
                if (subscribe == null) return false;

                _context.Subscribe.Remove(subscribe);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting a subscription.");
                return false;
            }
        }
    }
}
