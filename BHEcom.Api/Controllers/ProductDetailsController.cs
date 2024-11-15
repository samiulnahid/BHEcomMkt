using BHEcom.Common.Models;
using BHEcom.Data.Repositories;
using BHEcom.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BHEcom.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductDetailsController : ControllerBase
    {
        private readonly IProductDetailService _productDetailService;
        private readonly ILogger<ProductDetailRepository> _logger;
        public ProductDetailsController(IProductDetailService productDetailService, ILogger<ProductDetailRepository> logger)
        {
            _productDetailService = productDetailService;
            _logger = logger;
        }

        [HttpPost("Create")]
        public async Task<ActionResult> Create([FromBody] ProductDetail productDetail)
        {
            try
            {
                await _productDetailService.AddProductDetailAsync(productDetail);
                return CreatedAtAction(nameof(GetById), new { id = productDetail.DetailID }, productDetail);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding a productDetail.");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("GetById/{id}")]
        public async Task<ActionResult<ProductDetail>> GetById(Guid id)
        {
            try
            {
                var productDetail = await _productDetailService.GetProductDetailByIdAsync(id);
                if (productDetail == null)
                {
                    return NotFound();
                }
                return Ok(productDetail);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting a productDetail.");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<ProductDetail>>> GetAll()
        {
            try
            {
                var productDetails = await _productDetailService.GetAllProductDetailsAsync();
                return Ok(productDetails);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting all productDetail.");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("Update/{id}")]
        public async Task<ActionResult> Update(Guid id, [FromBody] ProductDetail productDetail)
        {
            try
            {
                if (id != productDetail.DetailID)
                {
                    return BadRequest();
                }
                await _productDetailService.UpdateProductDetailAsync(productDetail);
                return Ok("Successfully Updated");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating a productDetail.");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("Delete/{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            try
            {
                await _productDetailService.DeleteProductDetailAsync(id);
                return Ok("Successfully Deleted");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting a productDetail.");
                return StatusCode(500, ex.Message);
            }
        }
    }
}
