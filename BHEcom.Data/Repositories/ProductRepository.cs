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

                throw;
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
            .Where(p => p.IsActive && p.Stock > 0)
            .OrderBy(r => Guid.NewGuid())
            .Take(num)
            .ToListAsync();
        }
        public async Task<IEnumerable<Product>> GetLatestProductsAsync(int num)
        {
            return await _context.Products
                .Where(p => p.IsActive) // Filter for active products with stock > 0
                .OrderByDescending(p => p.CreatedDate) // Order by CreatedDate in descending order (latest first)
                .Take(num) // Take the specified number of products
                .ToListAsync();
        }


        public async Task<(IEnumerable<Product>? Products, int? TotalCount)> FilterAllProduct(ProductFilter filterEntity)
        {
            try
            {
                var query = _context.Products.AsQueryable();

                // Include only active products
                query = query.Where(p => p.IsActive);

                // Filter by Brand
                if (filterEntity.BrandId.HasValue)
                {
                    query = query.Where(p => p.BrandID == filterEntity.BrandId.Value);
                }

                // Filter by Category
                if (filterEntity.CategoryId.HasValue)
                {
                    query = query.Where(p => p.CategoryID == filterEntity.CategoryId.Value);
                }

                // Filter by Price Range
                if (filterEntity.MinPrice.HasValue)
                {
                    query = query.Where(p => p.Price >= filterEntity.MinPrice.Value);
                }
                if (filterEntity.MaxPrice.HasValue)
                {
                    query = query.Where(p => p.Price <= filterEntity.MaxPrice.Value);
                }


                //// Sort by Price (Low to High or High to Low)
                //if (filterEntity.LowToHigh.HasValue)
                //{
                //    query = filterEntity.LowToHigh.Value
                //        ? query.OrderBy(p => p.Price)
                //        : query.OrderByDescending(p => p.Price);
                //}

                // Sort based on selected options
                if (filterEntity.LowToHigh.HasValue && filterEntity.LowToHigh.Value)
                {
                    query = query.OrderBy(p => p.Price);
                }
                else if (filterEntity.HighToLow.HasValue && filterEntity.HighToLow.Value)
                {
                    query = query.OrderByDescending(p => p.Price);
                }

                // Sort by CreatedDate (Latest to Old or Old to Latest)
                if (filterEntity.LatestToOld.HasValue)
                {
                    query = filterEntity.LatestToOld.Value
                        ? query.OrderByDescending(p => p.CreatedDate)
                        : query.OrderBy(p => p.CreatedDate);
                }

                // Get the total count of filtered products (before pagination)
                int totalCount = await query.CountAsync();

                // If no products found, return an empty list with total count zero
                if (totalCount == 0)
                {
                    return (new List<Product>(), 0);
                }

                // Apply pagination
                var products = await query.Skip((filterEntity.Page - 1) * filterEntity.PageSize)
                                           .Take(filterEntity.PageSize)
                                           .ToListAsync();

                return (Products: products, TotalCount: totalCount);

            }
            catch (Exception ex)
            {
                // Log the exception (consider using a logging framework here)
                Console.WriteLine($"Error in GetFilteredProducts: {ex.Message}");

                // Return an error result with the exception message
                return (Products: null, TotalCount: null);
            }
        }


    }
}
