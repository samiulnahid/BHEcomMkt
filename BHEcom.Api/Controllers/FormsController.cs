using BHEcom.Common.Models;
using BHEcom.Data.Repositories;
using BHEcom.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BHEcom.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FormsController : ControllerBase
    {
        private readonly IFormsService _formsService;
        private readonly ILogger<FormsRepository> _logger;
        public FormsController(IFormsService formsService, ILogger<FormsRepository> logger)
        {
            _formsService = formsService;
            _logger = logger;
        }

        [HttpPost("Create")]
        public async Task<ActionResult> Create([FromBody] Forms form)
        {
            try
            {
                await _formsService.AddFormsAsync(form);
                return CreatedAtAction(nameof(GetById), new { id = form.FormID }, form);
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "An error occurred while adding a forms.");
                return StatusCode(500, ex.Message);

            }
        }

        [HttpGet("GetById/{id}")]
        public async Task<ActionResult<Forms>> GetById(Guid id)
        {
            try
            {
                var form = await _formsService.GetFormsByIdAsync(id);
                if (form == null)
                {
                    return NotFound();
                }
                return Ok(form);
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "An error occurred while getting a forms.");
                return StatusCode(500, ex.Message);

            }
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<Forms>>> GetAll()
        {
            try
            {
                var forms = await _formsService.GetAllFormssAsync();
                return Ok(forms);
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "An error occurred while getting all forms.");
                return StatusCode(500, ex.Message);

            }
        }

        [HttpPut("Update/{id}")]
        public async Task<ActionResult> Update(Guid id, [FromBody] Forms forms)
        {
            try
            {
                if (id != forms.FormID)
                {
                    return BadRequest();
                }
                await _formsService.UpdateFormsAsync(forms);
                return NoContent();
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "An error occurred while updating a forms.");
                return StatusCode(500, ex.Message);

            }
        }

        [HttpDelete("Delete/{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            try
            {
                await _formsService.DeleteFormsAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "An error occurred while deleting a forms.");
                return StatusCode(500, ex.Message);

            }
        }
    }

}
