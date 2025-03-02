using BHEcom.Common.Models;
using BHEcom.Data.Repositories;
using BHEcom.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BHEcom.Api.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class ShippingsController : ControllerBase
    {
        private readonly IShippingService _shippingService;
        private readonly ILogger<ShippingRepository> _logger;
        public ShippingsController(IShippingService shippingService, ILogger<ShippingRepository> logger)
        {
            _shippingService = shippingService;
            _logger = logger;
        }

        [HttpPost("Create")]
        public async Task<ActionResult> Create([FromBody] Shipping shipping)
        {
            try
            {
                await _shippingService.AddShippingAsync(shipping);
                return Ok(new { id = shipping.ShippingID, Success = true });
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "An error occurred while adding a shipping.");
                return StatusCode(500, new { Message = ex.Message, Success = false });
            }
        }

        [HttpPost("CreateOrUpdate")]
        public async Task<ActionResult> CreateOrUpdate([FromBody] Shipping shipping)
        {
            try
            {
                await _shippingService.CreateOrUpdateAsync(shipping);
                return Ok(new { success = true, message = "Shipping record created or updated successfully.", data = shipping });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating or updating a shipping.");
                return StatusCode(500, new { Message = ex.Message, Success = false });
            }
        }



        [HttpGet("GetById/{id}")]
        public async Task<ActionResult<Shipping>> GetById(Guid id)
        {
            try
            {
                var shipping = await _shippingService.GetShippingByIdAsync(id);
                //if (shipping == null)
                //{
                //    return NotFound();
                //}
                return Ok(new { data = shipping, Success = true });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while getById shipping.");
                return StatusCode(500, new { Message = ex.Message, Success = false });
            }
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<Shipping>>> GetAll()
        {
            try
            {
                var shippings = await _shippingService.GetAllShippingsAsync();
                return Ok(new { data = shippings, Success = true });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while get all shipping.");
                return StatusCode(500, new { Message = ex.Message, Success = false });
            }
        }

        [HttpPut("Update/{id}")]
        public async Task<ActionResult> Update(Guid id, [FromBody] Shipping shipping)
        {
            try
            {

                if (id != shipping.ShippingID)
                {
                    return BadRequest();
                }

                await _shippingService.UpdateShippingAsync(shipping);
                return Ok(new { Message = "Successfully Updated", Success = true });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating a shipping.");
                return StatusCode(500, new { Message = ex.Message, Success = false });
            }
        }

        [HttpDelete("Delete/{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            try
            {
                await _shippingService.DeleteShippingAsync(id);
                return Ok(new { Message = "Successfully Deleted", Success = true });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting a shipping.");
                return StatusCode(500, new { Message = ex.Message, Success = false });
            }
        }

    }
}
