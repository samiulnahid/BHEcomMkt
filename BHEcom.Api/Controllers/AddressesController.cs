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
                var id = await _addressService.AddAddressAsync(address);
                if (id == Guid.Empty)
                {
                    ResponseModel responseModel = new ResponseModel { Code = HandleResponse.ErrorCode, Message = HandleResponse.InsertionFailed };
                    return Ok(responseModel);
                }
                return Ok(new { id, Success = true , Massage = "Create successfully." });
                
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "An error occurred while adding a address.");
                return StatusCode(500, new { Message = ex.Message, Success = false });
            }
        }

        [HttpPost("CreateAndUpdateMultiple")]
        public async Task<ActionResult> CreateAndUpdateMultiple([FromBody] List<Address> addresses)
        {
            try
            {
                if (addresses == null || addresses.Count == 0 || addresses.Count > 2)
                {
                    return BadRequest(new { Message = "Provide one or two addresses (Billing and/or Shipping).", Success = false });
                }

                var response = new
                {
                    Billing = new { Id = (Guid?)null, Success = false },
                    Shipping = new { Id = (Guid?)null, Success = false },
                    Success = false
                };

                foreach (var address in addresses)
                {
                    if (address.AddressType == null || address.AddressType == string.Empty)
                        address.AddressType = "Shipping";
                    if (address.AddressType?.ToLower() == "billing")
                    {
                        if (address.AddressID != Guid.Empty)
                        {
                            bool isUpdated = await _addressService.UpdateAddressAsync(address);
                            response = new
                            {
                                Billing = new { Id = isUpdated ? address.AddressID : (Guid?)null, Success = isUpdated },
                                Shipping = response.Shipping,
                                Success = response.Success || isUpdated
                            };
                        }
                        else
                        {
                            var id = await _addressService.AddAddressAsync(address);
                            response = new
                            {
                                Billing = new { Id = id != Guid.Empty ? id : (Guid?)null, Success = id != Guid.Empty },
                                Shipping = response.Shipping,
                                Success = response.Success || id != Guid.Empty
                            };
                        }
                    }
                    else if (address.AddressType?.ToLower() == "shipping")
                    {
                        if (address.AddressID != Guid.Empty )
                        {
                            bool isUpdated = await _addressService.UpdateAddressAsync(address);
                            response = new
                            {
                                Billing = response.Billing,
                                Shipping = new { Id = isUpdated ? address.AddressID : (Guid?)null, Success = isUpdated },
                                Success = response.Success || isUpdated
                            };
                        }
                        else
                        {
                            var id = await _addressService.AddAddressAsync(address);
                            response = new
                            {
                                Billing = response.Billing,
                                Shipping = new { Id = id != Guid.Empty ? id : (Guid?)null, Success = id != Guid.Empty },
                                Success = response.Success || id != Guid.Empty
                            };
                        }
                    }
                    else
                    {
                        return BadRequest(new { Message = "Invalid AddressType. Allowed values are 'Billing' and 'Shipping'.", Success = false });
                    }
                }

                if (!response.Billing.Success && !response.Shipping.Success)
                {
                    return Ok(new { Message = "Failed to create or update both addresses.", Success = false });
                }

                return Ok(new
                {
                    Data = new
                    {
                        Billing = response.Billing,
                        Shipping = response.Shipping
                    },
                    Success = true
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing multiple addresses.");
                return StatusCode(500, new { Message = ex.Message, Success = false });
            }
        }


        [HttpGet("GetById/{id}")]
        public async Task<ActionResult<Address>> GetById(Guid id)
        {
            try
            {
                var address = await _addressService.GetAddressByIdAsync(id);
                //if (address == null)
                //{
                //    return Ok(new { data = address, Success = true });
                //}
                // return Ok(address);
                return Ok(new { data = address, Success = true });
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "An error occurred while getting a address.");
                return StatusCode(500, new { Message = ex.Message, Success = false });
            }
        } 

        [HttpGet("GetByUserId/{id}/{type}")]
        public async Task<ActionResult<Address>> GetByUserId(Guid id, string type = "Billing")
        {
            try
            {
                string addressType = type.ToLower();
                var address = await _addressService.GetAddressByUserIdAsync(id, addressType);
                if (address == null)
                {
                    return Ok(new { data = address,Message = "No address available!", Success = true });
                }
                return Ok(new { data = address, Success = true });
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "An error occurred while getting a address.");
                return StatusCode(500, new { Message = ex.Message, Success = false });
            }
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<Address>>> GetAll()
        {
            try
            {
                var addresses = await _addressService.GetAllAddressesAsync();
                if (addresses == null)
                    return Ok(new { data = addresses, Message = "No address available!", Success = true });
                return Ok(new { data = addresses, Success = true });
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "An error occurred while getting all address.");
                return StatusCode(500, new { Message = ex.Message, Success = false });
            }
        }

        [HttpGet("GetAllByUserId/{id}")]
        public async Task<ActionResult<List<Address>>> GetAllByUserId(Guid id)
        {
            try
            {
                var addresses = await _addressService.GetAllAddressesByUserId(id);
                if (addresses == null || !addresses.Any())
                {
                    return Ok(new { data = addresses, Message = "No addresses found for the provided UserID.", Success = true });
                }
                return Ok(new { data = addresses, Success = true });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting all address by user id.");
                return StatusCode(500, new { Message = ex.Message, Success = false });
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
                    return Ok(new { Message = "Updated successful", Success = true });
                else
                {
                    ResponseModel responseModel = new ResponseModel { Code = HandleResponse.ErrorCode, Message = HandleResponse.UpdateFailed };
                    return Ok(responseModel);
                }

            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "An error occurred while updating a address.");
                return StatusCode(500, new { Message = ex.Message, Success = false });
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
                return StatusCode(500, new { Message = ex.Message, Success = false });
            }
        }
    }

}
