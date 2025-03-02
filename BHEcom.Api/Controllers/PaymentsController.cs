using BHEcom.Common.Models;
using BHEcom.Data.Repositories;
using BHEcom.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BHEcom.Api.Controllers
{


    [ApiController]
    [Route("api/[controller]")]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        private readonly ILogger<PaymentRepository> _logger;
        public PaymentsController(IPaymentService paymentService, ILogger<PaymentRepository> logger)
        {
            _paymentService = paymentService;
            _logger = logger;
        }

        [HttpPost("Create")]
        public async Task<ActionResult> Create([FromBody] Payment payment)
        {
            try
            {
                await _paymentService.AddPaymentAsync(payment);
                return Ok(new { id = payment.PaymentID, Success = true });
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "An error occurred while adding a payment.");
                return StatusCode(500, new { Message = ex.Message, Success = false });
            }
        }

        [HttpGet("GetById/{id}")]
        public async Task<ActionResult<Payment>> GetById(Guid id)
        {
            try
            {
                var payment = await _paymentService.GetPaymentByIdAsync(id);
                if (payment == null)
                {
                    return NotFound();
                }
                return Ok(new { data = payment, Success = true });
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "An error occurred while getting a payment.");
                return StatusCode(500, new { Message = ex.Message, Success = false });
            }
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<Payment>>> GetAll()
        {
            try
            {
                var payments = await _paymentService.GetAllPaymentsAsync();
                return Ok(new { data = payments, Success = true });
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "An error occurred while getting all payment.");
                return StatusCode(500, new { Message = ex.Message, Success = false });
            }
        }

        [HttpPut("Update/{id}")]
        public async Task<ActionResult> Update(Guid id, [FromBody] Payment payment)
        {
            try
            {
                if (id != payment.PaymentID)
                {
                    return BadRequest();
                }
                await _paymentService.UpdatePaymentAsync(payment);
                return Ok(new { Message = "Successfully Updated", Success = true });
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "An error occurred while updating a payment.");
                return StatusCode(500, new { Message = ex.Message, Success = false });
            }
        }

        [HttpDelete("Delete/{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            try
            {
                await _paymentService.DeletePaymentAsync(id);
                return Ok(new { Message = "Successfully Deleted", Success = true });
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "An error occurred while deleting a payment.");
                return StatusCode(500, new { Message = ex.Message, Success = false });
            }
        }
    }

}
