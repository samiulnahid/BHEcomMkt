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
using System.Diagnostics.Metrics;
using System.Reflection.Emit;
namespace BHEcom.Data.Repositories
{
    public class AddressRepository : IAddressRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<AddressRepository> _logger;

        public AddressRepository(ApplicationDbContext context, ILogger<AddressRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<bool> AddAsync(Address address)
        {
            try
            {
                await _context.Addresses.AddAsync(address);
                int affectedRows = await _context.SaveChangesAsync();
                return affectedRows > 0;
            }
            catch (Exception ex)
            {

               _logger.LogError(ex, "An error occurred while adding a address.");
                return false;
            }
        }

        public async Task<Address> GetByIdAsync(Guid id)
        {
            var result = await (from address in _context.Addresses
                                join user in _context.Users
                                on address.UserID equals user.UserId into addressUserGroup
                                from user in addressUserGroup.DefaultIfEmpty() // Left join to handle missing users
                                where address.AddressID == id
                                select new Address
                                {
                                    AddressID = address.AddressID,
                                    UserID = address.UserID,
                                    AddressType = address.AddressType,
                                    AddressLine1 = address.AddressLine1,
                                    AddressLine2 = address.AddressLine2 ?? "N/A", // Handle null AddressLine2
                                    City = address.City,
                                    State = address.State,
                                    ZipCode = address.ZipCode,
                                    Country = address.Country,
                                    UserName = user != null ? user.UserName : "Unknown User", // Handle null UserName
                                }).FirstOrDefaultAsync();

            return result;
        }
         public async Task<Address> GetByUserIdAsync(Guid id)
        {
            var result = await (from address in _context.Addresses
                                join user in _context.Users
                                on address.UserID equals user.UserId into addressUserGroup
                                from user in addressUserGroup.DefaultIfEmpty() 
                                where address.UserID == id
                                select new Address
                                {
                                    AddressID = address.AddressID,
                                    UserID = address.UserID,
                                    AddressType = address.AddressType,
                                    AddressLine1 = address.AddressLine1,
                                    AddressLine2 = address.AddressLine2 ?? "N/A", 
                                    City = address.City,
                                    State = address.State,
                                    ZipCode = address.ZipCode,
                                    Country = address.Country,
                                    UserName = user != null ? user.UserName : "Unknown User", 
                                }).FirstOrDefaultAsync();

            return result;
        }

        public async Task<IEnumerable<Address>> GetAllAsync()
        {
            var result = await (from address in _context.Addresses
                                join user in _context.Users
                                on address.UserID equals user.UserId
                                select new Address
                                {
                                    AddressID = address.AddressID,
                                    UserID = address.UserID,
                                    AddressType = address.AddressType,
                                    AddressLine1 = address.AddressLine1,
                                    AddressLine2 = address.AddressLine2,
                                    City = address.City,
                                    State = address.State,
                                    ZipCode = address.ZipCode,
                                    Country = address.Country,
                                    UserName = user.UserName,
                                }).ToListAsync();

            return result;
            //return await _context.Addresses.ToListAsync();
        }


        //public async Task<bool> UpdateAsync(Address address)
        //{
        //    try
        //    {
        //        // Check if the address exists in the database
        //        var existingAddress = await _context.Addresses
        //                                            .AsNoTracking() // To ensure EF doesn't track the original entity
        //                                            .FirstOrDefaultAsync(a => a.AddressID == address.AddressID);

        //        if (existingAddress == null)
        //        {
        //            _logger.LogError($"Address with ID {address.AddressID} not found.");
        //            return false; // Return false if the address does not exist
        //        }

        //        // Update the address
        //        _context.Addresses.Update(address);
        //        int affectedRows = await _context.SaveChangesAsync();

        //        return (affectedRows > 0); // Return true if rows were affected (update succeeded)
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "An error occurred while updating the address.");
        //        return false; // Return false in case of an exception
        //    }
        //}

        public async Task<bool> UpdateAsync(Address address)
        {
            if (address?.AddressID == null)
            {
                _logger.LogError("Address ID is null.");
                return false;
            }

            try
            {
                // Check if the address exists in the database
                var existingAddress = await _context.Addresses
                                                    .AsNoTracking()
                                                    .FirstOrDefaultAsync(a => a.AddressID == address.AddressID);

                if (existingAddress == null)
                {
                    _logger.LogError($"Address with ID {address.AddressID} not found.");
                    return false;
                }

                existingAddress.AddressLine1 = address.AddressLine1;
                existingAddress.City = address.City;
                existingAddress.State = address.State;
                existingAddress.ZipCode = address.ZipCode;
                existingAddress.Country = address.Country;  

                // Check if nullable fields are properly handled
                existingAddress.AddressType = address.AddressType ?? existingAddress.AddressType;
                existingAddress.AddressLine2 = address.AddressLine2 ?? existingAddress.AddressLine2;

                // Update the address
                _context.Addresses.Update(existingAddress);
                int affectedRows = await _context.SaveChangesAsync();

                return affectedRows > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the address.");
                return false;
            }
        }



        public async Task<bool> DeleteAsync(Guid id)
        {
            var address = await _context.Addresses.FindAsync(id);
            if (address != null)
            {
                _context.Addresses.Remove(address);
                int affectedRows = await _context.SaveChangesAsync();
                return (affectedRows > 0);
            }
            else
                return false;
        }
    }

}
