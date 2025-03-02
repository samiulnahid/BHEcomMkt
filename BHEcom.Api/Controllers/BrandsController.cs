using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using BHEcom.Common.Models;
using BHEcom.Services;
using BHEcom.Services.Interfaces;
using BHEcom.Data.Repositories;
using BHEcom.Common.Helper;
using Microsoft.Extensions.Logging;

namespace BHEcom.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandsController : ControllerBase
    {
        private readonly IBrandService _brandService;
        private readonly ILogger<BrandRepository> _logger;
        private readonly FtpUploader _ftpUploader;
        public BrandsController(IBrandService brandService, ILogger<BrandRepository> logger, FtpUploader ftpUploader)
        {
            _brandService = brandService;
            _logger = logger;
            _ftpUploader = ftpUploader;
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<Brand>>> GetAll()
        {
            try
            {
                var brands = await _brandService.GetAllBrandsAsync();
                return Ok(new { data = brands, Success = true });

            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "An error occurred while adding a brand.");
                return StatusCode(500, new { Message = ex.Message, Success = false });
            }
        }

        [HttpGet("GetById/{id}")]
        public async Task<ActionResult<Brand>> GetById(Guid id)
        {
            try
            {
                var brand = await _brandService.GetBrandByIdAsync(id);
                if (brand == null)
                {
                    return Ok(new { data = brand,Message = "Brand Not Found!", Success = true });
                }
                return Ok(new { data = brand, Success = true });
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "An error occurred while getting a brand.");
                return StatusCode(500, new { Message = ex.Message, Success = false });
            }
        }

        [HttpPost("Create")]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult> Create([FromForm] Brand brand, [FromForm] IFormFile imageFile)
        {
            try
            {
                if (imageFile != null && imageFile.Length > 0)
                {
                    string folderName = "ecom/brand"; // Adjust based on the product folder
                    string imageUrl = await _ftpUploader.UploadFileAsync(imageFile, folderName);

                    // Assign the uploaded image URL to the product
                    brand.Image = imageUrl;
                    brand.CreatedDate = DateTime.Now;
                    brand.IsActive = true;
                }
                var (brandId, isUnique) = await _brandService.AddBrandAsync(brand);

                if (!isUnique)
                {
                    return StatusCode(500, new { Message = "Brand Name already exist!", Success = false });
                }
                return Ok(new { id = brandId, Success = true });

            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "An error occurred while getting all brand.");
                return StatusCode(500, new { Message = ex.Message, Success = false });
            }
        }

        [HttpPut("Update/{id}")]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult> Update(Guid id, [FromForm] Brand brand, [FromForm] IFormFile imageFile)
        {
            try
            {

                if (id != brand.BrandID)
                {
                    return BadRequest();
                }
                if (imageFile != null && imageFile.Length > 0)
                {
                    string folderName = "ecom/brand"; // Adjust based on the product folder
                    string imageUrl = await _ftpUploader.UploadFileAsync(imageFile, folderName);

                    // Assign the uploaded image URL to the product
                    brand.Image = imageUrl;
                    brand.ModifiedBy = brand.ModifiedBy;
                    brand.ModifiedDate = DateTime.Now;
                }

                var (isUpdated, oldImageUrl, isUniqueName) = await _brandService.UpdateBrandAsync(brand);

                if (!isUpdated)
                {
                    if (!isUniqueName)
                    {
                        return StatusCode(500, new { Message = "Brand Name already exist!", Success = false });
                    }
                    return StatusCode(500, new { Message = "Unsuccessfully Updated", Success = false });
                }

                if (!string.IsNullOrEmpty(oldImageUrl))
                {
                    // Image delete code
                    _ftpUploader.DeleteFile(oldImageUrl);
                }

                return Ok(new { Message = "Successfully Updated", Success = true });
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "An error occurred while updating a brand.");
                return StatusCode(500, new { Message = ex.Message, Success = false });
            }
        }

        [HttpDelete("Delete/{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            try
            {
                await _brandService.DeleteBrandAsync(id);
                return Ok(new { Message = "Successfully Deleted", Success = true });
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "An error occurred while deleting a brand.");
                return StatusCode(500, new { Message = ex.Message, Success = false });
            }
        }
    }
}
