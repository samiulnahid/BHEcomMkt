using Microsoft.AspNetCore.Mvc;
using BHEcom.Common.Models;
using BHEcom.Data.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;
using BHEcom.Services.Interfaces;


namespace BHEcom.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PagesController : ControllerBase
    {
        private readonly IPageService _pageService;
        private readonly ILogger<PageRepository> _logger;
        public PagesController(IPageService pageService, ILogger<PageRepository> logger)
        {
            _pageService = pageService;
            _logger = logger;
        }

        [HttpPost("Create")]
        public async Task<ActionResult> Create([FromBody] Page page)
        {
            try
            {
                await _pageService.AddPageAsync(page);
                return CreatedAtAction(nameof(GetById), new { id = page.PageID }, page);
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "An error occurred while adding a page.");
                return StatusCode(500, ex.Message);

            }
        }

        [HttpGet("GetById/{id}")]
        public async Task<ActionResult<Page>> GetById(Guid id)
        {
            try
            {
                var page = await _pageService.GetPageByIdAsync(id);
                if (page == null)
                {
                    return NotFound();
                }
                return Ok(page);
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "An error occurred while getting a page.");
                return StatusCode(500, ex.Message);

            }
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<Page>>> GetAll()
        {
            try
            {
                var pages = await _pageService.GetAllPagesAsync();
                return Ok(pages);
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "An error occurred while getting all page.");
                return StatusCode(500, ex.Message);

            }
        }

        [HttpPut("Update/{id}")]
        public async Task<ActionResult> Update(Guid id, [FromBody] Page page)
        {
            try
            {
                if (id != page.PageID)
                {
                    return BadRequest();
                }

                await _pageService.UpdatePageAsync(page);
                return NoContent();
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "An error occurred while updating a page.");
                return StatusCode(500, ex.Message);

            }
        }

        [HttpDelete("Delete/{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            try
            {
                await _pageService.DeletePageAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "An error occurred while delting a page.");
                return StatusCode(500, ex.Message);

            }
        }
    }

}
