using BHEcom.Common.Models;
using BHEcom.Data.Repositories;
using BHEcom.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BHEcom.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;
        private readonly ICartItemService _cartItemService;
        private readonly ILogger<CartRepository> _logger;
        public CartController(ICartService cartService, ICartItemService cartItemService, ILogger<CartRepository> logger)
        {
            _cartService = cartService;
            _cartItemService = cartItemService;
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
        [HttpPost]
        [Route("CreateOrUpdate")]
        public async Task<IActionResult> CreateOrUpdate([FromBody] CartManager cartManager)
        {
            if (cartManager == null)
                return BadRequest("Invalid data.");

            Cart cart = new Cart();
            Guid cartId =  Guid.Empty;
            bool IsUpdated = false;
            CartItem cartItem = new CartItem();
            Guid cartItemId = Guid.Empty;
            bool IsItemUpdated = false;

            if (cartManager.CartID != Guid.Empty)
            {
                cart = await _cartService.GetCartByIdAsync(cartManager.CartID);
            }
           
            if (cartManager.CartID != Guid.Empty)
            {
                cartItem = await _cartItemService.GetCartItemByIdAsync(cartManager.CartItemID);
            }
            if (cart == null) // Create Cart
            {
                cart = new Cart
                {
                    UserID = cartManager.UserID,
                    CreatedDate = DateTime.UtcNow
                };
               cartId =  await _cartService.AddCartAsync(cart);
            }
            else // Update Cart
            {
                cart.UserID = cartManager.UserID;
                IsUpdated = await _cartService.UpdateCartAsync(cart);
            }
          
            
            if (cartItem == null) // Create CartItem
            {
                cartItem = new CartItem
                {
                    CartID = cart.CartID,
                    ProductID = cartManager.ProductID,
                    Quantity = cartManager.Quantity
                };
               cartId =  await _cartItemService.AddCartItemAsync(cartItem);
            }
            else // Update CartItem
            {
                cartItem.ProductID = cartManager.ProductID;
                cartItem.Quantity = cartManager.Quantity;
                IsItemUpdated = await _cartItemService.UpdateCartItemAsync(cartItem);
            }

            return Ok(new
            {
                Message = "Cart and CartItem created/updated successfully.",
                CartID = cart.CartID,
                CartItemID = cartItem.CartItemID
            });
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

        [HttpDelete("DeleteCart/{cartId}")]
        public async Task<IActionResult> DeleteCart(Guid cartItemId, Guid CartId)
        {
            try
            {
                bool IsCartitemDeleted = await _cartItemService.DeleteCartItemAsync(cartItemId);
                bool IsCartDeleted = await _cartService.DeleteCartAsync(CartId);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting a cart.");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("Index/{userId}")]
        public async Task<IActionResult> Index(Guid userId)
        {
            try
            {
                var cartItems = await _cartService.GetCartItemsByUserIdAsync(userId);
                return Ok(cartItems);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while Getting a cartManager.");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("ClearCart/{id}")]
        public async Task<IActionResult> ClearCart(Guid id)
        {
            try
            {
                await _cartService.ClearCartAsync(id);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while Deleting a cartManager.");
                return StatusCode(500, ex.Message);
            }
        }

    }

}
