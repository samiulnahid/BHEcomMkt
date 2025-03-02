using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using BHEcom.Common.Models;
using BHEcom.Services;
using BHEcom.Services.Interfaces;
using BHEcom.Data.Repositories;
using BHEcom.Common.Helper;
using BHEcom.Services.Implementations;

namespace BHEcom.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly ILogger<CategoryRepository> _logger;
        private readonly FtpUploader _ftpUploader;

        public CategoriesController(ICategoryService categoryService, ILogger<CategoryRepository> logger, FtpUploader ftpUploader)
        {
            _categoryService = categoryService;
            _logger = logger;
            _ftpUploader = ftpUploader;
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<Category>>> GetAll()
        {
            try
            {
                var categories = await _categoryService.GetAllCategoriesAsync();
                return Ok(new { data = categories, Success = true });
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "An error occurred while adding a category.");
                return StatusCode(500, new { Message = ex.Message, Success = false });

            }
        }

        // Other actions if needed
        [HttpPost("Create")]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult> Create([FromForm] Category category, [FromForm] IFormFile imageFile)
        {
            try
            {
                if (imageFile != null && imageFile.Length > 0)
                {
                    string folderName = "ecom/category"; // Adjust based on the product folder
                    string imageUrl = await _ftpUploader.UploadFileAsync(imageFile, folderName);

                    // Assign the uploaded image URL to the product
                    category.Image = imageUrl;
                }
                category.CreatedDate = DateTime.Now;
               var (categoryID, isUnique) =  await _categoryService.AddCategoryAsync(category);

                if (!isUnique)
                {
                    return StatusCode(500, new { Message = "Category Name already exist!", Success = false });
                }

                return Ok(new { id = categoryID, Success = true });
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "An error occurred while getting a category.");
                return StatusCode(500, new { Message = ex.Message, Success = false });

            }
        }

        [HttpGet("GetById/{id}")]
        public async Task<ActionResult<Category>> GetById(Guid id)
        {
            try
            {
                var category = await _categoryService.GetCategoryByIdAsync(id);
                if (category == null)
                {
                    return Ok(new { data = category,Message = "Not Found!", Success = true });
                }
                return Ok(new { data = category, Success = true });
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "An error occurred while getting all category.");
                return StatusCode(500, new { Message = ex.Message, Success = false });

            }
        }



        [HttpPut("Update/{id}")]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult> Update(Guid id, [FromForm] Category category, [FromForm] IFormFile imageFile)
        {
            try
            {
                if (id != category.CategoryID)
                {
                    return BadRequest();
                }

                if (imageFile != null && imageFile.Length > 0)
                {
                    string folderName = "ecom/category"; // Adjust based on the product folder
                    string imageUrl = await _ftpUploader.UploadFileAsync(imageFile, folderName);

                    // Assign the uploaded image URL to the product
                    category.Image = imageUrl;
                }

                var (isUpdated, oldImageUrl, isUniqueName) = await _categoryService.UpdateCategoryAsync(category);

                if (!isUpdated)
                {
                    if (!isUniqueName)
                    {
                        return StatusCode(500, new { Message = "Category Name already exist!", Success = false });
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

                _logger.LogError(ex, "An error occurred while updating a category.");
                return StatusCode(500, new { Message = ex.Message, Success = false });

            }
        }

        [HttpDelete("Delete/{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            try
            {
                await _categoryService.DeleteCategoryAsync(id);
                return Ok(new { Message = "Successfully Deleted", Success = true });
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "An error occurred while deleting a category.");
                return StatusCode(500, new { Message = ex.Message, Success = false });

            }
        }
    }
}
