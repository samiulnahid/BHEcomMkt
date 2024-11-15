using BHEcom.Common.Models;
using BHEcom.Data.Repositories;
using BHEcom.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHEcom.Services.Implementations
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;

        public PaymentService(IPaymentRepository paymentRepository)
        {
            _paymentRepository = paymentRepository;
        }

        public async Task<IEnumerable<Payment>> GetAllPaymentsAsync()
        {
            return await _paymentRepository.GetAllAsync();
        }

        public async Task<Payment> GetPaymentByIdAsync(Guid id)
        {
            return await _paymentRepository.GetByIdAsync(id);
        }

        public async Task AddPaymentAsync(Payment payment)
        {
            await _paymentRepository.AddAsync(payment);
        }

        public async Task UpdatePaymentAsync(Payment payment)
        {
            await _paymentRepository.UpdateAsync(payment);
        }

        public async Task DeletePaymentAsync(Guid id)
        {
            await _paymentRepository.DeleteAsync(id);
        }
    }

}
