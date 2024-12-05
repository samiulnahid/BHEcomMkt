using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using BHEcom.Common.Models;
using BHEcom.Services;
using BHEcom.Services.Interfaces;
using BHEcom.Data.Repositories;

namespace BHEcom.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly ILogger<CategoryRepository> _logger;
        public CategoriesController(ICategoryService categoryService, ILogger<CategoryRepository> logger)
        {
            _categoryService = categoryService;
            _logger = logger;
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<Category>>> GetAll()
        {
            try
            {
                var categories = await _categoryService.GetAllCategoriesAsync();
                return Ok(categories);
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "An error occurred while adding a category.");
                return StatusCode(500, ex.Message);

            }
        }

        // Other actions if needed
        [HttpPost("Create")]
        public async Task<ActionResult> Create([FromBody] Category category)
        {
            try
            {
                category.CreatedDate = DateTime.Now;
                await _categoryService.AddCategoryAsync(category);
                return CreatedAtAction(nameof(GetById), new { id = category.CategoryID }, category);
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "An error occurred while getting a category.");
                return StatusCode(500, ex.Message);

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
                    return NotFound();
                }
                return Ok(category);
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "An error occurred while getting all category.");
                return StatusCode(500, ex.Message);

            }
        }



        [HttpPut("Update/{id}")]
        public async Task<ActionResult> Update(Guid id, [FromBody] Category category)
        {
            try
            {
                if (id != category.CategoryID)
                {
                    return BadRequest();
                }
                category.ModifiedDate = DateTime.Now;
                await _categoryService.UpdateCategoryAsync(category);
                return Ok(new { Message = "Successfully Updated", Success = true });
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "An error occurred while updating a category.");
                return StatusCode(500, ex.Message);

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
                return StatusCode(500, ex.Message);

            }
        }
    }
}
