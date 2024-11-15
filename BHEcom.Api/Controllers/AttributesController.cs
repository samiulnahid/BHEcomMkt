using BHEcom.Data.Repositories;
using Microsoft.AspNetCore.Mvc;
using BHEcom.Common.Models;
using BHEcom.Services.Interfaces;

namespace BHEcom.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AttributesController : ControllerBase
    {
        private readonly IAttributeService _attributeService;
        private readonly ILogger<AttributeRepository> _logger;
        public AttributesController(IAttributeService attributeService, ILogger<AttributeRepository> logger)
        {
            _attributeService = attributeService;
            _logger = logger;
        }

        [HttpPost("Create")]
        public async Task<ActionResult> Create([FromBody] Attributes attribute)
        {
            try
            {
                await _attributeService.AddAttributeAsync(attribute);
                return CreatedAtAction(nameof(GetById), new { id = attribute.AttributeID }, attribute);
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "An error occurred while adding a attribute.");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("GetById/{id}")]
        public async Task<ActionResult<Attributes>> GetById(Guid id)
        {
            try
            {
                var attribute = await _attributeService.GetAttributeByIdAsync(id);
                if (attribute == null)
                {
                    return NotFound();
                }
                return Ok(attribute);
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "An error occurred while getting a attribute.");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<Attributes>>> GetAll()
        {
            try
            {
                var attributes = await _attributeService.GetAllAttributesAsync();
                return Ok(attributes);
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "An error occurred while getting all attribute.");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("Update/{id}")]
        public async Task<ActionResult> Update(Guid id, [FromBody] Attributes attribute)
        {
            try
            {
                if (id != attribute.AttributeID)
                {
                    return BadRequest();
                }
                await _attributeService.UpdateAttributeAsync(attribute);
                return NoContent();
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "An error occurred while updating a attribute.");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("Delete/{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            try
            {
                await _attributeService.DeleteAttributeAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "An error occurred while deleting a attribute.");
                return StatusCode(500, ex.Message);
            }
        }
    }

}
