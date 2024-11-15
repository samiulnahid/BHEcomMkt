using Microsoft.AspNetCore.Mvc;
using BHEcom.Common.Models;
using BHEcom.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BHEcom.Services.Interfaces;


namespace BHEcom.Api.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly ILogger<OrderRepository> _logger;
        public OrdersController(IOrderService orderService, ILogger<OrderRepository> logger)
        {
            _orderService = orderService;
            _logger = logger;
        }

        [HttpPost("Create")]
        public async Task<ActionResult> Create([FromBody] Order order)
        {
            try
            {
                await _orderService.AddOrderAsync(order);
                return CreatedAtAction(nameof(GetById), new { id = order.OrderID }, order);
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "An error occurred while adding a order.");
                return StatusCode(500, ex.Message);

            }
        }

        [HttpGet("GetById/{id}")]
        public async Task<ActionResult<Order>> GetById(Guid id)
        {
            try
            {
                var order = await _orderService.GetOrderByIdAsync(id);
                if (order == null)
                {
                    return NotFound();
                }
                return Ok(order);
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "An error occurred while getting a order.");
                return StatusCode(500, ex.Message);

            }
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<Order>>> GetAll()
        {
            try
            {
                var orders = await _orderService.GetAllOrdersAsync();
                return Ok(orders);
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "An error occurred while getting all order.");
                return StatusCode(500, ex.Message);

            }
        }

        [HttpPut("Update/{id}")]
        public async Task<ActionResult> Update(Guid id, [FromBody] Order order)
        {
            try
            {
                if (id != order.OrderID)
                {
                    return BadRequest();
                }

                await _orderService.UpdateOrderAsync(order);
                return NoContent();
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "An error occurred while updating a order.");
                return StatusCode(500, ex.Message);

            }
        }

        [HttpDelete("Delete/{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            try
            {
                await _orderService.DeleteOrderAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "An error occurred while deleting a order.");
                return StatusCode(500, ex.Message);

            }
        }
    }

}
