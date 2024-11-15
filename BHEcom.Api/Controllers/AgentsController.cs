using BHEcom.Common.Models;
using BHEcom.Data.Repositories;
using BHEcom.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BHEcom.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AgentsController : ControllerBase
    {
        private readonly IAgentService _agentService;
        private readonly ILogger<AgentRepository> _logger;
        public AgentsController(IAgentService agentService, ILogger<AgentRepository> logger)
        {
            _agentService = agentService;
            _logger = logger;
        }

        [HttpPost("Create")]
        public async Task<ActionResult> Create([FromBody] Agent agent)
        {
            try
            {
                Guid agentId = await _agentService.AddAgentAsync(agent);
                return CreatedAtAction(nameof(GetById), new { id = agent.AgentID }, agent);
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "An error occurred while adding a agent.");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("GetById/{id}")]
        public async Task<ActionResult<Agent>> GetById(Guid id)
        {
            try
            {
                var agent = await _agentService.GetAgentByIdAsync(id);
                if (agent == null)
                {
                    return NotFound();
                }
                return Ok(agent);
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "An error occurred while getting a agent.");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<Agent>>> GetAll()
        {
            try
            {
                var agents = await _agentService.GetAllAgentsAsync();
                return Ok(agents);
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "An error occurred while getting all agent.");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("Update/{id}")]
        public async Task<ActionResult> Update(Guid id, [FromBody] Agent agent)
        {
            try
            {
                if (id != agent.AgentID)
                {
                    return BadRequest();
                }
                await _agentService.UpdateAgentAsync(agent);
                return Ok("Successfully Updated");
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "An error occurred while updating a agent.");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("Delete/{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            try
            {
                await _agentService.DeleteAgentAsync(id);
                return Ok("Successfully Deleted");
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "An error occurred while deleting a agent.");
                return StatusCode(500, ex.Message);
            }
        }
    }

}
