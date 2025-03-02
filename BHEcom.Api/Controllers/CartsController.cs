using BHEcom.Common.Models;
using BHEcom.Data.Repositories;
using BHEcom.Services.Implementations;
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
        private readonly IProductService _productService;
        private readonly ILogger<CartRepository> _logger;
        public CartController(ICartService cartService, ICartItemService cartItemService,IProductService productService, ILogger<CartRepository> logger)
        {
            _cartService = cartService;
            _cartItemService = cartItemService;
            _productService = productService;
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
                return StatusCode(500, new { Message = ex.Message, Success = false });
            }
        }
        [HttpPost]
        [Route("CreateOrUpdate")]
        public async Task<IActionResult> CreateOrUpdate([FromBody] CartManager cartManager)
        {
            try
            {
                if (cartManager == null)
                    return BadRequest("Invalid data.");

                Cart cart = new Cart();
                Guid cartId = Guid.Empty;
                bool IsUpdated = false;
                CartItem cartItem = new CartItem();
                Guid cartItemId = Guid.Empty;
                bool IsItemUpdated = false;

                var product = await _productService.GetProductByIdAsync(cartManager.ProductID);
                if (product.Stock < cartManager.Quantity)
                {
                    return Ok(new
                    {
                        Message = $"There are only {product.Stock} product available!",
                        Success = false
                    });
                }


                if (cartManager.CartID != Guid.Empty)
                {
                    cart = await _cartService.GetCartByIdAsync(cartManager.CartID);
                }
                else if(cartManager.UserID != Guid.Empty)
                {
                    cart = await _cartService.GetCartByuserIdAsync(cartManager.UserID);
                }

                if (cartManager.CartItemID != Guid.Empty)
                {
                    cartItem = await _cartItemService.GetCartItemByIdAsync(cartManager.CartItemID);
                }
                else if (cart != null && cart.CartID != Guid.Empty )
                {
                    cartItem = await _cartItemService.GetCartItemByCartandProductIdAsync(cart.CartID, cartManager.ProductID);
                    
                }
                if (cartItem != null)
                {
                    if (product.Stock < (cartManager.Quantity + cartItem.Quantity))
                    {
                        return Ok(new
                        {
                            Message = $"There are only {product.Stock} product available!",
                            Success = false
                        });
                    }
                }

                if (cart == null || cart.CartID == Guid.Empty) // Create Cart
                {
                    cart = new Cart
                    {
                        UserID = cartManager.UserID,
                        CreatedDate = DateTime.UtcNow
                    };
                    cartId = await _cartService.AddCartAsync(cart);
                    if (cartId == Guid.Empty) 
                    {
                        return BadRequest(new
                        {
                            Message = "Cart Create Failed!",
                            Success = false
                        });
                    }

                }
                else // Update Cart
                {
                    cart.UserID = cartManager.UserID;
                    IsUpdated = await _cartService.UpdateCartAsync(cart);
                    if (!IsUpdated)
                    {
                        return BadRequest(new
                        {
                            Message = "Cart Update Failed!",
                            Success = false
                        });
                    }

                }

                if (cartItem == null || cartItem.CartItemID == Guid.Empty ) // Create CartItem
                {
                    cartItem = new CartItem
                    {
                        CartID = cart.CartID,
                        ProductID = cartManager.ProductID,
                        Quantity = cartManager.Quantity
                    };
                    cartItemId = await _cartItemService.AddCartItemAsync(cartItem);
                    if (cartItemId == Guid.Empty)
                    {
                        return BadRequest(new
                        {
                            Message = "CartItem Create Failed!",
                            Success = false
                        });
                    }
                }
                else // Update CartItem
                {
                    cartItem.ProductID = cartManager.ProductID;
                    cartItem.Quantity = cartItem.Quantity + cartManager.Quantity;
                    IsItemUpdated = await _cartItemService.UpdateCartItemAsync(cartItem);
                    if (!IsItemUpdated)
                    {
                        return BadRequest(new
                        {
                            Message = "CartItem Create Failed!",
                            Success = false
                        });
                    }
                }
               
                return Ok(new
                {
                    Message = "Cart and CartItem created/updated successfully.",
                    CartID = cart.CartID,
                    CartItemID = cartItem.CartItemID,
                    Success = true
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding a Cart and CartItem.");
                return StatusCode(500, new { Message = ex.Message, Success = false });
            }
        }


        [HttpPost]
        [Route("MultipleCreateOrUpdate/{id}")]
        public async Task<IActionResult> MultipleCreateOrUpdate(Guid id, [FromBody] List<CartManager> cartManagerList)
        {
            try
            {

                if (cartManagerList == null || !cartManagerList.Any())
                    return BadRequest("Invalid data.");


                Cart cart = new Cart();

                if (id != Guid.Empty)
                {
                    cart = await _cartService.GetCartByuserIdAsync(id);
                }

                foreach (var cartManager in cartManagerList) {

                    if (cartManager == null)
                        return BadRequest("Invalid data.");

                 
                    Guid cartId = Guid.Empty;
                    bool IsUpdated = false;
                    CartItem cartItem = new CartItem();
                    Guid cartItemId = Guid.Empty;
                    bool IsItemUpdated = false;

                    var product = await _productService.GetProductByIdAsync(cartManager.ProductID);
                    if (product.Stock < cartManager.Quantity)
                    {
                        return Ok(new
                        {
                            Message = $"There are only {product.Stock} product available!",
                            Success = false
                        });
                    }


                    if (cartManager.CartItemID != Guid.Empty)
                    {
                        cartItem = await _cartItemService.GetCartItemByIdAsync(cartManager.CartItemID);
                    }
                    else if (cart != null && cart.CartID != Guid.Empty)
                    {
                        cartItem = await _cartItemService.GetCartItemByCartandProductIdAsync(cart.CartID, cartManager.ProductID);
                       
                    }
                    if (cartItem != null)
                    {
                        if (product.Stock < (cartManager.Quantity + cartItem.Quantity))
                        {
                            return Ok(new
                            {
                                Message = $"There are only {product.Stock} product available!",
                                Success = false
                            });
                        }
                    }

                    if (cart == null || cart.CartID == Guid.Empty) // Create Cart
                    {
                        cart = new Cart
                        {
                            UserID = cartManager.UserID,
                            CreatedDate = DateTime.UtcNow
                        };
                        cartId = await _cartService.AddCartAsync(cart);
                        if (cartId == Guid.Empty)
                        {
                            return BadRequest(new
                            {
                                Message = "Cart Create Failed!",
                                Success = false
                            });
                        }

                    }
                    else // Update Cart
                    {
                        cart.UserID = cartManager.UserID;
                        IsUpdated = await _cartService.UpdateCartAsync(cart);
                        if (!IsUpdated)
                        {
                            return BadRequest(new
                            {
                                Message = "Cart Update Failed!",
                                Success = false
                            });
                        }

                    }

                    if (cartItem == null || cartItem.CartItemID == Guid.Empty) // Create CartItem
                    {
                        cartItem = new CartItem
                        {
                            CartID = cart.CartID,
                            ProductID = cartManager.ProductID,
                            Quantity = cartManager.Quantity
                        };
                        cartItemId = await _cartItemService.AddCartItemAsync(cartItem);
                        if (cartItemId == Guid.Empty)
                        {
                            return BadRequest(new
                            {
                                Message = "CartItem Create Failed!",
                                Success = false
                            });
                        }
                    }
                    else // Update CartItem
                    {
                        cartItem.ProductID = cartManager.ProductID;
                        cartItem.Quantity = cartItem.Quantity + cartManager.Quantity;
                        IsItemUpdated = await _cartItemService.UpdateCartItemAsync(cartItem);
                        if (!IsItemUpdated)
                        {
                            return BadRequest(new
                            {
                                Message = "CartItem Create Failed!",
                                Success = false
                            });
                        }
                    }

                }
                return Ok(new
                {
                    Message = "Cart and CartItem created/updated successfully.",
                    CartID = cart.CartID,
                    Success = true
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding a Cart and CartItem.");
                return StatusCode(500, new { Message = ex.Message, Success = false });
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
                    return Ok(new { data = cart, Message = "Not found!", Success = true });
                }
                //return Ok(cart);
                return Ok(new { data = cart, Success = true });
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "An error occurred while getting a cart.");
                return StatusCode(500, new { Message = ex.Message, Success = false });
            }
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<Cart>>> GetAll()
        {
            try
            {
                var carts = await _cartService.GetAllCartsAsync();
                if (carts == null)
                         return Ok(new { data = carts, Message = "No data available!", Success = true });
                return Ok(new { data = carts, Success = true });
               // return Ok(carts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting all cart.");
                return StatusCode(500, new { Message = ex.Message, Success = false });
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
                return Ok(new { Message = "Succfully updated", Success = true });
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "An error occurred while updating a cart.");
                return StatusCode(500, new { Message = ex.Message, Success = false });
            }
        }

        [HttpDelete("Delete/{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            try
            {
                await _cartService.DeleteCartAsync(id);
                return Ok(new { Message = "Succfully Delete", Success = true });
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "An error occurred while deleting a cart.");
                return StatusCode(500, new { Message = ex.Message, Success = false });
            }
        }

        [HttpDelete("DeleteCart/{cartId}/{cartItemId}")]
        public async Task<IActionResult> DeleteCart(Guid cartItemId, Guid CartId)
        {
            try
            {
                bool IsCartitemDeleted = await _cartItemService.DeleteCartItemAsync(cartItemId);
                bool IsCartDeleted = await _cartService.DeleteCartAsync(CartId);
                return Ok(new { Message = "Succfully Delete", Success = true });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting a cart.");
                return StatusCode(500, new { Message = ex.Message, Success = false });
            }
        }

        [HttpGet("Index/{userId}")]
        public async Task<IActionResult> UserCartItems(Guid userId)
        {
            try
            {
                var cartItems = await _cartService.GetCartItemsByUserIdAsync(userId);
                return Ok(new { data = cartItems, Success = true });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while Getting a cartManager.");
                return StatusCode(500, new { Message = ex.Message, Success = false });
            }
        }
        
        [HttpGet("CheckProductAvailability/{cartId}")]
        public async Task<IActionResult> CheckProductAvailability(Guid cartId)
        {
            try
            {
                var cartItems = await _cartService.GetCartManagerByCartIdAsync(cartId);
                var unavailableProductList = new List<object>();

                if (cartItems == null || !cartItems.Any()) { 
                    return NotFound(new
                    {
                        Success = false,
                        Message = "CardItem unavailable!"
                    });
                }
                foreach (var item in cartItems)
                {
                    if (item.ProductID == Guid.Empty)
                        return NotFound(new
                        {
                            Success = false,
                            Message = $"{item.ProductName} not exist!"
                        });

                    var product = await _productService.GetProductByIdAsync(item.ProductID);

                    if (product.Stock < item.Quantity)
                    {
                        var unavailableProduct = new
                        {
                            ProductId = product.ProductID,
                            CartItemId = item.CartItemID,
                            Stock = product.Stock
                        };

                        unavailableProductList.Add(unavailableProduct);
                        continue; // Skip adding this product to the order
                    }

                }

                // Check if there are any unavailable products
                if (unavailableProductList.Any())
                {
                    return Ok(new
                    {
                        Success = false,
                        Message = "Some products are unavailable.",
                        UnavailableProducts = unavailableProductList
                    });
                }

                return Ok(new
                {
                    Success = true,
                    Message = "All Product are available."
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while Getting a cartManager.");
                return StatusCode(500, new { Message = ex.Message, Success = false });
            }
        }

        [HttpPost("ClearCart/{id}")]
        public async Task<IActionResult> ClearCart(Guid id)
        {
            try
            {
                await _cartService.ClearCartAsync(id);
                return Ok(new { Message = "Succfully Clean Cart", Success = true });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while Deleting a cartManager.");
                return StatusCode(500, new { Message = ex.Message, Success = false });
            }
        }
         [HttpPost("DeleteCart/{id}")]
        public async Task<IActionResult> DeleteCart(Guid id)
        {
            try
            {
               bool isDeleted =  await _cartService.DeleteFullCartAsync(id);
                if (!isDeleted)
                    return StatusCode(500, new { message = "Internal Server Error", success = false });
                return Ok(new
                {
                    Message = "Cart and CartItem Delete successfully.",
                    Success = true
                });

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while Deleting a cartManager.");
                return StatusCode(500, new { Message = ex.Message, Success = false });
            }
        }

       
    }

}
