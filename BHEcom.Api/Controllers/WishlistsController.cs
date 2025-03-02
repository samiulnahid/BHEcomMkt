using BHEcom.Common.Models;
using BHEcom.Data.Repositories;
using BHEcom.Services.Implementations;
using BHEcom.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BHEcom.Api.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class WishlistsController : ControllerBase
    {
        private readonly IWishlistService _wishlistService;
        private readonly ILogger<WishlistRepository> _logger;
        private readonly IProductService _productService;

        public WishlistsController(IWishlistService wishlistService, ILogger<WishlistRepository> logger, IProductService productService)
        {
            _wishlistService = wishlistService;
            _logger = logger;
            _productService = productService;
        }

        [HttpPost("Create")]
        public async Task<ActionResult> Create([FromBody] Wishlist wishlist)
        {
            try
            {
                await _wishlistService.AddWishlistAsync(wishlist);
                return CreatedAtAction(nameof(GetById), new { id = wishlist.WishlistID }, wishlist);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding a wishlistItem.");
                return StatusCode(500, new { Message = ex.Message, Success = false });
            }
        }

        //[HttpGet("GetById/{id}")]
        //public async Task<ActionResult<Wishlist>> GetById(Guid id)
        //{
        //    try
        //    {
        //        var wishlist = await _wishlistService.GetWishlistByIdAsync(id);
        //        if (wishlist == null)
        //        {
        //            return NotFound();
        //        }
        //        return Ok(wishlist);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "An error occurred while getById a wishlistItem.");
        //        return StatusCode(500, new { Message = ex.Message, Success = false });
        //    }
        //}

        //[HttpGet("GetAll")]
        //public async Task<ActionResult<IEnumerable<Wishlist>>> GetAll()
        //{
        //    try
        //    {
        //        var wishlists = await _wishlistService.GetAllWishlistsAsync();
        //        return Ok(wishlists);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "An error occurred while get all wishlistItem.");
        //        return StatusCode(500, new { Message = ex.Message, Success = false });
        //    }
        //}

        [HttpPut("Update/{id}")]
        public async Task<ActionResult> Update(Guid id, [FromBody] Wishlist wishlist)
        {
            try
            {
                if (id != wishlist.WishlistID)
                {
                    return BadRequest();
                }
                await _wishlistService.UpdateWishlistAsync(wishlist);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating a wishlistItem.");
                return StatusCode(500, new { Message = ex.Message, Success = false });
            }
        }

        
        [HttpPost]
        [Route("CreateOrUpdate")]
        public async Task<IActionResult> CreateOrUpdate([FromBody] WishlistManager wishlistManager)
        {
            try
            {
                if (wishlistManager == null)
                    return BadRequest("Invalid data.");

                Wishlist wishlist = new Wishlist();
                Guid wishlistId = Guid.Empty;
                bool isUpdated = false;
                WishlistItem wishlistItem = new WishlistItem();
                Guid wishlistItemId = Guid.Empty;
                bool isItemUpdated = false;

                var product = await _productService.GetProductByIdAsync(wishlistManager.ProductID);
                if (product.Stock < wishlistManager.Quantity )
                {
                    return Ok(new
                    {
                        Message = $"{product.ProductName} has only {product.Stock} product available!",
                        ProductId = product.ProductID,
                        Success = false
                    });
                }
                if (product == null)
                {
                    return BadRequest(new
                    {
                        Message = "Product not found.",
                        Success = false
                    });
                }

                if (wishlistManager.WishlistID != Guid.Empty)
                {
                    wishlist = await _wishlistService.GetWishlistByIdAsync(wishlistManager.WishlistID);
                }
                else if (wishlistManager.UserID != Guid.Empty)
                {
                    wishlist = await _wishlistService.GetWishlistByUserIdAsync(wishlistManager.UserID);
                }

                if (wishlistManager.WishlistItemID != Guid.Empty)
                {
                    wishlistItem = await _wishlistService.GetWishlistItemByIdAsync(wishlistManager.WishlistItemID);
                }
                else if (wishlist != null && wishlist.WishlistID != Guid.Empty)
                {
                    wishlistItem = await _wishlistService.GetWishlistItemByWishlistAndProductIdAsync(wishlist.WishlistID, wishlistManager.ProductID);
                }

                if (wishlistItem != null)
                {
                    if (product.Stock < wishlistManager.Quantity + wishlistItem.Quantity)
                    {
                        return Ok(new
                        {
                            Message = $"{product.ProductName} has only {product.Stock} product available!",
                            ProductId = product.ProductID,
                            Success = false
                        });
                    }
                }

                if (wishlist == null || wishlist.WishlistID == Guid.Empty) // Create Wishlist
                {
                    wishlist = new Wishlist
                    {
                        UserID = wishlistManager.UserID,
                        CreatedDate = DateTime.UtcNow
                    };
                    wishlistId = await _wishlistService.AddWishlistAsync(wishlist);
                    if (wishlistId == Guid.Empty)
                    {
                        return BadRequest(new
                        {
                            Message = "Wishlist creation failed!",
                            Success = false
                        });
                    }
                }
                else // Update Wishlist
                {
                    wishlist.UserID = wishlistManager.UserID;
                    isUpdated = await _wishlistService.UpdateWishlistAsync(wishlist);
                    if (!isUpdated)
                    {
                        return BadRequest(new
                        {
                            Message = "Wishlist update failed!",
                            Success = false
                        });
                    }
                }

                if (wishlistItem == null || wishlistItem.WishlistItemID == Guid.Empty) // Create WishlistItem
                {
                    wishlistItem = new WishlistItem
                    {
                        WishlistID = wishlist.WishlistID,
                        ProductID = wishlistManager.ProductID,
                        Quantity = wishlistManager.Quantity,
                        AddedDate = DateTime.Now
                    };
                    wishlistItemId = await _wishlistService.AddWishlistItemAsync(wishlistItem);
                    if (wishlistItemId == Guid.Empty)
                    {
                        return BadRequest(new
                        {
                            Message = "WishlistItem creation failed!",
                            Success = false
                        });
                    }
                }
                else // Update WishlistItem
                {
                    wishlistItem.ProductID = wishlistManager.ProductID;
                    wishlistItem.Quantity = wishlistItem.Quantity + wishlistManager.Quantity;
                    isItemUpdated = await _wishlistService.UpdateWishlistItemAsync(wishlistItem);
                    if (!isItemUpdated)
                    {
                        return BadRequest(new
                        {
                            Message = "WishlistItem update failed!",
                            Success = false
                        });
                    }
                }

                return Ok(new
                {
                    Message = "Wishlist and WishlistItem created/updated successfully.",
                    WishlistID = wishlist.WishlistID,
                    WishlistItemID = wishlistItem.WishlistItemID,
                    Success = true
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding/updating a Wishlist and WishlistItem.");
                return StatusCode(500, new { Message = ex.Message, Success = false });
            }
        }

        [HttpGet("GetById/{id}")]
        public async Task<ActionResult<Wishlist>> GetById(Guid id)
        {
            try
            {
                var wishlist = await _wishlistService.GetListWithItemsByIdAsync(id);
                if (wishlist == null)
                {
                    return Ok(new { data = wishlist, Message = "Not found!", Success = true });
                }
                return Ok(new { data = wishlist, Success = true });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting a wishlist.");
                return StatusCode(500, new { Message = ex.Message, Success = false });
            }
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<Wishlist>>> GetAll()
        {
            try
            {
                var wishlists = await _wishlistService.GetAllWishlistsAsync();
                if (wishlists == null)
                    return Ok(new { data = wishlists, Message = "No data available!", Success = true });
                return Ok(new { data = wishlists, Success = true });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting all wishlists.");
                return StatusCode(500, new { Message = ex.Message, Success = false });
            }
        }

        [HttpDelete("ClearWishlist/{id}")]
        public async Task<IActionResult> ClearWishlist(Guid id)
        {
            try
            {
                await _wishlistService.ClearWishlistAsync(id);
                return Ok(new { Message = "Successfully cleared wishlist.", Success = true });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while clearing a wishlist.");
                return StatusCode(500, new { Message = ex.Message, Success = false });
            }
        }

        [HttpDelete("DeleteWishlist/{id}")]
        public async Task<IActionResult> DeleteWishlist(Guid id)
        {
            try
            {
                bool isDeleted = await _wishlistService.DeleteWishlistAsync(id);
                if (!isDeleted)
                    return StatusCode(500, new { message = "Internal Server Error", success = false });
                return Ok(new
                {
                    Message = "Wishlist deleted successfully.",
                    Success = true
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting a wishlist.");
                return StatusCode(500, new { Message = ex.Message, Success = false });
            }
        }

        [HttpDelete("DeleteFullWishlist/{id}")]
        public async Task<IActionResult> DeleteFullWishlist(Guid id)
        {
            try
            {
                bool isDeleted = await _wishlistService.DeleteFullWishlistAsync(id);
                if (!isDeleted)
                    return StatusCode(500, new { message = "Internal Server Error", success = false });
                return Ok(new
                {
                    Message = "Wishlist and WishlistItems deleted successfully.",
                    Success = true
                });

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while Deleting a cartManager.");
                return StatusCode(500, new { Message = ex.Message, Success = false });
            }
        }
        [HttpDelete("DeleteWishlistByUserId/{id}")]
        public async Task<ActionResult> DeleteWishlistByUserId(Guid id)
        {
            try
            {
                bool isDeleted = await _wishlistService.DeleteWishlistByUserId(id);
                if (!isDeleted)
                    return StatusCode(500, new { message = "Internal Server Error", success = false });
                return Ok(new
                {
                    Message = "Wishlist and WishlistItems deleted successfully.",
                    Success = true
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting a wishlistItem.");
                return StatusCode(500, new { Message = ex.Message, Success = false });
            }
        }

        [HttpDelete("DeleteWishlistItem/{id}")]
        public async Task<IActionResult> DeleteWishlistItem(Guid id)
        {
            try
            {
               
                bool isDeleted = await _wishlistService.DeleteWishlistItemAsync(id);
                if (!isDeleted)
                    return StatusCode(500, new { message = "Delete unsuccessful!", success = false });
                return Ok(new
                {
                    Message = "Wishlist Item Delete successfully.",
                    Success = true
                });
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "An error occurred while deleting a Wishlist Item.");
                return StatusCode(500, new { Message = ex.Message, Success = false });

            }
        }

        [HttpPost]
        [Route("MultipleCreateOrUpdate/{id}")]
        public async Task<IActionResult> MultipleCreateOrUpdate(Guid id, [FromBody] List<WishlistManager> WishlistManagerList)
        {
            try
            {
                if (WishlistManagerList == null || !WishlistManagerList.Any())
                    return BadRequest("Invalid data.");

                Wishlist wishlist = new Wishlist();
                if (id != Guid.Empty)
                {
                    wishlist = await _wishlistService.GetWishlistByUserIdAsync(id);
                }

                foreach (var wishlistManager in WishlistManagerList)
                {
                    if (wishlistManager == null)
                        return BadRequest("Invalid data.");

                 
                    Guid wishlistId = Guid.Empty;
                    bool isUpdated = false;
                    WishlistItem wishlistItem = new WishlistItem();
                    Guid wishlistItemId = Guid.Empty;
                    bool isItemUpdated = false;

                    var product = await _productService.GetProductByIdAsync(wishlistManager.ProductID);
                    if (product == null)
                    {
                        return BadRequest(new
                        {
                            Message = "Product not found.",
                            Success = false
                        });
                    }
                    if (product.Stock < wishlistManager.Quantity)
                    {
                        return Ok(new
                        {
                            Message = $"{product.ProductName} has only {product.Stock} product available!",
                            ProductId= product.ProductID,
                            Success = false
                        });
                    }


                    if (wishlistManager.WishlistItemID != Guid.Empty)
                    {
                        wishlistItem = await _wishlistService.GetWishlistItemByIdAsync(wishlistManager.WishlistItemID);
                    }
                    else if (wishlist != null && wishlist.WishlistID != Guid.Empty)
                    {
                        wishlistItem = await _wishlistService.GetWishlistItemByWishlistAndProductIdAsync(wishlist.WishlistID, wishlistManager.ProductID);
                    }
                    if (wishlistItem != null)
                    {
                        if (product.Stock < wishlistManager.Quantity + wishlistItem.Quantity)
                        {
                            return Ok(new
                            {
                                Message = $"{product.ProductName} has only {product.Stock} product available!",
                                ProductId = product.ProductID,
                                Success = false
                            });
                        }
                    }

                    if (wishlist == null || wishlist.WishlistID == Guid.Empty) // Create Wishlist
                    {
                        wishlist = new Wishlist
                        {
                            UserID = wishlistManager.UserID,
                            CreatedDate = DateTime.UtcNow
                        };
                        wishlistId = await _wishlistService.AddWishlistAsync(wishlist);
                        if (wishlistId == Guid.Empty)
                        {
                            return BadRequest(new
                            {
                                Message = "Wishlist creation failed!",
                                Success = false
                            });
                        }
                    }
                    else // Update Wishlist
                    {
                        wishlist.UserID = wishlistManager.UserID;
                        isUpdated = await _wishlistService.UpdateWishlistAsync(wishlist);
                        if (!isUpdated)
                        {
                            return BadRequest(new
                            {
                                Message = "Wishlist update failed!",
                                Success = false
                            });
                        }
                    }

                    if (wishlistItem == null || wishlistItem.WishlistItemID == Guid.Empty) // Create WishlistItem
                    {
                        wishlistItem = new WishlistItem
                        {
                            WishlistID = wishlist.WishlistID,
                            ProductID = wishlistManager.ProductID,
                            Quantity = wishlistManager.Quantity,
                            AddedDate = DateTime.Now
                        };
                        wishlistItemId = await _wishlistService.AddWishlistItemAsync(wishlistItem);
                        if (wishlistItemId == Guid.Empty)
                        {
                            return BadRequest(new
                            {
                                Message = "WishlistItem creation failed!",
                                Success = false
                            });
                        }
                    }
                    else // Update WishlistItem
                    {
                        wishlistItem.ProductID = wishlistManager.ProductID;
                        wishlistItem.Quantity = wishlistItem.Quantity + wishlistManager.Quantity;
                        isItemUpdated = await _wishlistService.UpdateWishlistItemAsync(wishlistItem);
                        if (!isItemUpdated)
                        {
                            return BadRequest(new
                            {
                                Message = "WishlistItem update failed!",
                                Success = false
                            });
                        }
                    }

                  
                }
                return Ok(new
                {
                    Message = "Wishlist and WishlistItem created/updated successfully.",
                    WishlistID = wishlist.WishlistID,
                    Success = true
                });

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding/updating a Wishlist and WishlistItem.");
                return StatusCode(500, new { Message = ex.Message, Success = false });
            }
        }

        [HttpGet("GetByUserId/{userId}")]
        public async Task<IActionResult> GetByUserId(Guid userId)
        {
            try
            {
                var data = await _wishlistService.GetByUserIdAsync(userId);
                return Ok(new { data = data, Success = true });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while Getting a Wishlist.");
                return StatusCode(500, new { Message = ex.Message, Success = false });
            }
        }
    }
}
