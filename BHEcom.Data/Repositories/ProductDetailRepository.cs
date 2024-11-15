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

    public class ProductDetailRepository : IProductDetailRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ProductDetailRepository> _logger;
        public ProductDetailRepository(ApplicationDbContext context, ILogger<ProductDetailRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<bool> AddAsync(ProductDetail productDetail)
        {
            try
            {
                await _context.ProductDetails.AddAsync(productDetail);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                // Handle the exception as needed
               _logger.LogError(ex, "An error occurred while adding a productDetail.");
                return false;
            }
        }

        public async Task<ProductDetail> GetByIdAsync(Guid id)
        {
            return await _context.ProductDetails.FindAsync(id);
        }

        public async Task<IEnumerable<ProductDetail>> GetAllAsync()
        {
            return await _context.ProductDetails.ToListAsync();
        }
       
        public async Task UpdateAsync(ProductDetail productDetail)
        {
            try
            {
                _context.ProductDetails.Update(productDetail);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // Handle the exception as needed
               _logger.LogError(ex, "An error occurred while updating a productDetail.");
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            var productDetail = await _context.ProductDetails.FindAsync(id);
            if (productDetail != null)
            {
                _context.ProductDetails.Remove(productDetail);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<IEnumerable<ProductDetail>> GetAllByProductIdAsync(Guid id)
        {
            return await _context.ProductDetails
                .Where(p => p.ProductID == id)
                .ToListAsync() ?? new List<ProductDetail>(); ;
        }
        
    }
}
