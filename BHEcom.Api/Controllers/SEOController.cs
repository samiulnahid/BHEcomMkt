using BHEcom.Common.Models;
using BHEcom.Data.Repositories;
using BHEcom.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BHEcom.Api.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class SEOController : ControllerBase
    {
        private readonly ISEOService _seoService;
        private readonly ILogger<SEORepository> _logger;
        public SEOController(ISEOService seoService, ILogger<SEORepository> logger)
        {
            _seoService = seoService; 
            _logger = logger;
        }

        [HttpPost("Create")]
        public async Task<ActionResult> Create([FromBody] SEO seoModel)
        {
            try
            {
                await _seoService.AddSEOAsync(seoModel);
                return CreatedAtAction(nameof(GetById), new { id = seoModel.SEOId }, seoModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding a SEO.");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("GetById/{id}")]
        public async Task<ActionResult<SEO>> GetById(Guid id)
        {
            try
            {
                var seoModel = await _seoService.GetSEOByIdAsync(id);
                if (seoModel == null)
                {
                    return NotFound();
                }
                return Ok(seoModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting a SEO.");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<SEO>>> GetAll()
        {
            try
            {
                var seoModels = await _seoService.GetAllSEOAsync();
                return Ok(seoModels);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting all SEO.");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("Update/{id}")]
        public async Task<ActionResult> Update(Guid id, [FromBody] SEO seoModel)
        {
            try
            {
                if (id != seoModel.SEOId)
                {
                    return BadRequest();
                }
                await _seoService.UpdateSEOAsync(seoModel);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating a SEO.");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("Delete/{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            try
            {
                await _seoService.DeleteSEOAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting a SEO.");
                return StatusCode(500, ex.Message);
            }
        }
    }
}
