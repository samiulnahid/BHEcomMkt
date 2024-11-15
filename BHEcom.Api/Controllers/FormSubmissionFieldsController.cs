using BHEcom.Common.Models;
using BHEcom.Data.Repositories;
using BHEcom.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BHEcom.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FormSubmissionFieldsController : ControllerBase
    {
        private readonly IFormSubmissionFieldService _formSubmissionFieldService;
        private readonly ILogger<FormSubmissionFieldRepository> _logger;
        public FormSubmissionFieldsController(IFormSubmissionFieldService formSubmissionFieldService, ILogger<FormSubmissionFieldRepository> logger)
        {
            _formSubmissionFieldService = formSubmissionFieldService;
            _logger = logger;
        }

        [HttpPost("Create")]
        public async Task<ActionResult> Create([FromBody] FormSubmissionField formSubmissionField)
        {
            try
            {
                await _formSubmissionFieldService.AddFormSubmissionFieldAsync(formSubmissionField);
                return CreatedAtAction(nameof(GetById), new { id = formSubmissionField.SubmissionFieldID }, formSubmissionField);
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "An error occurred while adding a formSubmissionField.");
                return StatusCode(500, ex.Message);

            }
        }

        [HttpGet("GetById/{id}")]
        public async Task<ActionResult<FormSubmissionField>> GetById(Guid id)
        {
            try
            {
                var formSubmissionField = await _formSubmissionFieldService.GetFormSubmissionFieldByIdAsync(id);
                if (formSubmissionField == null)
                {
                    return NotFound();
                }
                return Ok(formSubmissionField);
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "An error occurred while getting a formSubmissionField.");
                return StatusCode(500, ex.Message);

            }
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<FormSubmissionField>>> GetAll()
        {
            try
            {
                var formSubmissionFields = await _formSubmissionFieldService.GetAllFormSubmissionFieldsAsync();
                return Ok(formSubmissionFields);
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "An error occurred while getting all formSubmissionField.");
                return StatusCode(500, ex.Message);

            }
        }

        [HttpPut("Update/{id}")]
        public async Task<ActionResult> Update(Guid id, [FromBody] FormSubmissionField formSubmissionField)
        {
            try
            {
                if (id != formSubmissionField.SubmissionFieldID)
                {
                    return BadRequest();
                }

                await _formSubmissionFieldService.UpdateFormSubmissionFieldAsync(formSubmissionField);
                return NoContent();
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "An error occurred while updating a formSubmissionField.");
                return StatusCode(500, ex.Message);

            }
        }

        [HttpDelete("Delete/{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            try
            {
                await _formSubmissionFieldService.DeleteFormSubmissionFieldAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "An error occurred while deleting a formSubmissionField.");
                return StatusCode(500, ex.Message);

            }
        }
    }

}
