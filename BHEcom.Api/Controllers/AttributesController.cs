using BHEcom.Data.Repositories;
using Microsoft.AspNetCore.Mvc;
using BHEcom.Common.Models;
using BHEcom.Services.Interfaces;
using Microsoft.Extensions.Logging;
using System;

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
                return Ok(new { id = attribute.AttributeID, Success = true });
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "An error occurred while adding a attribute.");
                return StatusCode(500, new { Message = ex.Message, Success = false });
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
                    return Ok(new { data = attribute,Message = "No data found!", Success = true });
                }
                return Ok(new { data = attribute, Success = true });
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "An error occurred while getting a attribute.");
                return StatusCode(500, new { Message = ex.Message, Success = false });
            }
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<Attributes>>> GetAll()
        {
            try
            {
                var attributes = await _attributeService.GetAllAttributesAsync();
                if (attributes == null)
                    return Ok(new { data = attributes, Message = "No data found!", Success = true });
                return Ok(new { data = attributes, Success = true });

            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "An error occurred while getting all attribute.");
                return StatusCode(500, new { Message = ex.Message, Success = false });
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
                return Ok(new { Message = "Successfully Updated", Success = true });
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "An error occurred while updating a attribute.");
                return StatusCode(500, new { Message = ex.Message, Success = false });
            }
        }

        [HttpDelete("Delete/{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            try
            {
                await _attributeService.DeleteAttributeAsync(id);
                return Ok(new { Message = "Successfully Deleted", Success = true });
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "An error occurred while deleting a attribute.");
                return StatusCode(500, new { Message = ex.Message, Success = false });
            }
        }
    }

}
