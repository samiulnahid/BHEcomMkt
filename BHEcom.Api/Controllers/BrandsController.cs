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
    public class BrandsController : ControllerBase
    {
        private readonly IBrandService _brandService;
        private readonly ILogger<BrandRepository> _logger;
        public BrandsController(IBrandService brandService, ILogger<BrandRepository> logger)
        {
            _brandService = brandService;
            _logger = logger;
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<Brand>>> GetAll()
        {
            try
            {
                var brands = await _brandService.GetAllBrandsAsync();
                return Ok(brands);
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "An error occurred while adding a brand.");
                return StatusCode(500, ex.Message);
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
                    return NotFound();
                }
                return Ok(brand);
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "An error occurred while getting a brand.");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("Create")]
        public async Task<ActionResult> Create([FromBody] Brand brand)
        {
            try
            {
                await _brandService.AddBrandAsync(brand);
                return CreatedAtAction(nameof(GetById), new { id = brand.BrandID }, brand);
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "An error occurred while getting all brand.");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("Update/{id}")]
        public async Task<ActionResult> Update(Guid id, [FromBody] Brand brand)
        {
            try
            {

                if (id != brand.BrandID)
                {
                    return BadRequest();
                }

                await _brandService.UpdateBrandAsync(brand);
                return Ok(new { Message = "Successfully Updated", Success = true });
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "An error occurred while updating a brand.");
                return StatusCode(500, ex.Message);
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
                return StatusCode(500, ex.Message);
            }
        }
    }
}
