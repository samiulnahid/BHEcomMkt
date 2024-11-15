using BHEcom.Common.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHEcom.Data.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ProductRepository> _logger;
        public ProductRepository(ApplicationDbContext context, ILogger<ProductRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Guid> AddAsync(Product product)
        {
            try
            {
                await _context.Products.AddAsync(product);
                await _context.SaveChangesAsync();
                return product.ProductID;
            }
            catch (Exception ex)
            {
                // Handle the exception as needed
               _logger.LogError(ex, "An error occurred while adding a product.");
                return Guid.Empty;
            }
        }



        public async Task<Product> GetByIdAsync(Guid id)
        {
            var result = await (from product in _context.Products
                                join seller in _context.Agents
                                on product.SellerID equals seller.AgentID
                                join caregory in _context.Categories
                                on product.CategoryID equals caregory.CategoryID
                                join brand in _context.Brands
                                on product.BrandID equals brand.BrandID
                                join store in _context.Stores
                                on product.StoreID equals store.StoreID
                                where product.ProductID == id
                                select new Product
                                {
                                    ProductID = product.ProductID,
                                    StoreID = product.StoreID,
                                    CategoryID = product.CategoryID,
                                    BrandID = product.BrandID,
                                    SellerID = product.SellerID,
                                    ProductName = product.ProductName,
                                    Description = product.Description,
                                    Stock = product.Stock,
                                    CreatedDate = product.CreatedDate,
                                    ModifiedDate = product.ModifiedDate,
                                    IsActive = product.IsActive,
                                    //SellerName = seller.AgencyName,
                                    //OwnerID = store.OwnerID,
                                    //CategoryName = caregory.CategoryName,
                                    //BrandName = brand.BrandName,
                                }).FirstOrDefaultAsync(); 

            return result;
            //return await _context.Products.FindAsync(id);
        }
        public async Task<IEnumerable<Product>> GetByCategoryIdAsync(Guid id)
        {
            var result = await (from product in _context.Products
                                join seller in _context.Agents
                                on product.SellerID equals seller.AgentID
                                join caregory in _context.Categories
                                on product.CategoryID equals caregory.CategoryID
                                join brand in _context.Brands
                                on product.BrandID equals brand.BrandID
                                join store in _context.Stores
                                on product.StoreID equals store.StoreID
                                where product.CategoryID == id
                                select new Product
                                {
                                    ProductID = product.ProductID,
                                    StoreID = product.StoreID,
                                    CategoryID = product.CategoryID,
                                    BrandID = product.BrandID,
                                    SellerID = product.SellerID,
                                    ProductName = product.ProductName,
                                    Description = product.Description,
                                    Stock = product.Stock,
                                    CreatedDate = product.CreatedDate,
                                    ModifiedDate = product.ModifiedDate,
                                    IsActive = product.IsActive,
                                    //SellerName = seller.AgencyName,
                                    //OwnerID = store.OwnerID,
                                    //CategoryName = caregory.CategoryName,
                                    //BrandName = brand.BrandName,
                                }).ToListAsync();

            return result;
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {

            return await _context.Products
                        .Where(product => product.IsActive)
                        .ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetAllByStoreIdAsync(Guid id)
        {

            var result = await (from product in _context.Products
                                join seller in _context.Agents
                                on product.SellerID equals seller.AgentID
                                join caregory in _context.Categories
                                on product.CategoryID equals caregory.CategoryID
                                join brand in _context.Brands
                                on product.BrandID equals brand.BrandID
                                join store in _context.Stores
                                on product.StoreID equals store.StoreID
                                where product.StoreID == id
                                select new Product
                                {
                                    ProductID = product.ProductID,
                                    StoreID = product.StoreID,
                                    CategoryID = product.CategoryID,
                                    BrandID = product.BrandID,
                                    SellerID = product.SellerID,
                                    ProductName = product.ProductName,
                                    Description = product.Description,
                                    Stock = product.Stock,
                                    CreatedDate = product.CreatedDate,
                                    ModifiedDate = product.ModifiedDate,
                                    IsActive = product.IsActive,
                                    //SellerName = seller.AgencyName,
                                    //OwnerID = store.OwnerID,
                                    //CategoryName = caregory.CategoryName,
                                    //BrandName = brand.BrandName,
                                }).ToListAsync();

            return result;
            //return await _context.Products.ToListAsync();
        }


        public async Task<bool> UpdateAsync(Product product)
        {
            try
            {

                // Find the existing user by UserId
                var existingProduct = await _context.Products.FirstOrDefaultAsync(p => p.ProductID == product.ProductID);

                if (existingProduct == null)
                {
                    return false; // Return false if the user is not found
                }

                // Update the UserName
                existingProduct.Description = product.Description;
                existingProduct.ShortDescription = product.ShortDescription;
                existingProduct.ModifiedDate = DateTime.Now;
                existingProduct.IsActive = true;

                _context.Products.Update(existingProduct);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                // Handle the exception as needed
               _logger.LogError(ex, "An error occurred while updating a product.");
                return false;
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Product>> GetRandomProductsAsync(int num)
        {
            return await _context.Products
            .Where(p => p.IsActive)
            .OrderBy(r => Guid.NewGuid())
            .Take(num)
            .ToListAsync();
        }

      


    }
}
