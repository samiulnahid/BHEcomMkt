using Microsoft.AspNetCore.Mvc;
using BHEcom.Common.Models;
using BHEcom.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BHEcom.Services.Interfaces;
using BHEcom.Services.Implementations;
using System.ComponentModel.DataAnnotations;


namespace BHEcom.Api.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IOrderDetailService _orderDetailService;
        private readonly ICartService _cartService;
        private readonly ILogger<OrderRepository> _logger;
        public OrdersController(IOrderService orderService, ICartService cartService, IOrderDetailService orderDetailService, ILogger<OrderRepository> logger)
        {
            _orderService = orderService;
            _cartService = cartService;
            _orderDetailService = orderDetailService;
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


        [HttpPost("CreateOrder")]
        public async Task<ActionResult> CreateOrder([FromBody] Order model)
        {
            try
            {
               // if (order == null ||  order.CartProductList == null || !order.CartProductList.Any())
                if (model == null ||model.CartID == null || model.CartID == Guid.Empty)
                    return BadRequest(new { Success = false, Message = "Invalid data." });

                Guid cartId = model.CartID ?? Guid.Empty;

                var cartManager = await _cartService.GetCartManagerByCartIdAsync(cartId);
                var allAmount = cartManager.Sum(item =>item.TotalPrice ?? 0);

                model.TotalAmount = allAmount;
                Guid orderId = await _orderService.AddOrderAsync(model);
                if(orderId == Guid.Empty)
                    return StatusCode(500, "Order Creation Failed!");

                foreach (var item in cartManager)
                {
                    OrderDetail detail = new OrderDetail()
                    {
                        OrderID = orderId,
                        ProductID = item.ProductID,
                        Quantity = item.Quantity,
                        Price = item.Price ?? 0,
                    };
                    Guid detailId = await _orderDetailService.AddOrderDetailAsync(detail);
                    if (detailId == Guid.Empty)
                        return StatusCode(500, "Order Details Creation Failed!");
                }

                return Ok(new { OrderId = orderId, Success = true, Message = "Successfully create order & order details." });


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
        public async Task<ActionResult> Update(Guid id, [FromBody] string status)
        {
            try
            {
                if (id == Guid.Empty)
                {
                    return BadRequest();
                }

                bool isUpdated = await _orderService.UpdateOrderAsync(id, status);
                if (!isUpdated)
                    return Ok(new { Success = false, Message = "Order status update unsuccessful." });

                return Ok(new { Success = true, Message = "Successfully updated order status." });
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
