using Microsoft.AspNetCore.Mvc;
using BHEcom.Common.Models;
using BHEcom.Data.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;
using BHEcom.Services.Interfaces;

namespace BHEcom.Api.Controllers
{
    

    [ApiController]
    [Route("api/[controller]")]
    public class OrderDetailsController : ControllerBase
    {
        private readonly IOrderDetailService _orderDetailService;
        private readonly ILogger<OrderDetailRepository> _logger;
        public OrderDetailsController(IOrderDetailService orderDetailService, ILogger<OrderDetailRepository> logger)
        {
            _orderDetailService = orderDetailService;
            _logger = logger;
        }

        [HttpPost("Create")]
        public async Task<ActionResult> Create([FromBody] OrderDetail orderDetail)
        {
            try
            {
                await _orderDetailService.AddOrderDetailAsync(orderDetail);
                return CreatedAtAction(nameof(GetById), new { id = orderDetail.OrderDetailID }, orderDetail);
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "An error occurred while adding a orderDetail.");
                return StatusCode(500, ex.Message);

            }
        }

        [HttpGet("GetById/{id}")]
        public async Task<ActionResult<OrderDetail>> GetById(Guid id)
        {
            try
            {
                var orderDetail = await _orderDetailService.GetOrderDetailByIdAsync(id);
                if (orderDetail == null)
                {
                    return NotFound();
                }
                return Ok(orderDetail);
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "An error occurred while getting a orderDetail.");
                return StatusCode(500, ex.Message);

            }
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<OrderDetail>>> GetAll()
        {
            try
            {
                var orderDetails = await _orderDetailService.GetAllOrderDetailsAsync();
                return Ok(orderDetails);
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "An error occurred while getting all orderDetail.");
                return StatusCode(500, ex.Message);

            }
        }

        [HttpPut("Update/{id}")]
        public async Task<ActionResult> Update(Guid id, [FromBody] OrderDetail orderDetail)
        {
            try
            {
                if (id != orderDetail.OrderDetailID)
                {
                    return BadRequest();
                }

                await _orderDetailService.UpdateOrderDetailAsync(orderDetail);
                return NoContent();
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "An error occurred while updating a orderDetail.");
                return StatusCode(500, ex.Message);

            }
        }

        [HttpDelete("Delete/{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            try
            {
                await _orderDetailService.DeleteOrderDetailAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "An error occurred while deleting a orderDetail.");
                return StatusCode(500, ex.Message);

            }
        }
    }

}
