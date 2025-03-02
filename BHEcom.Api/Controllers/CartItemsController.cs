using BHEcom.Common.Models;
using BHEcom.Data.Repositories;
using BHEcom.Services.Implementations;
using BHEcom.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BHEcom.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartItemsController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly ICartItemService _cartItemService;
        private readonly ILogger<CartItemRepository> _logger;
        public CartItemsController(ICartItemService cartItemService, IProductService productService, ILogger<CartItemRepository> logger)
        {
            _cartItemService = cartItemService;
            _productService = productService;
            _logger = logger;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] CartItem cartItem)
        {
            try
            {
                await _cartItemService.AddCartItemAsync(cartItem);
                return Ok(new { id = cartItem.CartItemID, Success = true });
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "An error occurred while adding a cartItem.");
                return StatusCode(500, new { Message = ex.Message, Success = false });

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
                    return Ok(new { data = cartItem, Message = "CartItem not found!", Success = true });
                }
                return Ok(new { data = cartItem, Success = true });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting a cartItem.");
                return StatusCode(500, new { Message = ex.Message, Success = false });

            }
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<CartItem>>> GetAll()
        {
            try
            {

                var cartItems = await _cartItemService.GetAllCartItemsAsync();
                if (cartItems == null)
                    return Ok(new { data = cartItems, Message = "No Data Available!", Success = true });
                return Ok(new { data = cartItems, Success = true });
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "An error occurred while getting all cartItem.");
                return StatusCode(500, new { Message = ex.Message, Success = false });

            }
        }

        [HttpPut("Update/{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] CartItem cartItem)
        {
            try
            {
                if (id != cartItem.CartItemID)
                {
                    return BadRequest();
                }
                await _cartItemService.UpdateCartItemAsync(cartItem);
                return Ok(new { Message = "Successfully Updated", Success = true });
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "An error occurred while updating a cartItem.");
                return StatusCode(500, new { Message = ex.Message, Success = false });

            }
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                bool isDeleted =  await _cartItemService.DeleteCartItemAsync(id);
                if (!isDeleted)
                    return StatusCode(500, new { message = "Delete unsuccessful!", success = false });
                return Ok(new
                {
                    Message = "CartItem Delete successfully.",
                    Success = true
                });
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "An error occurred while deleting a cartItem.");
                return StatusCode(500, new { Message = ex.Message, Success = false });

            }
        }

        [HttpPut("IncreaseCartItemQuantity")]
        public async Task<IActionResult> IncreaseCartItemQuantity([FromBody] CartItem cartItem)
        {
            try
            {
                if (cartItem == null || cartItem.CartItemID == Guid.Empty)
                    return Ok(new
                    {
                        Message = $"CartItemId Requried!",
                        Success = false
                    });

                var (isUpdate, message) = await _cartItemService.UpdateCartItemQuantityAsync(cartItem.CartItemID, "increase");

                return Ok(new { Message = message, Success = isUpdate });
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "An error occurred while updating a cartItem.");
                return StatusCode(500, new { Message = ex.Message, Success = false });

            }
        }



        [HttpPut("DecreaseCartItemQuantity")]
        public async Task<IActionResult> DecreaseCartItemQuantity([FromBody] CartItem cartItem)
        {
            try
            {
                if (cartItem == null || cartItem.CartItemID == Guid.Empty)
                    return Ok(new
                    {
                        Message = $"CartItemId Requried!",
                        Success = false
                    });

                var (isUpdate, message) = await _cartItemService.UpdateCartItemQuantityAsync(cartItem.CartItemID, "decrease");

                return Ok(new { Message = message, Success = isUpdate });
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "An error occurred while updating a cartItem.");
                return StatusCode(500, new { Message = ex.Message, Success = false });

            }
        }
    }

}
