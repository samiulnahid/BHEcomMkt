using BHEcom.Common.Models;
using BHEcom.Data.Repositories;
using BHEcom.Services.Interfaces;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

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
        public async Task<ActionResult> Create([FromBody] User user)
        {
            try
            {
                await _adminService.RegisterAsync(user);
                // return CreatedAtAction(nameof(GetById), new { id = user.UserId }, user);
                return StatusCode(200);
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "An error occurred while adding a member.");
                return StatusCode(500, new { Message = ex.Message, Success = false });
            }
        }

        [HttpPost("Register")]
        public async Task<ActionResult> Register([FromBody] User user)
        {
                try
                {

                if (user == null || string.IsNullOrWhiteSpace(user.UserName))
                {
                    return BadRequest(new { Success = false, Message = "User value required! " });
                }


                user.LoweredUserName = user.UserName.ToLower();

                bool IsExisted = await _adminService.CheckUserNameExistAsync(user);
                if (IsExisted)
                {
                    return BadRequest(new { Success = false, Message = "UserName already exist. Please change user name" });
                }

                user.IsAnonymous = false;
                user.LastActivityDate = DateTime.Now;

                if (IsValidEmail(user.UserName))
                    user.Email = user.UserName;
                else if (IsPhoneNumber(user.UserName))
                    user.Email = "";
                else
                {
                    _logger.LogError("An error occurred while registering a User.");
                    return StatusCode(500, "Please Enter Valid UserName");
                }
                if(user.RoleName==null)
                    return StatusCode(500, "Please Give Role Name");

                string roleName = string.Empty;
                switch (user.RoleName.ToLower())
                {
                    case "user":
                        roleName = RoleName.User.ToString() ;
                        break;

                    case "seller":
                        roleName = RoleName.Seller.ToString();
                        break;

                    case "admin":
                        roleName = RoleName.Admin.ToString();
                        break;

                    default:
                        return StatusCode(500, "Please Give Correct Role Name");
                }


                Guid userId = await _adminService.RegisterAsyncMembership(user, roleName, user.Email);

                //Guid userId = await _adminService.CreateUserAsync(user, roleName, user.Email);

                if (userId == Guid.Empty)
                {
                    return StatusCode(500, "Register Unsuccessful");
                }
                return Ok(new { id = userId, Success = true, Message = "Successfully Register" });
            }
                catch (Exception ex)
               {

                   _logger.LogError(ex, "An error occurred while adding a user.");
                    return StatusCode(500, new { Message = ex.Message, Success = false });
                }
            }



            [HttpPost("Login")]
        public async Task<ActionResult> Login([FromBody] User request)
        {
            if (string.IsNullOrEmpty(request.UserName) || string.IsNullOrEmpty(request.Password))
            {
                return BadRequest(new { Success = false, Message = "Username and Password are required." });
            }

            var (isSuccess, userId, roleName,userName) = await _adminService.ValidateUser(request.UserName, request.Password);

            if (!isSuccess)
                return Unauthorized(new { Success = false, Message = "Invalid credentials or account is not approved/locked." });
            var user = new
            {
                UserId = userId,
                UserName = userName,
                RoleName = roleName,
                Message = "Login Successful."
            };
            return Ok(new { Success = true, data = user });
        }

        private bool IsValidEmail(string email)
        {
            string emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(email, emailPattern);
        }

        private bool IsPhoneNumber(string phone)
        {
            string phonePattern = @"^\+?[0-9]{10,15}$"; // Adjust for country codes if needed
            return Regex.IsMatch(phone, phonePattern);
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
        //        return StatusCode(500, new { Message = ex.Message, Success = false });
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
        //        return StatusCode(500, new { Message = ex.Message, Success = false });
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
        //        return StatusCode(500, new { Message = ex.Message, Success = false });
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
        //        return StatusCode(500, new { Message = ex.Message, Success = false });
        //    }
        //}


    }

}
