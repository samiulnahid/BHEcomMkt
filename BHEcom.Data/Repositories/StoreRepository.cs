using BHEcom.Common.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Security.Cryptography.X509Certificates;
namespace BHEcom.Data.Repositories
{
    public class StoreRepository : IStoreRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<StoreRepository> _logger;

        public StoreRepository(ApplicationDbContext context, ILogger<StoreRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Guid> AddAsync(Store store)
        {
            try
            {
                await _context.Stores.AddAsync(store);
                await _context.SaveChangesAsync();
                return store.StoreID;
            }
            catch (Exception ex)
            {

               _logger.LogError(ex, "An error occurred while adding a store.");
                return Guid.Empty;
            }
        }

        public async Task<Store> GetByIdAsync(Guid id)
        {
            return await _context.Stores
                        .FirstOrDefaultAsync(store => store.StoreID == id);
           
        }

        public async Task<IEnumerable<Store>> GetAllStoresAsync()
        {
            return await _context.Stores.ToListAsync();
        }

        public async Task<(bool isUpdated, string? oldImageUrl)> UpdateAsync(Store store)
        {
            try
            {
                string? oldImage = string.Empty;
                // Find the existing user by UserId
                var existingStore = await _context.Stores.FirstOrDefaultAsync(s => s.StoreID == store.StoreID);

                if (existingStore == null)
                {
                    return (false, oldImage);
                }

                // Update the UserName
                existingStore.Description = store.Description;
                //existingStore.Image = store.Image;
                existingStore.StoreName = store.StoreName;
                existingStore.ModifiedDate = DateTime.Now;
                existingStore.IsActive = true;

                if (store.Image != null)
                {
                    oldImage = existingStore.Image;
                    existingStore.Image = store.Image;
                }

                // Save the changes to the database
                _context.Stores.Update(existingStore);
                await _context.SaveChangesAsync();

                return (true, oldImage);
            }
            catch (Exception ex)
            {
               _logger.LogError(ex, "An error occurred while updating a store.");
                return (false, null) ;
            }
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var store = await _context.Stores.FindAsync(id);
            if (store != null)
            {
                _context.Stores.Remove(store);
                int affectedRows = await _context.SaveChangesAsync();
                return (affectedRows > 0);
            }
            else
                return false;
        }

        public async Task<StoreManager> GetStoreManagerByStoreIdAsync(Guid storeId)
        {
            var result = await (from store in _context.Stores
                                join agent in _context.Agents on store.OwnerID equals agent.AgentID
                                join user in _context.Users on agent.UserID equals user.UserId
                                join address in _context.Addresses on user.UserId equals address.UserID
                                where store.StoreID == storeId
                                select new StoreManager
                                {
                                    // Store Fields
                                    StoreID = store.StoreID,
                                    StoreName = store.StoreName,
                                    Description = store.Description,
                                    OwnerID = store.OwnerID,
                                    Image = store.Image,

                                    // Agent Fields
                                    AgentID = agent.AgentID,
                                    AgencyName = agent.AgencyName,
                                    ContactPerson = agent.ContactPerson,
                                    ContactEmail = agent.ContactEmail,
                                    ContactPhone = agent.ContactPhone,

                                    // Address Fields
                                    AddressID = address.AddressID,
                                    AddressLine = address.AddressLine1,
                                    City = address.City,
                                    State = address.State,
                                    ZipCode = address.ZipCode,
                                    Country = address.Country,

                                    // User Fields
                                    UserId = user.UserId,
                                    UserName = user.UserName
                                }).FirstOrDefaultAsync();
           
            return result;
        }

        public async Task<IEnumerable<StoreManager>> GetAllStoreManagersAsync()
        {
            var result = await (from store in _context.Stores
                                join agent in _context.Agents on store.OwnerID equals agent.AgentID
                                join user in _context.Users on agent.UserID equals user.UserId
                                join address in _context.Addresses on user.UserId equals address.UserID
                                select new StoreManager
                                {
                                    // Store Fields
                                    StoreID = store.StoreID,
                                    StoreName = store.StoreName,
                                    Description = store.Description,
                                    OwnerID = store.OwnerID,
                                    Image = store.Image,

                                    // Agent Fields
                                    AgentID = agent.AgentID,
                                    AgencyName = agent.AgencyName,
                                    ContactPerson = agent.ContactPerson,
                                    ContactEmail = agent.ContactEmail,
                                    ContactPhone = agent.ContactPhone,

                                    // Address Fields
                                    AddressID = address.AddressID,
                                    AddressLine = address.AddressLine1,
                                    City = address.City,
                                    State = address.State,
                                    ZipCode = address.ZipCode,
                                    Country = address.Country,

                                    // User Fields
                                    UserId = user.UserId,
                                    UserName = user.UserName
                                }).ToListAsync();

            return result;
        }

        public async Task<bool> CreateBrandAndCategoryByStoreIdAsync(StoreConfig storeConfig)
        {
            try
            {
                if (storeConfig.StoreBrands != null)
                { 
                    // Create Store Brands
                    foreach (var storeBrand in storeConfig.StoreBrands)
                    {

                        storeBrand.StoreBrandID = Guid.NewGuid();
                        storeBrand.StoreID = storeConfig.StoreID;
                        storeBrand.IsActive = true;
                        _context.StoreBrands.Add(storeBrand);
                        await _context.SaveChangesAsync();

                    }

                }
             
                if(storeConfig.StoreCategories != null)
                {
                    // Create Store Categories
                    foreach (var storeCategory in storeConfig.StoreCategories)
                    {
                        if (storeCategory.StoreCategoryID == Guid.Empty)
                        {
                            storeCategory.StoreCategoryID = Guid.NewGuid();
                            storeCategory.StoreID = storeConfig.StoreID;
                            storeCategory.IsActive = true;

                            _context.StoreCategories.Add(storeCategory);
                            await _context.SaveChangesAsync();
                        }
                       
                    }
                }
                

                return true;
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "An error occurred while adding a store Config.");
                return false;
            }
        }

