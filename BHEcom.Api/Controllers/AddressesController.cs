using BHEcom.Common.Models;
using BHEcom.Data.Repositories;
using BHEcom.Services.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using static BHEcom.Common.Handler.Constant;

namespace BHEcom.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AddressesController : ControllerBase
    {
        private readonly IAddressService _addressService;
        private readonly ILogger<AddressRepository> _logger;
        public AddressesController(IAddressService addressService, ILogger<AddressRepository> logger)
        {
            _addressService = addressService;
            _logger = logger;
        }

        [HttpPost("Create")]
        public async Task<ActionResult> Create([FromBody] Address address)
        {
            try
            {
                bool isCreated = await _addressService.AddAddressAsync(address);
                if (isCreated) 
                    return CreatedAtAction(nameof(GetById), new { id = address.AddressID }, address);
                
                else
                {
                   ResponseModel responseModel = new ResponseModel { Code = HandleResponse.ErrorCode, Message = HandleResponse.InsertionFailed };
                   return Ok(responseModel);
                }
                
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "An error occurred while adding a address.");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("GetById/{id}")]
        public async Task<ActionResult<Address>> GetById(Guid id)
        {
            try
            {
                var address = await _addressService.GetAddressByIdAsync(id);
                if (address == null)
                {
                    return Ok(HandleResponse.EmptyData);
                }
                return Ok(address);
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "An error occurred while getting a address.");
                return StatusCode(500, ex.Message);
            }
        }
        
        [HttpGet("GetByUserId/{id}")]
        public async Task<ActionResult<Address>> GetByUserId(Guid id)
        {
            try
            {
                var address = await _addressService.GetAddressByUserIdAsync(id);
                if (address == null)
                {
                    return Ok(HandleResponse.EmptyData);
                }
                return Ok(address);
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "An error occurred while getting a address.");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<Cart>>> GetAll()
        {
            try
            {
                var addresses = await _addressService.GetAllAddressesAsync();
                if (addresses == null)
                    return Ok(HandleResponse.EmptyData);
                return Ok(addresses);
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "An error occurred while getting all address.");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("Update/{id}")]
        public async Task<ActionResult> Update(Guid id, [FromBody] Address address)
        {
            try
            {
                if (id != address.AddressID)
                {
                    return BadRequest();
                }
                bool isUpdated = await _addressService.UpdateAddressAsync(address);
                if (isUpdated)
                    return CreatedAtAction(nameof(GetById), new { id = address.AddressID }, address);

                else
                {
                    ResponseModel responseModel = new ResponseModel { Code = HandleResponse.ErrorCode, Message = HandleResponse.UpdateFailed };
                    return Ok(responseModel);
                }

            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "An error occurred while updating a address.");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("Delete/{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            try
            {
                bool isDeleted = await _addressService.DeleteAddressAsync(id);
                if (isDeleted)
                {
                    ResponseModel responseModel = new ResponseModel { Code = HandleResponse.SuccessCode, Message = HandleResponse.Delete };
                    return Ok(responseModel);
                }

                else
                {
                    ResponseModel responseModel = new ResponseModel { Code = HandleResponse.ErrorCode, Message = HandleResponse.DeleteFailed };
                    return Ok(responseModel);
                }

            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "An error occurred while deleting a address.");
                return StatusCode(500, ex.Message);
            }
        }
    }

}
