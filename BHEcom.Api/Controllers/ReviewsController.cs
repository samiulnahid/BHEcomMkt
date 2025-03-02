using BHEcom.Common.Models;
using BHEcom.Data.Repositories;
using BHEcom.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BHEcom.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReviewsController : ControllerBase
    {
        private readonly IReviewService _reviewService;
        private readonly ILogger<ReviewRepository> _logger;
        public ReviewsController(IReviewService reviewService, ILogger<ReviewRepository> logger)
        {
            _reviewService = reviewService;
            _logger = logger;
        }

        [HttpPost("Create")]
        public async Task<ActionResult> Create([FromBody] Review review)
        {
            try
            {
                await _reviewService.AddReviewAsync(review);
                return CreatedAtAction(nameof(GetById), new { id = review.ReviewID }, review);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding a review.");
                return StatusCode(500, new { Message = ex.Message, Success = false });
            }
        }

        [HttpGet("GetById/{id}")]
        public async Task<ActionResult<Review>> GetById(Guid id)
        {
            try
            {
                var review = await _reviewService.GetReviewByIdAsync(id);
                if (review == null)
                {
                    return NotFound();
                }
                return Ok(review);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting a review.");
                return StatusCode(500, new { Message = ex.Message, Success = false });
            }
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<Review>>> GetAll()
        {
            try
            {
                var reviews = await _reviewService.GetAllReviewsAsync();
                return Ok(reviews);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting all review.");
                return StatusCode(500, new { Message = ex.Message, Success = false });
            }
        }

        [HttpPut("Update/{id}")]
        public async Task<ActionResult> Update(Guid id, [FromBody] Review review)
        {
            try
            {
                if (id != review.ReviewID)
                {
                    return BadRequest();
                }

                await _reviewService.UpdateReviewAsync(review);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating a review.");
                return StatusCode(500, new { Message = ex.Message, Success = false });
            }
        }

        [HttpDelete("Delete/{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            try
            {
                await _reviewService.DeleteReviewAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "An error occurred while deleting a review.");
                return StatusCode(500, new { Message = ex.Message, Success = false });
            }
        }
    }
}
