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
    public class PageContentsController : ControllerBase
    {
        private readonly IPageContentService _pageContentService;
        private readonly ILogger<PageContentRepository> _logger;
        public PageContentsController(IPageContentService pageContentService, ILogger<PageContentRepository> logger)
        {
            _pageContentService = pageContentService;
            _logger = logger;
        }

        [HttpPost("Create")]
        public async Task<ActionResult> Create([FromBody] PageContent pageContent)
        {
            try
            {
                await _pageContentService.AddPageContentAsync(pageContent);
                return CreatedAtAction(nameof(GetById), new { id = pageContent.ContentID }, pageContent);
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "An error occurred while adding a pageContent.");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("GetById/{id}")]
        public async Task<ActionResult<PageContent>> GetById(Guid id)
        {
            try
            {
                var pageContent = await _pageContentService.GetPageContentByIdAsync(id);
                if (pageContent == null)
                {
                    return NotFound();
                }
                return Ok(pageContent);
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "An error occurred while getting a pageContent.");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<PageContent>>> GetAll()
        {
            try
            {
                var pageContents = await _pageContentService.GetAllPageContentsAsync();
                return Ok(pageContents);
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "An error occurred while getting all pageContent.");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("Update/{id}")]
        public async Task<ActionResult> Update(Guid id, [FromBody] PageContent pageContent)
        {
            try
            {
                if (id != pageContent.ContentID)
                {
                    return BadRequest();
                }
                await _pageContentService.UpdatePageContentAsync(pageContent);
                return NoContent();
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "An error occurred while updating a pageContent.");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("Delete/{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            try
            {
                await _pageContentService.DeletePageContentAsync(id);
                return NoContent();

            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "An error occurred while deleting a pageContent.");
                return StatusCode(500, ex.Message);
            }
        }
    }

}
