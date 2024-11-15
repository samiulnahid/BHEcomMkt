using BHEcom.Common.Models;
using BHEcom.Data.Repositories;
using BHEcom.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BHEcom.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;
        private readonly ILogger<CartRepository> _logger;
        public CartController(ICartService cartService, ILogger<CartRepository> logger)
        {
            _cartService = cartService;
            _logger = logger;
        }

        [HttpPost("Create")]
        public async Task<ActionResult> Create([FromBody] Cart cart)
        {
            try
            {
                await _cartService.AddCartAsync(cart);
                return CreatedAtAction(nameof(GetById), new { id = cart.CartID }, cart);
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "An error occurred while adding a cart.");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("GetById/{id}")]
        public async Task<ActionResult<Cart>> GetById(Guid id)
        {
            try
            {
                var cart = await _cartService.GetCartByIdAsync(id);
                if (cart == null)
                {
                    return NotFound();
                }
                return Ok(cart);
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "An error occurred while getting a cart.");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<Cart>>> GetAll()
        {
            try
            {
                var carts = await _cartService.GetAllCartsAsync();
                return Ok(carts);
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "An error occurred while getting all cart.");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("Update/{id}")]
        public async Task<ActionResult> Update(Guid id, [FromBody] Cart cart)
        {
            try
            {
                if (id != cart.CartID)
                {
                    return BadRequest();
                }
                await _cartService.UpdateCartAsync(cart);
                return NoContent();
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "An error occurred while updating a cart.");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("Delete/{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            try
            {
                await _cartService.DeleteCartAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "An error occurred while deleting a cart.");
                return StatusCode(500, ex.Message);
            }
        }
    }

}
