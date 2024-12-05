using BHEcom.Common.Models;
using BHEcom.Data.Repositories;
using BHEcom.Services.Implementations;
using BHEcom.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using static BHEcom.Common.Handler.Constant;

namespace BHEcom.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StoreController : ControllerBase
    {
        private readonly IStoreService _storeService;
        private readonly ILogger<StoreRepository> _logger;
        private readonly IAdminService _adminService;
        private readonly IAddressService _addressService;
        private readonly IAgentService _agentService;
        public StoreController(IStoreService sotreService, ILogger<StoreRepository> logger, IAddressService addressService, IAgentService agentService, IAdminService adminService)
        {
            _storeService = sotreService;
            _addressService = addressService;
            _agentService = agentService;
            _adminService = adminService;
            _logger = logger;
        }

        [HttpPost("Create")]
        public async Task<ActionResult> Create([FromBody] StoreManager stores)
        {
            try
            {
                User user = new User
                {
                    UserName = stores.ContactPhone,
                    LoweredUserName = stores.ContactPhone.ToLower(),
                    IsAnonymous = false,
                    LastActivityDate = DateTime.Now,
                    Password = new Random().Next(100000, 999999).ToString(), // Generate a random 6-digit password

                    Email = stores.ContactEmail
                };

                string email = stores.ContactEmail; 
                string roleName = "StoreAdmin";

                Guid userId = await _adminService.RegisterAsyncMembership(user, roleName, stores.ContactEmail);

                //Guid userId = await _adminService.CreateUserAsync(user, roleName, email);
                Store store = new Store();
                if (userId != Guid.Empty) {
                    
                   Address address = new Address
                    {
                        UserID = userId,
                        AddressLine1 = stores.AddressLine,
                        City = stores.City,
                        State = stores.State,
                        ZipCode = stores.ZipCode,
                        Country = stores.Country,   
                    };
                    var addressId = await _addressService.AddAddressAsync(address);

                    Agent agent = new Agent
                    {
                        UserID = userId,
                        AgencyName =  stores.AgencyName,
                        ContactPerson = stores.ContactPerson,
                        ContactEmail = stores.ContactEmail,
                        ContactPhone = stores.ContactPhone,
                    };
                    Guid agentId = await _agentService.AddAgentAsync(agent);
                    Guid storeId = Guid.Empty;

                    if (agentId != Guid.Empty) {

                        store.OwnerID = agentId;
                        store.StoreName = stores.StoreName;
                        store.Description = stores.Description;
                        store.CreatedDate = DateTime.Now;
                        store.IsActive = true;

                        storeId = await _storeService.AddStoreAsync(store);
                    }

                    if (storeId != Guid.Empty)
                    {
                        return Ok(new { StoreId = storeId, Success = true });
                    }
                }
                return BadRequest(new { Success = false, Message = "Failed to add store." });
                //return CreatedAtAction(nameof(GetById), new { id = store.StoreID }, store);

            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "An error occurred while adding a store.");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("GetById/{id}")]
        public async Task<ActionResult<StoreManager>> GetById(Guid id)
        {
            try
            {
                var store = await _storeService.GetStoreByIdAsync(id);
                if (store == null)
                {
                    return NotFound();
                }
                return Ok(store);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while getById store.");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<StoreManager>>> GetAll()
        {
            try
            {
                var stores = await _storeService.GetAllStoreManagersAsync();
                return Ok(stores);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while get all sotre.");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("Update/{id}")]
        public async Task<ActionResult> Update(Guid id, [FromBody] StoreManager stores)
        {
            try
            {

                if (id != stores.StoreID)
                {
                    return BadRequest();
                }

                User user = new User
                {
                    UserId = stores.UserId,
                    UserName = stores.ContactPhone,
                    LoweredUserName = stores.ContactPhone,
                    LastActivityDate = DateTime.Now,
                };
                bool isUserUpdate = await _adminService.UpdateUserNameAsync(user);

                Store store = new Store();
                if (isUserUpdate)
                {

                    Address address = new Address
                    {
                        AddressID = stores.AddressID,
                        UserID = stores.UserId,
                        AddressLine1 = stores.AddressLine,
                        City = stores.City,
                        State = stores.State,
                        ZipCode = stores.ZipCode,
                        Country = stores.Country,
                    };
                    bool isAddressUpdated = await _addressService.UpdateAddressAsync(address);

                    Agent agent = new Agent
                    {
                        AgentID = stores.OwnerID,
                        UserID = stores.UserId,
                        AgencyName = stores.AgencyName,
                        ContactPerson = stores.ContactPerson,
                        ContactEmail = stores.ContactEmail,
                        ContactPhone = stores.ContactPhone,
                    };
                    bool isAgentUpdated = await _agentService.UpdateAgentAsync(agent);

                    if (isAgentUpdated)
                    {
                        store.StoreID = stores.StoreID;
                        store.StoreName = stores.StoreName;
                        store.Description = stores.Description;
                        store.IsActive = true;

                        await _storeService.UpdateStoreAsync(store);

                    }
                }
                return Ok(new { StoreId = store.StoreID, Success = true });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating a store.");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("Delete/{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            try
            {
                await _storeService.DeleteStoreAsync(id);
                return Ok(new { Message = "Successfully Deleted", Success = true });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting a store.");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("CreateConfig")]
        public async Task<ActionResult> CreateConfig([FromBody] StoreConfig storeConfig)
        {
            if (storeConfig == null || storeConfig.StoreBrands == null || storeConfig.StoreCategories == null || storeConfig.StoreID == Guid.Empty)
            {
                return BadRequest("Invalid data.");
            }
            bool isCreated = await _storeService.CreateStoreConfigAsync(storeConfig);

            return Ok(new { Message = "Successfully Created", Success = true });

        }

        [HttpGet("GetStoreConfigById/{id}")]
        public async Task<ActionResult<StoreConfig>> GetStoreConfigById(Guid id)
        {
            var storeData = await _storeService.GetStoreConfigAsync(id);

            if (storeData == null)
            {
                return NotFound();
            }

            return Ok(storeData);
        }

        [HttpPost("UpdateStoreConfig/{id}")]
        public async Task<ActionResult> UpdateStoreConfig(Guid id, [FromBody] StoreConfig storeConfig) {

            if (storeConfig == null)
            {
                if(storeConfig.StoreBrands == null && storeConfig.StoreCategories == null)
                {
                    return BadRequest("Invalid data.");
                }
            }
            bool isDeleted = await _storeService.DeleteStoreConfigAsync(id);

            if (!isDeleted)
            {
                return BadRequest("Error data.");
            };
            bool isCreated = await _storeService.CreateStoreConfigAsync(storeConfig);
            if (isCreated) {
            
            }
            return Ok(new { Message = "Successfully Updated", Success = true });
        }

        //[HttpPost("CreateStoreProductField")]
        //public async Task<ActionResult> CreateStoreProductField([FromBody] List<StoreProductField> storeProductFieldList)
        //{
        //    if (storeProductFieldList == null || (!storeProductFieldList.Any()))
        //    {
        //        return BadRequest("Invalid data.");
        //    }
        //    bool isCreated = await _storeService.CreateStoreProductFieldAsync(storeProductFieldList);

        //    return NoContent();

        //}

        [HttpPost("CreateStoreProductField")]
        public async Task<ActionResult> CreateStoreProductField([FromBody] List<CategoryFieldsDto> data)
        {
            if (data == null || !data.Any())
            {
                return BadRequest("No data received.");
            }

            var storeProductFieldList = new List<StoreProductField>();

            foreach (var categoryFields in data)
            {
                foreach (var field in categoryFields.Fields)
                {
                    storeProductFieldList.Add(new StoreProductField
                    {
                        StoreCategoryID = categoryFields.StoreCategoryID,
                        CategoryID = categoryFields.CategoryID,
                        EntityName = field,
                        IsActive = true
                    });
                    bool isCreated = await _storeService.CreateStoreProductFieldAsync(storeProductFieldList);
                    if (!isCreated)
                    {
                        return BadRequest("Invalid data.");
                    }
                }
            }

            return Ok(new { Message = "Successfully Createed", Success = true });
        }

        [HttpGet("GetStoreProductFieldBySotreId/{id}")]
        public async Task<ActionResult<StoreConfig>> GetStoreProductFieldBySotreId(Guid id)
        {
            var allData = await _storeService.GetStoreProductFieldsByStoreId(id);

            if (allData == null)
            {
                return NotFound();
            }

            return Ok(allData);
        }

        [HttpDelete("DeleteStoreProductField/{id}")]
        public async Task<ActionResult> DeleteStoreProductField(Guid id)
        {
            try
            {
                await _storeService.DeleteStoreProductFieldAsync(id);
                return Ok(new { Message = "Successfully Deleted", Success = true });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting a store.");
                return StatusCode(500, ex.Message);
            }
        }
    }
}
