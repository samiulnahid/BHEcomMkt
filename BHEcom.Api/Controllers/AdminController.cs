using BHEcom.Common.Models;
using BHEcom.Data.Repositories;
using BHEcom.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BHEcom.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;
        private readonly ILogger<AdminRepository> _logger;
        public AdminController(IAdminService adminService, ILogger<AdminRepository> logger)
        {
            _adminService = adminService;
            _logger = logger;
        }

        [HttpPost("Create")]
        public async Task<ActionResult> Create([FromBody] Useres user)
        {
            try
            {
                await _adminService.RegisterAsync(user);
                // return CreatedAtAction(nameof(GetById), new { id = user.UserId }, user);
                return StatusCode(200);
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "An error occurred while adding a agent.");
                return StatusCode(500, ex.Message);
            }
        }

        //[HttpGet("GetById/{id}")]
        //public async Task<ActionResult<Useres>> GetById(Guid id)
        //{
        //    try
        //    {
        //        var user = await _adminService.GetByIdAsync(id);
        //        if (user == null)
        //        {
        //            return NotFound();
        //        }
        //        return Ok(user);
        //    }
        //    catch (Exception ex)
        //    {

        //        _logger.LogError(ex, "An error occurred while getting a agent.");
        //        return StatusCode(500, ex.Message);
        //    }
        //}

        //[HttpGet("GetAll")]
        //public async Task<ActionResult<IEnumerable<Useres>>> GetAll()
        //{
        //    try
        //    {
        //        var admin = await _adminService.GetAllAsync();
        //        return Ok(admin);
        //    }
        //    catch (Exception ex)
        //    {

        //        _logger.LogError(ex, "An error occurred while getting all agent.");
        //        return StatusCode(500, ex.Message);
        //    }
        //}

        //[HttpPut("Update/{id}")]
        //public async Task<ActionResult> Update(Guid id, [FromBody] Useres user)
        //{
        //    try
        //    {
        //        if (id != user.UserId)
        //        {
        //            return BadRequest();
        //        }
        //        await _adminService.UpdateAsync(user);
        //        return NoContent();
        //    }
        //    catch (Exception ex)
        //    {

        //        _logger.LogError(ex, "An error occurred while updating a agent.");
        //        return StatusCode(500, ex.Message);
        //    }
        //}

        //[HttpDelete("Delete/{id}")]
        //public async Task<ActionResult> Delete(Guid id)
        //{
        //    try
        //    {
        //        await _adminService.DeleteAsync(id);
        //        return NoContent();
        //    }
        //    catch (Exception ex)
        //    {

        //        _logger.LogError(ex, "An error occurred while deleting a agent.");
        //        return StatusCode(500, ex.Message);
        //    }
        //}
    }

}
