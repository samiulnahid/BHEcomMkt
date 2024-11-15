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

    public class PaymentRepository : IPaymentRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<PaymentRepository> _logger;
        public PaymentRepository(ApplicationDbContext context, ILogger<PaymentRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task AddAsync(Payment payment)
        {
            try
            {
                await _context.Payments.AddAsync(payment);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {

               _logger.LogError(ex, "An error occurred while adding a payment.");
            }
        }

        public async Task<Payment> GetByIdAsync(Guid id)
        {
            return await _context.Payments.FindAsync(id);
        }

        public async Task<IEnumerable<Payment>> GetAllAsync()
        {
            return await _context.Payments.ToListAsync();
        }

        public async Task UpdateAsync(Payment payment)
        {
            try
            {
                _context.Payments.Update(payment);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {

               _logger.LogError(ex, "An error occurred while updating a payment.");
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            var payment = await _context.Payments.FindAsync(id);
            if (payment != null)
            {
                _context.Payments.Remove(payment);
                await _context.SaveChangesAsync();
            }
        }
    }
}
