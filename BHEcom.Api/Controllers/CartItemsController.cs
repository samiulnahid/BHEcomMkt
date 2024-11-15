using BHEcom.Common.Models;
using BHEcom.Data.Repositories;
using BHEcom.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BHEcom.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartItemsController : ControllerBase
    {
        private readonly ICartItemService _cartItemService;
        private readonly ILogger<CartItemRepository> _logger;
        public CartItemsController(ICartItemService cartItemService, ILogger<CartItemRepository> logger)
        {
            _cartItemService = cartItemService;
            _logger = logger;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] CartItem cartItem)
        {
            try
            {
                await _cartItemService.AddCartItemAsync(cartItem);
                return CreatedAtAction(nameof(GetById), new { id = cartItem.CartItemID }, cartItem);
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "An error occurred while adding a cartItem.");
                return StatusCode(500, ex.Message);

            }
        }

        [HttpGet("GetById/{id}")]
        public async Task<ActionResult<CartItem>> GetById(Guid id)
        {
            try
            {
                var cartItem = await _cartItemService.GetCartItemByIdAsync(id);
                if (cartItem == null)
                {
                    return NotFound();
                }
                return Ok(cartItem);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting a cartItem.");
                return StatusCode(500, ex.Message);

            }
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<CartItem>>> GetAll()
        {
            try
            {

                var cartItems = await _cartItemService.GetAllCartItemsAsync();
                return Ok(cartItems);
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "An error occurred while getting all cartItem.");
                return StatusCode(500, ex.Message);

            }
        }

        [HttpPut("Update/{id}")]
        public async Task<IActionResult> UpdateAsync(Guid id, [FromBody] CartItem cartItem)
        {
            try
            {
                if (id != cartItem.CartItemID)
                {
                    return BadRequest();
                }
                await _cartItemService.UpdateCartItemAsync(cartItem);
                return NoContent();
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "An error occurred while updating a cartItem.");
                return StatusCode(500, ex.Message);

            }
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            try
            {
                await _cartItemService.DeleteCartItemAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "An error occurred while deleting a cartItem.");
                return StatusCode(500, ex.Message);

            }
        }
    }

}
