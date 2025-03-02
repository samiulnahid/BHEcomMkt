using BHEcom.Common.Helper;
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
        private readonly FtpUploader _ftpUploader;
        public StoreController(IStoreService sotreService, ILogger<StoreRepository> logger, IAddressService addressService, IAgentService agentService, IAdminService adminService, FtpUploader ftpUploader)
        {
            _storeService = sotreService;
            _addressService = addressService;
            _agentService = agentService;
            _adminService = adminService;
            _logger = logger;
            _ftpUploader = ftpUploader;
        }

        [HttpPost("Create")]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult> Create([FromForm] StoreManager stores, [FromForm] IFormFile imageFile)
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
                        Number = stores.ContactPhone,
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

                        if (imageFile != null && imageFile.Length > 0)
                        {
                            string folderName = "ecom/store"; 
                            string imageUrl = await _ftpUploader.UploadFileAsync(imageFile, folderName);

                            // Assign the uploaded image URL to the product
                            store.Image = imageUrl;
                        }

                        store.OwnerID = agentId;
                        store.StoreName = stores.StoreName;
                        store.Description = stores.Description;
                        store.CreatedDate = DateTime.Now;
                        store.IsActive = true;

                        storeId = await _storeService.AddStoreAsync(store);
                    }
                    var Data = new
                    {
                        StoreId = storeId,
                        UserId = userId,
                        AddressId = addressId,
                        AgentId = agentId,
                    };
                    if (storeId != Guid.Empty)
                    {
                        return Ok(new { data = Data, Success = true });
                    }
                }
                return BadRequest(new { Success = false, Message = "Failed to add store." });
                //return CreatedAtAction(nameof(GetById), new { id = store.StoreID }, store);

            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "An error occurred while adding a store.");
                return StatusCode(500, new { Message = ex.Message, Success = false });
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
                    return Ok(new { data = store, Message = "Data not found!", Success = true });
                }
                return Ok(new { data = store, Success = true });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while getById store.");
                return StatusCode(500, new { Message = ex.Message, Success = false });
            }
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<StoreManager>>> GetAll()
        {
            try
            {
                var stores = await _storeService.GetAllStoreManagersAsync();
                return Ok(new { data = stores, Success = true });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while get all sotre.");
                return StatusCode(500, new { Message = ex.Message, Success = false });
            }
        }

        [HttpPut("Update/{id}")]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult> Update(Guid id, [FromForm] StoreManager stores, [FromForm] IFormFile? imageFile)
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

                        if (imageFile != null && imageFile.Length > 0)
                        {
                            string folderName = "ecom/store";
                            string imageUrl = await _ftpUploader.UploadFileAsync(imageFile, folderName);

                            // Assign the uploaded image URL to the product
                            store.Image = imageUrl;
                        }

                        store.StoreID = stores.StoreID;
                        store.StoreName = stores.StoreName;
                        store.Description = stores.Description;
                        store.IsActive = true;

                        var (isUpdated, oldImageUrl) = await _storeService.UpdateStoreAsync(store);

                        if (!isUpdated)
                        {
                            return StatusCode(500, new { Message = "Unsuccessfully Updated", Success = false });
                        }
                        if (!string.IsNullOrEmpty(oldImageUrl))
                        {
                            // Image delete code
                            _ftpUploader.DeleteFile(oldImageUrl);
                        }
                        var Data = new
                        {
                            storeId = store.StoreID,
                            userId = user.UserId,
                            addressId = address.AddressID,
                            agentId = agent.AgentID,
                        };
                        return Ok(new { data = Data, Success = true, Message = "Successfully Updated" });
                    }
                  
                }
                return Ok(new {  Success = false, Message = "Update Unsuccessful!" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating a store.");
                return StatusCode(500, new { Message = ex.Message, Success = false });
            }
        }

        // [HttpPut("UpdateTest/{id}")]
        //public async Task<ActionResult> UpdateTest(Guid id, [FromBody] StoreManager stores)
        //{
        //    try
        //    {

        //        if (id != stores.StoreID)
        //        {
        //            return BadRequest();
        //        }

        //        User user = new User
        //        {
        //            UserId = stores.UserId,
        //            UserName = stores.ContactPhone,
        //            LoweredUserName = stores.ContactPhone,
        //            LastActivityDate = DateTime.Now,
        //        };
        //        bool isUserUpdate = await _adminService.UpdateUserNameAsync(user);

        //        Store store = new Store();
        //        if (isUserUpdate)
        //        {

        //            Address address = new Address
        //            {
        //                AddressID = stores.AddressID,
        //                UserID = stores.UserId,
        //                AddressLine1 = stores.AddressLine,
        //                City = stores.City,
        //                State = stores.State,
        //                ZipCode = stores.ZipCode,
        //                Country = stores.Country,
        //            };
        //            bool isAddressUpdated = await _addressService.UpdateAddressAsync(address);

        //            Agent agent = new Agent
        //            {
        //                AgentID = stores.OwnerID,
        //                UserID = stores.UserId,
        //                AgencyName = stores.AgencyName,
        //                ContactPerson = stores.ContactPerson,
        //                ContactEmail = stores.ContactEmail,
        //                ContactPhone = stores.ContactPhone,
        //            };
        //            bool isAgentUpdated = await _agentService.UpdateAgentAsync(agent);

        //            if (isAgentUpdated)
        //            {

        //                //if (imageFile != null && imageFile.Length > 0)
        //                //{
        //                //    string folderName = "ecom/store";
        //                //    string imageUrl = await _ftpUploader.UploadFileAsync(imageFile, folderName);

        //                //    // Assign the uploaded image URL to the product
        //                //    store.Image = imageUrl;
        //                //}

        //                store.StoreID = stores.StoreID;
        //                store.StoreName = stores.StoreName;
        //                store.Description = stores.Description;
        //                store.IsActive = true;

        //                var (isUpdated, oldImageUrl) = await _storeService.UpdateStoreAsync(store);

        //                if (!isUpdated)
        //                {
        //                    return StatusCode(500, new { Message = "Unsuccessfully Updated", Success = false });
        //                }
        //                if (!string.IsNullOrEmpty(oldImageUrl))
        //                {
        //                    // Image delete code
        //                    _ftpUploader.DeleteFile(oldImageUrl);
        //                }

        //            }
        //        }
        //        return Ok(new { id = store.StoreID, Success = true, Message = "Succefully Updated" });
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "An error occurred while updating a store.");
        //        return StatusCode(500, new { Message = ex.Message, Success = false });
        //    }
        //}

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
                return StatusCode(500, new { Message = ex.Message, Success = false });
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
                return Ok(new { data = storeData, Message = "Data not found!", Success = true });
            }

            return Ok(new { data = storeData, Success = true });
        }

        [HttpPost("CreateAndUpdateStoreConfig2/{id}")]
        public async Task<ActionResult> CreateAndUpdateStoreConfig2(Guid id, [FromBody] StoreConfig storeConfig) 
        {

            if (storeConfig == null)
            {
                return BadRequest(new { Message = "Invalid data.", Success = false });
                
            }
            var storeData = await _storeService.GetStoreConfigAsync(id);

            if (storeData != null)
            {
                bool isDeleted = await _storeService.DeleteStoreConfigAsync(id);
                //if (!isDeleted)
                //{
                //    return BadRequest(new { Message = "Dalete Faild!.", Success = false });
                //};
            }

            bool isCreated = await _storeService.CreateStoreConfigAsync(storeConfig);
            if (!isCreated) {
                return Ok(new { Message = "Unsuccessful Oparetion", Success = false });
            }
            return Ok(new { Message = "Successfully Updated and Create", Success = true });
        }

        [HttpPost("CreateAndUpdateStoreConfig/{id}")]
        public async Task<ActionResult> CreateAndUpdateStoreConfig(Guid id, [FromBody] StoreConfig storeConfig) 
        {

            if (storeConfig == null)
            {
                return BadRequest(new { Message = "Invalid data.", Success = false });
                
            }
            var storeData = await _storeService.GetStoreConfigAsync(id);
            StoreConfig newConfig = new StoreConfig
            {
                StoreBrands = new List<StoreBrand>(),
                StoreCategories = new List<StoreCategory>()
            };
            newConfig.StoreID = storeConfig.StoreID;

            if (storeData != null)
            {

                if (storeConfig.StoreBrands?.Any() == true)
                {
                    
                    newConfig.StoreBrands.AddRange(storeConfig.StoreBrands);
                   
                }
               
                if (storeData.StoreCategories?.Any() == true)
                {
                    foreach (var item in storeData.StoreCategories)
                    {
                        // Check if the item exists in storeConfig.StoreCategories
                        var existsInConfig = storeConfig.StoreCategories?
                            .Any(sc => sc.StoreCategoryID == item.StoreCategoryID) ?? false;

                        // If it does not exist, add it to newConfig.StoreCategories
                        if (!existsInConfig)
                        {
                            newConfig.StoreCategories.Add(item);
                        }
                    }

                }

                //if (storeData?.StoreCategories?.Any() == true)
                //{
                //    // Add categories from storeData not present in storeConfig
                //    var missingCategories = storeData.StoreCategories
                //        .Where(item => storeConfig.StoreCategories?.All(sc => sc.StoreCategoryID != item.StoreCategoryID) ?? true);

                //    newConfig.StoreCategories.AddRange(missingCategories);
                //}


                if (newConfig.StoreCategories.Any() || newConfig.StoreBrands.Any())
                {
                   var ( Success,  Message, CategoryIds) = await _storeService.DeleteStoreBrandandStoreCategoryAsync(newConfig);

                    if (CategoryIds?.Any() == true)
                    {
                        return Ok(new {data = CategoryIds, Message, Success = false });
                    }
                }

            }

            bool isCreated = await _storeService.CreateStoreConfigAsync(storeConfig);
            if (!isCreated) {
                return Ok(new { Message = "Unsuccessful Oparetion", Success = false });
            }
            return Ok(new { Message = "Successfully Updated and Create", Success = true });
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
                        EntityName = field.Fields,
                        IsActive = true
                    });
                   
                }
                bool isCreated = await _storeService.CreateStoreProductFieldAsync(storeProductFieldList);
                if (!isCreated)
                {
                    return BadRequest("Invalid data.");
                }
            }

            return Ok(new { Message = "Successfully Createed", Success = true });
        }

         [HttpPost("CreateandUpdateStoreProductField")]
        public async Task<ActionResult> CreateandUpdateStoreProductField([FromBody] List<CategoryFieldsDto> data)
        {
            if (data == null || !data.Any())
            {
                return BadRequest(new { Message = "No data received.", Success = false });
            }

            var createProductFieldList = new List<StoreProductField>();
            var updateProductFieldList = new List<StoreProductField>();

            foreach (var categoryFields in data)
            {
                foreach (var field in categoryFields.Fields)
                {
                    if (field.ProductFieldID == Guid.Empty)
                    {
                        createProductFieldList.Add(new StoreProductField
                        {
                            StoreCategoryID = categoryFields.StoreCategoryID,
                            CategoryID = categoryFields.CategoryID,
                            EntityName = field.Fields,
                            IsActive = true
                        });
                    }
                    else
                    {
                        if (field.ProductFieldID != Guid.Empty)
                        {
                            Guid id = field.ProductFieldID;
                        }
                        updateProductFieldList.Add(new StoreProductField
                        {
                            
                            ProductFieldID = field.ProductFieldID,
                            StoreCategoryID = categoryFields.StoreCategoryID,
                            CategoryID = categoryFields.CategoryID,
                            EntityName = field.Fields,
                            IsActive = true
                        });
                    }
                   
                }
                if (createProductFieldList.Any())
                {
                    bool isCreated = await _storeService.CreateStoreProductFieldAsync(createProductFieldList);
                    if (!isCreated)
                    {
                        return BadRequest(new { Message = "Invalid data.", Success = false });
                    }
                }
               if (updateProductFieldList.Any())
                {
                    bool isUpdate= await _storeService.UpdateProductFieldsAsync(updateProductFieldList);
                    if (!isUpdate)
                    {
                        return BadRequest(new { Message = "Invalid data or Store ProductFields Id not Found", Success = false });
                    }
                }
               
            }

            return Ok(new { Message = "Successfully Create And Update", Success = true });
        }

        [HttpGet("GetStoreProductFieldBySotreId/{id}")]
        public async Task<ActionResult<StoreProductField>> GetStoreProductFieldBySotreId(Guid id)
        {
            var allData = await _storeService.GetStoreProductFieldsByStoreId(id);

            if (allData == null)
            {
                return Ok(new { data = allData,Message = "Data not found!", Success = true });
            }
            return Ok(new { data = allData, Success = true });
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
                return StatusCode(500, new { Message = ex.Message, Success = false });
            }
        }
    }
}
