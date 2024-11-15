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

        public async Task<bool> UpdateAsync(Store store)
        {
            try
            {
                
                // Find the existing user by UserId
                var existingStore = await _context.Stores.FirstOrDefaultAsync(s => s.StoreID == store.StoreID);

                if (existingStore == null)
                {
                    return false; // Return false if the user is not found
                }

                // Update the UserName
                existingStore.Description = store.Description;
                existingStore.StoreName = store.StoreName;
                existingStore.ModifiedDate = DateTime.Now;
                existingStore.IsActive = true;

                // Save the changes to the database
                _context.Stores.Update(existingStore);
                await _context.SaveChangesAsync();

                return true; // Return true to indicate succe
            }
            catch (Exception ex)
            {
               _logger.LogError(ex, "An error occurred while updating a store.");
                return false;
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
                // Create Store Brands
                foreach (var storeBrand in storeConfig.StoreBrands)
                {
                    storeBrand.StoreBrandID = Guid.NewGuid();
                    storeBrand.StoreID = storeConfig.StoreID;
                    storeBrand.IsActive = true;

                    _context.StoreBrands.Add(storeBrand);
                    await _context.SaveChangesAsync();
                }

                // Create Store Categories
                foreach (var storeCategory in storeConfig.StoreCategories)
                {
                    storeCategory.StoreCategoryID = Guid.NewGuid();
                    storeCategory.StoreID = storeConfig.StoreID;
                    storeCategory.IsActive = true;

                    _context.StoreCategories.Add(storeCategory);
                    await _context.SaveChangesAsync();
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
                    await _context.SaveChangesAsync();
                }


                return true;
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "An error occurred while adding a store Config.");
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


    }

}
