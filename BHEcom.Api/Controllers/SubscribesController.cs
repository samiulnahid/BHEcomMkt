using BHEcom.Common.Models;
using BHEcom.Data.Repositories;
using BHEcom.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace BHEcom.Api.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class SubscribesController : ControllerBase
    {
        private readonly ISubscribeService _subscribeService;
        private readonly ILogger<SubscribesController> _logger;

        public SubscribesController(ISubscribeService subscribeService, ILogger<SubscribesController> logger)
        {
            _subscribeService = subscribeService;
            _logger = logger;
        }

        [HttpPost("Create")]
        public async Task<ActionResult> Create([FromBody] Subscribe subscribe)
        {
            try
            {
                Guid subscribeId = await _subscribeService.AddSubscribeAsync(subscribe);
                return Ok(new { id = subscribeId, Success = true });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding a subscription.");
                return StatusCode(500, new { Message = ex.Message, Success = false });
            }
        }

        [HttpGet("GetById/{id}")]
        public async Task<ActionResult<Subscribe>> GetById(Guid id)
        {
            try
            {
                var subscription = await _subscribeService.GetSubscribeByIdAsync(id);
                if (subscription == null)
                    return NotFound();

                return Ok(new { data = subscription, Success = true });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching a subscription.");
                return StatusCode(500, new { Message = ex.Message, Success = false });
            }
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<Subscribe>>> GetAll()
        {
            try
            {
                var subscriptions = await _subscribeService.GetAllSubscribesAsync();
                return Ok(new { data = subscriptions, Success = true });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching subscriptions.");
                return StatusCode(500, new { Message = ex.Message, Success = false });
            }
        }

        [HttpPut("Update/{id}")]
        public async Task<ActionResult> Update(Guid id, [FromBody] Subscribe subscribe)
        {
            try
            {
                if (id != subscribe.SubscribeID)
                    return BadRequest();

                bool isUpdated = await _subscribeService.UpdateSubscribeAsync(subscribe);
                if (!isUpdated)
                    return Ok(new { Message = "Update Unsuccessful", Success = false });

                return Ok(new { Message = "Successfully Updated", Success = true });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the subscription.");
                return StatusCode(500, new { Message = ex.Message, Success = false });
            }
        }

        [HttpDelete("Delete/{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            try
            {
                bool isDeleted = await _subscribeService.DeleteSubscribeAsync(id);
                if (!isDeleted)
                    return Ok(new { Message = "Delete Unsuccessful", Success = false });

                return Ok(new { Message = "Successfully Deleted", Success = true });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the subscription.");
                return StatusCode(500, new { Message = ex.Message, Success = false });
            }
        }
    }
}
