using BHEcom.Common.Models;
using BHEcom.Data.Repositories;
using BHEcom.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BHEcom.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FormFieldsController : ControllerBase
    {
        private readonly IFormFieldService _formFieldService;
        private readonly ILogger<FormFieldRepository> _logger;
        public FormFieldsController(IFormFieldService formFieldService, ILogger<FormFieldRepository> logger)
        {
            _formFieldService = formFieldService;
            _logger = logger;
        }

        [HttpPost("Create")]
        public async Task<ActionResult> Create([FromBody] FormField formField)
        {
            try
            {
                await _formFieldService.AddFormFieldAsync(formField);
                return CreatedAtAction(nameof(GetById), new { id = formField.FieldID }, formField);
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "An error occurred while adding a formField.");
                return StatusCode(500, ex.Message);

            }
        }

        [HttpGet("GetById/{id}")]
        public async Task<ActionResult<FormField>> GetById(Guid id)
        {
            try
            {
                var formField = await _formFieldService.GetFormFieldByIdAsync(id);
                if (formField == null)
                {
                    return NotFound();
                }
                return Ok(formField);
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "An error occurred while getting a formField.");
                return StatusCode(500, ex.Message);

            }
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<FormField>>> GetAll()
        {
            try
            {
                var formFields = await _formFieldService.GetAllFormFieldsAsync();
                return Ok(formFields);
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "An error occurred while getting all formField.");
                return StatusCode(500, ex.Message);

            }
        }

        [HttpPut("Update/{id}")]
        public async Task<ActionResult> Update(Guid id, [FromBody] FormField formField)
        {
            try
            {
                if (id != formField.FieldID)
                {
                    return BadRequest();
                }
                await _formFieldService.UpdateFormFieldAsync(formField);
                return NoContent();
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "An error occurred while updating a formField.");
                return StatusCode(500, ex.Message);

            }
        }

        [HttpDelete("Delete/{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            try
            {
                await _formFieldService.DeleteFormFieldAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "An error occurred while deleting a formField.");
                return StatusCode(500, ex.Message);

            }
        }
    }

}
