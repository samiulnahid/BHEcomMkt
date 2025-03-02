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
                return Ok(new { id = productDetail.DetailID, Success = true });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding a productDetail.");
                return StatusCode(500, new { Message = ex.Message, Success = false });
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
                    return Ok(new { data = productDetail, Message = "Not Found!", Success = true });
                }
                return Ok(new { data = productDetail, Success = true });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting a productDetail.");
                return StatusCode(500, new { Message = ex.Message, Success = false });
            }
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<ProductDetail>>> GetAll()
        {
            try
            {
                var productDetails = await _productDetailService.GetAllProductDetailsAsync();
                return Ok(new { data = productDetails, Success = true });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting all productDetail.");
                return StatusCode(500, new { Message = ex.Message, Success = false });
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
                 
                bool result = await _productDetailService.UpdateProductDetailAsync(productDetail);
                if (!result)
                {
                    return NotFound($"ProductDetail with ID {productDetail.DetailID} not found.");
                }

                return Ok(new { Message = "Successfully Updated", Success = true });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating a productDetail.");
                return StatusCode(500, new { Message = ex.Message, Success = false });
            }
        }

        [HttpDelete("Delete/{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            try
            {
                await _productDetailService.DeleteProductDetailAsync(id);
                return Ok(new { Message = "Successfully Deleted", Success = true });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting a productDetail.");
                return StatusCode(500, new { Message = ex.Message, Success = false });
            }
        }
    }
}