        //public async Task<StoreConfig> GetBrandAndCategoryByStoreIdAsync(Guid storeId)
        //{
        //    var storeBrands = await _context.StoreBrands
        //   .Where(b => b.StoreID == storeId)
        //   .Select(b => new StoreBrand
        //   {
        //       BrandID = b.BrandID,
        //       IsActive = b.IsActive
        //   })
        //   .ToListAsync();

        //    // Get Store Categories filtered by StoreID
        //    var StoreCategories = await _context.StoreCategories
        //        .Where(c => c.StoreID == storeId)
        //        .Select(c => new StoreCategory
        //        {
        //            CategoryID = c.CategoryID,
        //            IsActive = c.IsActive
        //        })
        //        .ToListAsync();

        //    // Return data if both are available, otherwise return null
        //    if (storeBrands.Any() || StoreCategories.Any())
        //    {
        //        return new StoreConfig
        //        {
        //            StoreID = storeId,
        //            StoreBrands = storeBrands,
        //            StoreCategories = StoreCategories
        //        };
        //    }

        //    return null; // Return null if no brands or categories found
        //}


        public async Task<StoreConfig> GetBrandAndCategoryByStoreIdAsync(Guid storeId)
        {
            // Get Store Brands filtered by StoreID, including BrandName
            var storeBrands = await _context.StoreBrands
                .Where(b => b.StoreID == storeId)
                .Join(_context.Brands,
                      sb => sb.BrandID,
                      b => b.BrandID,
                      (sb, b) => new StoreBrand
                      {
                          StoreBrandID = sb.StoreBrandID, // Ensure to include this if needed
                          StoreID = storeId,
                          BrandID = b.BrandID,
                          BrandName = b.BrandName, // Getting BrandName from Brands table
                          IsActive = sb.IsActive
                      })
                .ToListAsync();

            // Get Store Categories filtered by StoreID, including CategoryName
            var storeCategories = await _context.StoreCategories
                .Where(c => c.StoreID == storeId)
                .Join(_context.Categories,
                      sc => sc.CategoryID,
                      c => c.CategoryID,
                      (sc, c) => new StoreCategory
                      {
                          StoreCategoryID = sc.StoreCategoryID, // Include if needed
                          StoreID = storeId,
                          CategoryID = c.CategoryID,
                          CategoryName = c.CategoryName, // Getting CategoryName from Categories table
                          IsActive = sc.IsActive
                      })
                .ToListAsync();

            // Return data if either brands or categories are available, otherwise return null
            if (storeBrands.Any() || storeCategories.Any())
            {
                return new StoreConfig
                {
                    StoreID = storeId,
                    StoreBrands = storeBrands,
                    StoreCategories = storeCategories
                };
            }

            return null; // Return null if no brands or categories found
        }


