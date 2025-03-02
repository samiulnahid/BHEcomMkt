using BHEcom.Common.Models;
using BHEcom.Data.Repositories;
using BHEcom.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

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
                return Ok(new { id = agentId, Success = true });
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "An error occurred while adding a agent.");
                return StatusCode(500, new { Message = ex.Message, Success = false });
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
                    return Ok(new { data = agent,Message = "No data found!", Success = true });
                }
                return Ok(new { data = agent, Success = true });
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "An error occurred while getting a agent.");
                return StatusCode(500, new { Message = ex.Message, Success = false });
            }
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<Agent>>> GetAll()
        {
            try
            {
                var agents = await _agentService.GetAllAgentsAsync();
                if (agents == null)
                    return Ok(new { data = agents, Message = "No data found!", Success = true });
                return Ok(new { data = agents, Success = true });
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "An error occurred while getting all agent.");
                return StatusCode(500, new { Message = ex.Message, Success = false });
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
                bool isUpdate = await _agentService.UpdateAgentAsync(agent);
                if (!isUpdate) 
                     return Ok(new { Message = "Update Unsuccessful ", Success = false });
                return Ok(new { Message = "Successfully Updated", Success = true });
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "An error occurred while updating a agent.");
                return StatusCode(500, new { Message = ex.Message, Success = false });
            }
        }

        [HttpDelete("Delete/{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            try
            {
                bool isDelete = await _agentService.DeleteAgentAsync(id);
                if (!isDelete)
                            return Ok(new { Message = "Delete unsuccessful", Success = false });
                return Ok(new { Message = "Successfully Deleted", Success = true });
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "An error occurred while deleting a agent.");
                return StatusCode(500, new { Message = ex.Message, Success = false });
            }
        }
    }

}
