using BHEcom.Common.Models;
using BHEcom.Data.Repositories;
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
        public WishlistsController(IWishlistService wishlistService, ILogger<WishlistRepository> logger)
        {
            _wishlistService = wishlistService;
            _logger = logger;
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
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("GetById/{id}")]
        public async Task<ActionResult<Wishlist>> GetById(Guid id)
        {
            try
            {
                var wishlist = await _wishlistService.GetWishlistByIdAsync(id);
                if (wishlist == null)
                {
                    return NotFound();
                }
                return Ok(wishlist);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getById a wishlistItem.");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<Wishlist>>> GetAll()
        {
            try
            {
                var wishlists = await _wishlistService.GetAllWishlistsAsync();
                return Ok(wishlists);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while get all wishlistItem.");
                return StatusCode(500, ex.Message);
            }
        }

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
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("Delete/{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            try
            {
                await _wishlistService.DeleteWishlistAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting a wishlistItem.");
                return StatusCode(500, ex.Message);
            }
        }
    }
}