        public async Task<bool> DeletetBrandAndCategoryByStoreIdAsync(Guid storeId) {

            try
            {
                // Get all StoreBrands by StoreID
                var storeBrands = await _context.StoreBrands
                    .Where(b => b.StoreID == storeId)
                    .ToListAsync();

                // Get all StoreCategories by StoreID
                var storeCategories = await _context.StoreCategories
                    .Where(c => c.StoreID == storeId)
                    .ToListAsync();

                // Remove StoreBrands if found
                if (storeBrands.Any())
                {
                    _context.StoreBrands.RemoveRange(storeBrands);
                }

                // Remove StoreCategories if found
                if (storeCategories.Any())
                {
                    _context.StoreCategories.RemoveRange(storeCategories);
                }

                // Save changes to the database if any records were removed
                if (storeBrands.Any() || storeCategories.Any())
                {
                    await _context.SaveChangesAsync();
                }

                return true;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "An error occurred while Deleteing store Config.");
                return false;
            }
        }

        public async Task<bool> CreateProductFieldAsync(List<StoreProductField> productFieldList)
        {
            try
            {
                // Create Store Brands
                foreach (var field in productFieldList)
                {
                    field.ProductFieldID = Guid.NewGuid();
                    field.IsActive = true;

                    _context.StoreProductFields.Add(field);
                    
                }

                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "An error occurred while adding a store Config.");
                return false;
            }
        }

        public async Task<bool> UpdateProductFieldsAsync(List<StoreProductField> productFieldList)
        {
            try
            {
                bool isFound = true;
                foreach (var updatedField in productFieldList)
                {
                    // Fetch the existing entity from the database
                    var existingField = await _context.StoreProductFields
                        .FirstOrDefaultAsync(f => f.ProductFieldID == updatedField.ProductFieldID);

                    if (existingField != null)
                    {
                        // Update properties
                        existingField.EntityName = updatedField.EntityName;
                        existingField.IsActive = true;
                    }
                    else
                    {
                        _logger.LogWarning($"ProductFieldID {updatedField.ProductFieldID} not found.");
                        isFound = false;
                    }
                }

                // Save all changes
                await _context.SaveChangesAsync();
                if (!isFound)
                    return false;
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating product fields.");
                return false;
            }
        }


        public async Task<List<StoreProductField>> GetProductFieldsByStoreId(Guid id)
        {
            return await (from spf in _context.StoreProductFields
                          join sc in _context.StoreCategories on spf.StoreCategoryID equals sc.StoreCategoryID
                          where sc.StoreID == id
                          select spf)
                  .ToListAsync();
        } 
        public async Task<List<StoreProductField>> GetProductFieldsByCategoryId(Guid id)
        {
            return await (from spf in _context.StoreProductFields
                          join sc in _context.StoreCategories on spf.StoreCategoryID equals sc.StoreCategoryID
                          where sc.CategoryID == id
                          select spf)
                  .ToListAsync();
        }

        public async Task<bool> DeleteStoreProductField(Guid id)
        {
            var field = await _context.StoreProductFields.FindAsync(id);
            if (field != null)
            {
                _context.StoreProductFields.Remove(field);
                int affectedRows = await _context.SaveChangesAsync();
                return (affectedRows > 0);
            }
            else
                return false;
            
        }

        public async Task<(bool Success, string Message, List<Guid>? CategoryIds)> DeleteStoreBrandandStoreCategoryAsync(StoreConfig storeConfig)
        {
            bool isDeleteAny = false;
            var categoriesWithFields = new List<Guid>();
            var categoriesNameWithFields = new List<String>();

            if (storeConfig.StoreCategories?.Any() == true)
            {
                foreach (var storeCategory in storeConfig.StoreCategories)
                {
                    
                    var field = await _context.StoreProductFields
                                      .FirstOrDefaultAsync(s => s.StoreCategoryID == storeCategory.StoreCategoryID);

                    if (field != null)
                    {
                        // Add the StoreCategoryID to the list
                        categoriesWithFields.Add(storeCategory.StoreCategoryID);
                        categoriesNameWithFields.Add(storeCategory.CategoryName ?? string.Empty);
                    }

                    else
                    {
                        var ExsitingCategory = await _context.StoreCategories.FindAsync(storeCategory.StoreCategoryID);
                        if (ExsitingCategory != null)
                        {
                            _context.StoreCategories.Remove(ExsitingCategory);
                            isDeleteAny = true;
                        }
                            
                    }
                    
                }
            }

            // If there are categories with fields, return early with a message
            if (categoriesWithFields.Any())
            {
                string formattedCategoryNames = string.Join(", ", categoriesNameWithFields);
                return (false, $"{formattedCategoryNames} StoreCategories have related fields in StoreProductFields.", categoriesWithFields);
            }


            // Get all StoreBrands by StoreID
            var storeBrands = await _context.StoreBrands
                .Where(b => b.StoreID == storeConfig.StoreID)
                .ToListAsync();


            // Remove StoreBrands if found
            if (storeBrands.Any())
            {
                _context.StoreBrands.RemoveRange(storeBrands);
                isDeleteAny = true;
            }

            //var field = await _context.StoreProductFields.FindAsync(id);
            if (isDeleteAny)
            {
                await _context.SaveChangesAsync();
                return (true, "Deletion successful.", null);
            }
            else
                return (false, "No deletions were made.", null);

        }


    }

}
