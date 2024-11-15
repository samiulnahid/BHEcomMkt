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
    public class ReviewRepository : IReviewRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ReviewRepository> _logger;
        public ReviewRepository(ApplicationDbContext context, ILogger<ReviewRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task AddAsync(Review review)
        {
            try
            {
                await _context.Reviews.AddAsync(review);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // Handle the exception as needed
               _logger.LogError(ex, "An error occurred while adding a review.");
            }
        }

        public async Task<Review> GetByIdAsync(Guid id)
        {
            return await _context.Reviews.FindAsync(id);
        }

        public async Task<IEnumerable<Review>> GetAllAsync()
        {
            return await _context.Reviews.ToListAsync();
        }

        public async Task UpdateAsync(Review review)
        {
            try
            {
                _context.Reviews.Update(review);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // Handle the exception as needed
               _logger.LogError(ex, "An error occurred while updating a review.");
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            var review = await _context.Reviews.FindAsync(id);
            if (review != null)
            {
                _context.Reviews.Remove(review);
                await _context.SaveChangesAsync();
            }
        }
    }
}
