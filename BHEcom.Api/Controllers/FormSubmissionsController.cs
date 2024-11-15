using BHEcom.Common.Models;
using BHEcom.Data.Repositories;
using BHEcom.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BHEcom.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FormSubmissionsController : ControllerBase
    {
        private readonly IFormSubmissionService _formSubmissionService;
        private readonly ILogger<FormSubmissionRepository> _logger;
        public FormSubmissionsController(IFormSubmissionService formSubmissionService, ILogger<FormSubmissionRepository> logger)
        {
            _formSubmissionService = formSubmissionService;
            _logger = logger;
        }

        [HttpPost("Create")]
        public async Task<ActionResult> Create([FromBody] FormSubmission formSubmission)
        {
            try
            {
                await _formSubmissionService.AddFormSubmissionAsync(formSubmission);
                return CreatedAtAction(nameof(GetById), new { id = formSubmission.SubmissionID }, formSubmission);
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "An error occurred while adding a formSubmission.");
                return StatusCode(500, ex.Message);

            }
        }

        [HttpGet("GetById/{id}")]
        public async Task<ActionResult<FormSubmission>> GetById(Guid id)
        {
            try
            {
                var formSubmission = await _formSubmissionService.GetFormSubmissionByIdAsync(id);
                if (formSubmission == null)
                {
                    return NotFound();
                }
                return Ok(formSubmission);
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "An error occurred while getting a formSubmission.");
                return StatusCode(500, ex.Message);

            }
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<FormSubmission>>> GetAll()
        {
            try
            {
                var formSubmissions = await _formSubmissionService.GetAllFormSubmissionsAsync();
                return Ok(formSubmissions);
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "An error occurred while getting all formSubmission.");
                return StatusCode(500, ex.Message);

            }
        }

        [HttpPut("Update/{id}")]
        public async Task<ActionResult> Update(Guid id, [FromBody] FormSubmission formSubmission)
        {
            try
            {
                if (id != formSubmission.SubmissionID)
                {
                    return BadRequest();
                }

                await _formSubmissionService.UpdateFormSubmissionAsync(formSubmission);
                return NoContent();
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "An error occurred while updating a formSubmission.");
                return StatusCode(500, ex.Message);

            }
        }

        [HttpDelete("Delete/{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            try
            {
                await _formSubmissionService.DeleteFormSubmissionAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "An error occurred while deleting a formSubmission.");
                return StatusCode(500, ex.Message);

            }
        }
    }

}
