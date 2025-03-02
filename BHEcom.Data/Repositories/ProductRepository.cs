using BHEcom.Common.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
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
                                    ProductName = product.ProductName,
                                    Description = product.Description,
                                    Price = product.Price,
                                    Stock = product.Stock,
                                    ShortDescription = product.ShortDescription,
                                    CreatedDate = product.CreatedDate,
                                    ModifiedDate = product.ModifiedDate,
                                    IsActive = product.IsActive,
                                    StoreName = store.StoreName,
                                    CategoryName = caregory.CategoryName,
                                    BrandName = brand.BrandName,
                                    Image = product.Image
                                }).FirstOrDefaultAsync(); 

            return result;
            //return await _context.Products.FindAsync(id);
        }
        public async Task<(IEnumerable<Product> Products, int TotalCount)> GetByCategoryIdAsync(Guid id, int pageNumber, int pageSize)
        {
            int totalCount = 0;
           // Calculate the total count of products in the category
           totalCount = await _context.Products.CountAsync(p => p.CategoryID == id);

            var result = await (from product in _context.Products
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
                                    ProductName = product.ProductName,
                                    Description = product.Description,
                                    Stock = product.Stock,
                                    CreatedDate = product.CreatedDate,
                                    ModifiedDate = product.ModifiedDate,
                                    IsActive = product.IsActive,
                                    Image = product.Image,
                                    Price = product.Price,
                                    ShortDescription = product.ShortDescription,
                                    Discount = product.Discount,
                                    StoreName = store.StoreName,
                                    CategoryName = caregory.CategoryName,
                                    BrandName = brand.BrandName,
                                })
                                .Skip((pageNumber - 1) * pageSize)
                                .Take(pageSize)
                                .ToListAsync();

                return (result, totalCount);
          
         }
        public async Task<(IEnumerable<Product> Products, int TotalCount)> GetByBrandIdAsync(Guid id, int pageNumber, int pageSize)
        {
            int totalCount = 0;
            totalCount = await _context.Products.CountAsync(p => p.BrandID == id);

            var result = await (from product in _context.Products
                                join caregory in _context.Categories
                                on product.CategoryID equals caregory.CategoryID
                                join brand in _context.Brands
                                on product.BrandID equals brand.BrandID
                                join store in _context.Stores
                                on product.StoreID equals store.StoreID
                                where product.BrandID == id
                                select new Product
                                {
                                    ProductID = product.ProductID,
                                    StoreID = product.StoreID,
                                    CategoryID = product.CategoryID,
                                    BrandID = product.BrandID,
                                    ProductName = product.ProductName,
                                    Description = product.Description,
                                    Stock = product.Stock,
                                    CreatedDate = product.CreatedDate,
                                    ModifiedDate = product.ModifiedDate,
                                    IsActive = product.IsActive,
                                    Image = product.Image,
                                    Price = product.Price,
                                    ShortDescription = product.ShortDescription,
                                    Discount = product.Discount,
                                    StoreName = store.StoreName,
                                    CategoryName = caregory.CategoryName,
                                    BrandName = brand.BrandName,
                                })
                                .Skip((pageNumber - 1) * pageSize)
                                .Take(pageSize)
                                .ToListAsync();

            return (result, totalCount);
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
                                    ProductName = product.ProductName,
                                    Description = product.Description,
                                    Stock = product.Stock,
                                    CreatedDate = product.CreatedDate,
                                    ModifiedDate = product.ModifiedDate,
                                    IsActive = product.IsActive,
                                    Image = product.Image,
                                    Price = product.Price,
                                    ShortDescription = product.ShortDescription,
                                    Discount = product.Discount,
                                    //SellerName = seller.AgencyName,
                                    //OwnerID = store.OwnerID,
                                    //CategoryName = caregory.CategoryName,
                                    //BrandName = brand.BrandName,
                                }).ToListAsync();

            return result;
            //return await _context.Products.ToListAsync();
        }


        public async Task<(bool isUpdated, string? oldImageUrl)> UpdateAsync(Product product)
        {
            try
            {
                string? oldImage = string.Empty;
                // Find the existing user by UserId
                var existingProduct = await _context.Products.FirstOrDefaultAsync(p => p.ProductID == product.ProductID);

                if (existingProduct == null)
                {
                    return (false, oldImage); // Return false if the user is not found
                }

                // Update the UserName
                existingProduct.BrandID = product.BrandID;
                existingProduct.CategoryID = product.CategoryID;
                existingProduct.ProductName = product.ProductName;
                existingProduct.Description = product.Description;
                existingProduct.ShortDescription = product.ShortDescription;
                existingProduct.Stock = product.Stock;
                existingProduct.Price = product.Price;
                existingProduct.ModifiedDate = DateTime.Now;
                existingProduct.IsActive = true;

                if (product.Image != null) {
                    oldImage = existingProduct.Image;
                    existingProduct.Image = product.Image;
                }

                _context.Products.Update(existingProduct);
                await _context.SaveChangesAsync();
                return (true, oldImage);
            }
            catch (Exception ex)
            {
                // Handle the exception as needed
               _logger.LogError(ex, "An error occurred while updating a product.");
                return (false, null);
            }
        }
        public async Task<bool> UpdateDescripAsync(Product product)
        {
            try
            {

                // Find the existing user by UserId
                var existingProduct = await _context.Products.FirstOrDefaultAsync(p => p.ProductID == product.ProductID);

                if (existingProduct == null)
                {
                    return false; // Return false if the user is not found
                }

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

        public async Task<(bool isDelete, string? oldImageUrl)> DeleteAsync(Guid id)
        {
            string? imageUrl = string.Empty;
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                imageUrl = product.Image;
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
                return (true, imageUrl);   
            }
            return (false, null);
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

        public async Task<(List<Product> TopSellingProducts, List<Product> RandomProducts)> GetTopSellingAndRandomProductsAsyncXX(int featureCount, int randomCount)
        {
            // Step 1: Get the top 50 selling products based on total quantity sold and check stock availability
            var topSellingProducts = await _context.OrderDetails
                .GroupBy(od => od.ProductID)
                .Select(g => new
                {
                    ProductID = g.Key,
                    TotalQuantity = g.Sum(od => od.Quantity)
                })
                .OrderByDescending(p => p.TotalQuantity)
                .Join(_context.Products,
                      od => od.ProductID,
                      p => p.ProductID,
                      (od, p) => p)
                .Where(p => p.Stock > 0) // Check stock availability
                .Take(featureCount)
                .ToListAsync();

            // Step 2: Get n random products (excluding the top-selling ones for variety) and check stock availability
            var randomProducts = await _context.Products
                .Where(p => p.Stock > 0 && !topSellingProducts.Select(ts => ts.ProductID).Contains(p.ProductID)) // Check stock availability and exclude top-selling
                .OrderBy(r => Guid.NewGuid())
                .Take(randomCount)
                .ToListAsync();

            return (topSellingProducts, randomProducts);
        }

        public async Task<List<Product>> GetTopSellingRandomProductsAsync(int featureCount, int randomCount)
        {
            // Step 1: Get the top selling products based on total quantity sold and check stock availability
            var topSellingProducts = await _context.OrderDetails
                .GroupBy(od => od.ProductID)
                .Select(g => new
                {
                    ProductID = g.Key,
                    TotalQuantity = g.Sum(od => od.Quantity)
                })
                .OrderByDescending(p => p.TotalQuantity)
                .Join(_context.Products,
                      od => od.ProductID,
                      p => p.ProductID,
                      (od, p) => new Product
                      {
                          ProductID = p.ProductID,
                          StoreID = p.StoreID,
                          CategoryID = p.CategoryID,
                          BrandID = p.BrandID,
                          ProductName = p.ProductName,
                          Description = p.Description,
                          ShortDescription = p.ShortDescription,
                          Price = p.Price,
                          Stock = p.Stock,
                          CreatedDate = p.CreatedDate,
                          ModifiedDate = p.ModifiedDate,
                          IsActive = p.IsActive,
                          currencyType = p.currencyType,
                          Discount = p.Discount,
                          IsTop = p.IsTop,
                          Image = p.Image,
                          StoreName = _context.Stores.Where(s => s.StoreID == p.StoreID).Select(s => s.StoreName).FirstOrDefault(),
                          CategoryName = _context.Categories.Where(c => c.CategoryID == p.CategoryID).Select(c => c.CategoryName).FirstOrDefault(),
                          BrandName = _context.Brands.Where(b => b.BrandID == p.BrandID).Select(b => b.BrandName).FirstOrDefault()
                      })
                .Where(p => p.Stock > 0) // Check stock availability
                .Take(featureCount)
                .ToListAsync();

            // Step 2: Get random products (excluding the top-selling ones for variety) and check stock availability
            var randomProducts = await _context.Products
                .Where(p => p.Stock > 0 && !topSellingProducts.Select(ts => ts.ProductID).Contains(p.ProductID)) // Check stock availability and exclude top-selling
                .Select(p => new Product
                {
                    ProductID = p.ProductID,
                    StoreID = p.StoreID,
                    CategoryID = p.CategoryID,
                    BrandID = p.BrandID,
                    ProductName = p.ProductName,
                    Description = p.Description,
                    ShortDescription = p.ShortDescription,
                    Price = p.Price,
                    Stock = p.Stock,
                    CreatedDate = p.CreatedDate,
                    ModifiedDate = p.ModifiedDate,
                    IsActive = p.IsActive,
                    currencyType = p.currencyType,
                    Discount = p.Discount,
                    IsTop = p.IsTop,
                    Image = p.Image,
                    StoreName = _context.Stores.Where(s => s.StoreID == p.StoreID).Select(s => s.StoreName).FirstOrDefault(),
                    CategoryName = _context.Categories.Where(c => c.CategoryID == p.CategoryID).Select(c => c.CategoryName).FirstOrDefault(),
                    BrandName = _context.Brands.Where(b => b.BrandID == p.BrandID).Select(b => b.BrandName).FirstOrDefault()
                })
                .OrderBy(r => Guid.NewGuid())
                .Take(randomCount)
                .ToListAsync();

            return  randomProducts;
        }

        public async Task<(List<Product> TopSellingProducts, int TotalCount)> GetAllTopSellingProductsAsync(int featureCount, ProductFilter filter)
        {
            // Step 1: Get the top selling products based on total quantity sold and check stock availability
            var topSellingProductsQuery = _context.OrderDetails
                .GroupBy(od => od.ProductID)
                .Select(g => new
                {
                    ProductID = g.Key,
                    TotalQuantity = g.Sum(od => od.Quantity)
                })
                .OrderByDescending(p => p.TotalQuantity)
                .Join(_context.Products,
                    od => od.ProductID,
                    p => p.ProductID,
                    (od, p) => new Product
                    {
                        ProductID = p.ProductID,
                        StoreID = p.StoreID,
                        CategoryID = p.CategoryID,
                        BrandID = p.BrandID,
                        ProductName = p.ProductName,
                        Description = p.Description,
                        ShortDescription = p.ShortDescription,
                        Price = p.Price,
                        Stock = p.Stock,
                        CreatedDate = p.CreatedDate,
                        ModifiedDate = p.ModifiedDate,
                        IsActive = p.IsActive,
                        currencyType = p.currencyType,
                        Discount = p.Discount,
                        IsTop = p.IsTop,
                        Image = p.Image,
                        StoreName = _context.Stores.Where(s => s.StoreID == p.StoreID).Select(s => s.StoreName).FirstOrDefault(),
                        CategoryName = _context.Categories.Where(c => c.CategoryID == p.CategoryID).Select(c => c.CategoryName).FirstOrDefault(),
                        BrandName = _context.Brands.Where(b => b.BrandID == p.BrandID).Select(b => b.BrandName).FirstOrDefault()
                    })
                .Where(p => p.Stock > 0); // Check stock availability

            // Apply filters to the top-selling products
            if (filter.BrandId.HasValue)
            {
                topSellingProductsQuery = topSellingProductsQuery.Where(p => p.BrandID == filter.BrandId);
            }

            if (filter.CategoryId.HasValue)
            {
                topSellingProductsQuery = topSellingProductsQuery.Where(p => p.CategoryID == filter.CategoryId);
            }

            if (filter.MinPrice.HasValue)
            {
                topSellingProductsQuery = topSellingProductsQuery.Where(p => p.Price >= filter.MinPrice);
            }

            if (filter.MaxPrice.HasValue)
            {
                topSellingProductsQuery = topSellingProductsQuery.Where(p => p.Price <= filter.MaxPrice);
            }

            // Sorting based on filter criteria
            if (filter.LowToHigh.HasValue && filter.LowToHigh.Value)
            {
                topSellingProductsQuery = topSellingProductsQuery.OrderBy(p => p.Price);
            }
            else if (filter.HighToLow.HasValue && filter.HighToLow.Value)
            {
                topSellingProductsQuery = topSellingProductsQuery.OrderByDescending(p => p.Price);
            }
            else if (filter.LatestToOld.HasValue && filter.LatestToOld.Value)
            {
                topSellingProductsQuery = topSellingProductsQuery.OrderByDescending(p => p.CreatedDate);
            }

            // Get the total count of products based on the filters
            var totalCount = await topSellingProductsQuery.CountAsync();

            // Apply pagination
            var topSellingProducts = await topSellingProductsQuery
                .Skip((filter.Page - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ToListAsync();

            return (TopSellingProducts: topSellingProducts, TotalCount: totalCount);
        }



        public async Task<PageVisit> GetPageVisitAsync(Guid xid, string xtype, string month)
        {
            return await _context.PageVisits
                .FirstOrDefaultAsync(pv => pv.XID == xid && pv.XType == xtype && pv.Month == month);
        }

        public async Task<bool> CreatePageVisitAsync(PageVisit pageVisit)
        {
            await _context.PageVisits.AddAsync(pageVisit);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdatePageVisitAsync(PageVisit pageVisit)
        {
            _context.PageVisits.Update(pageVisit);
            await _context.SaveChangesAsync();
            return true;
        }

        //public async Task<List<Product>> SearchProductsAsync(string searchTerm)
        //{
        //    using (var connection = _context.Database.GetDbConnection())
        //    {
        //        if (connection.State == ConnectionState.Closed)
        //            connection.Open();

        //        var command = connection.CreateCommand();
        //        command.CommandText = "SearchProducts";
        //        command.CommandType = CommandType.StoredProcedure;

        //        var searchTermParam = command.CreateParameter();
        //        searchTermParam.ParameterName = "@SearchTerm";
        //        searchTermParam.Value = searchTerm ?? string.Empty;
        //        command.Parameters.Add(searchTermParam);

        //        using (var reader = await command.ExecuteReaderAsync())
        //        {
        //            var results = new List<Product>();

        //            while (await reader.ReadAsync())
        //            {
        //                results.Add(new Product
        //                {
        //                    ProductID = reader.GetGuid(reader.GetOrdinal("ProductID")),
        //                    ProductName = reader.GetString(reader.GetOrdinal("ProductName")),
        //                    Description = reader["ProductDescription"] as string,
        //                    ShortDescription = reader["ShortDescription"] as string,
        //                    Price = reader.GetDecimal(reader.GetOrdinal("Price")),
        //                    Stock = reader.GetInt32(reader.GetOrdinal("Stock")),
        //                    IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive")),
        //                    Image = reader["ProductImage"] as string,
        //                    CreatedDate = reader.GetDateTime(reader.GetOrdinal("CreatedDate")),
        //                    ModifiedDate = reader["ModifiedDate"] as DateTime?,
        //                    currencyType = reader["currencyType"] as string,
        //                    Discount = reader["Discount"] as decimal?,
        //                    IsTop = reader["IsTop"] as bool?,
        //                    CategoryID = reader.GetGuid(reader.GetOrdinal("CategoryID")),
        //                    BrandID = reader.GetGuid(reader.GetOrdinal("BrandID")),
        //                    BrandName = reader["BrandName"] as string,
        //                    CategoryName = reader["CategoryName"] as string
        //                });
        //            }

        //            return results;
        //        }
        //    }
        //}


        public async Task<(List<Product>? Products, int? TotalCount)> SearchProductsAsyncXX(string searchTerm, ProductFilter filter)
        {
            using (var connection = _context.Database.GetDbConnection())
            {
                if (connection.State == ConnectionState.Closed)
                    await connection.OpenAsync();

                var command = connection.CreateCommand();
                command.CommandText = "SearchProducts";
                command.CommandType = CommandType.StoredProcedure;

                // Add search term parameter
                var searchTermParam = command.CreateParameter();
                searchTermParam.ParameterName = "@SearchTerm";
                searchTermParam.Value = searchTerm ?? string.Empty;
                command.Parameters.Add(searchTermParam);

                // Add BrandId filter
                if (filter.BrandId.HasValue)
                {
                    var brandIdParam = command.CreateParameter();
                    brandIdParam.ParameterName = "@BrandId";
                    brandIdParam.Value = filter.BrandId.Value;
                    command.Parameters.Add(brandIdParam);
                }

                // Add CategoryId filter
                if (filter.CategoryId.HasValue)
                {
                    var categoryIdParam = command.CreateParameter();
                    categoryIdParam.ParameterName = "@CategoryId";
                    categoryIdParam.Value = filter.CategoryId.Value;
                    command.Parameters.Add(categoryIdParam);
                }

                // Add MinPrice filter
                if (filter.MinPrice.HasValue)
                {
                    var minPriceParam = command.CreateParameter();
                    minPriceParam.ParameterName = "@MinPrice";
                    minPriceParam.Value = filter.MinPrice.Value;
                    command.Parameters.Add(minPriceParam);
                }

                // Add MaxPrice filter
                if (filter.MaxPrice.HasValue)
                {
                    var maxPriceParam = command.CreateParameter();
                    maxPriceParam.ParameterName = "@MaxPrice";
                    maxPriceParam.Value = filter.MaxPrice.Value;
                    command.Parameters.Add(maxPriceParam);
                }

                // Add LowToHigh sorting
                if (filter.LowToHigh.HasValue)
                {
                    var lowToHighParam = command.CreateParameter();
                    lowToHighParam.ParameterName = "@LowToHigh";
                    lowToHighParam.Value = filter.LowToHigh.Value;
                    command.Parameters.Add(lowToHighParam);
                }

                // Add HighToLow sorting
                if (filter.HighToLow.HasValue)
                {
                    var highToLowParam = command.CreateParameter();
                    highToLowParam.ParameterName = "@HighToLow";
                    highToLowParam.Value = filter.HighToLow.Value;
                    command.Parameters.Add(highToLowParam);
                }

                // Add LatestToOld sorting
                if (filter.LatestToOld.HasValue)
                {
                    var latestToOldParam = command.CreateParameter();
                    latestToOldParam.ParameterName = "@LatestToOld";
                    latestToOldParam.Value = filter.LatestToOld.Value;
                    command.Parameters.Add(latestToOldParam);
                }

                // Add pagination parameters
                var pageParam = command.CreateParameter();
                pageParam.ParameterName = "@Page";
                pageParam.Value = filter.Page;
                command.Parameters.Add(pageParam);

                var pageSizeParam = command.CreateParameter();
                pageSizeParam.ParameterName = "@PageSize";
                pageSizeParam.Value = filter.PageSize;
                command.Parameters.Add(pageSizeParam);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    var results = new List<Product>();

                    // Read products
                    while (await reader.ReadAsync())
                    {
                        results.Add(new Product
                        {
                            ProductID = reader.GetGuid(reader.GetOrdinal("ProductID")),
                            ProductName = reader.GetString(reader.GetOrdinal("ProductName")),
                            Description = reader["ProductDescription"] as string,
                            ShortDescription = reader["ShortDescription"] as string,
                            Price = reader.GetDecimal(reader.GetOrdinal("Price")),
                            Stock = reader.GetInt32(reader.GetOrdinal("Stock")),
                            IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive")),
                            Image = reader["ProductImage"] as string,
                            CreatedDate = reader.GetDateTime(reader.GetOrdinal("CreatedDate")),
                            ModifiedDate = reader["ModifiedDate"] as DateTime?,
                            currencyType = reader["currencyType"] as string,
                            Discount = reader["Discount"] as decimal?,
                            IsTop = reader["IsTop"] as bool?,
                            CategoryID = reader.GetGuid(reader.GetOrdinal("CategoryID")),
                            BrandID = reader.GetGuid(reader.GetOrdinal("BrandID")),
                            BrandName = reader["BrandName"] as string,
                            CategoryName = reader["CategoryName"] as string
                        });
                    }

                    // Move to the next result set for total count
                    await reader.NextResultAsync();

                    int? totalCount = null;
                    if (await reader.ReadAsync())
                    {
                        totalCount = reader.GetInt32(0);
                    }

                    return (Products: results, TotalCount: totalCount);
                }
            }
        }

       // public async Task<(List<Product>? Products, int? TotalCount)> SearchProductsAsync(string searchTerm, ProductFilter filter)
        public async Task<(List<Product>? Products, int? TotalCount)> SearchProductsAsync( ProductFilter filter)
        {
            using (var connection = _context.Database.GetDbConnection())
            {
                if (connection.State == ConnectionState.Closed)
                    await connection.OpenAsync();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "SearchProducts";
                    command.CommandType = CommandType.StoredProcedure;

                    // Add parameters
                    command.Parameters.Add(CreateParameter(command, "@SearchTerm", filter.SearchTerm ?? string.Empty));
                    command.Parameters.Add(CreateParameter(command, "@BrandId", filter.BrandId));
                    command.Parameters.Add(CreateParameter(command, "@CategoryId", filter.CategoryId));
                    command.Parameters.Add(CreateParameter(command, "@MinPrice", filter.MinPrice));
                    command.Parameters.Add(CreateParameter(command, "@MaxPrice", filter.MaxPrice));
                    command.Parameters.Add(CreateParameter(command, "@LowToHigh", filter.LowToHigh));
                    command.Parameters.Add(CreateParameter(command, "@HighToLow", filter.HighToLow));
                    command.Parameters.Add(CreateParameter(command, "@LatestToOld", filter.LatestToOld));
                    command.Parameters.Add(CreateParameter(command, "@Page", filter.Page));
                    command.Parameters.Add(CreateParameter(command, "@PageSize", filter.PageSize));

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        var results = new List<Product>();

                        // Read products
                        while (await reader.ReadAsync())
                        {
                            results.Add(new Product
                            {
                                ProductID = reader.GetGuid(reader.GetOrdinal("ProductID")),
                                ProductName = reader.GetString(reader.GetOrdinal("ProductName")),
                                Description = reader["ProductDescription"] as string,
                                ShortDescription = reader["ShortDescription"] as string,
                                Price = reader.GetDecimal(reader.GetOrdinal("Price")),
                                Stock = reader.GetInt32(reader.GetOrdinal("Stock")),
                                IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive")),
                                Image = reader["ProductImage"] as string,
                                CreatedDate = reader.GetDateTime(reader.GetOrdinal("CreatedDate")),
                                ModifiedDate = reader["ModifiedDate"] as DateTime?,
                                currencyType = reader["currencyType"] as string,
                                Discount = reader["Discount"] as decimal?,
                                IsTop = reader["IsTop"] as bool?,
                                CategoryID = reader.GetGuid(reader.GetOrdinal("CategoryID")),
                                BrandID = reader.GetGuid(reader.GetOrdinal("BrandID")),
                                StoreID = reader.GetGuid(reader.GetOrdinal("StoreID")),
                                BrandName = reader["BrandName"] as string,
                                CategoryName = reader["CategoryName"] as string,
                                StoreName = reader["StoreName"] as string
                            });
                        }

                        // Move to the next result set for total count
                        await reader.NextResultAsync();

                        int? totalCount = null;
                        if (await reader.ReadAsync())
                        {
                            totalCount = reader.GetInt32(0);
                        }

                        return (Products: results, TotalCount: totalCount);
                    }
                }
            }
        }

        // Helper method to create parameters
        private DbParameter CreateParameter(DbCommand command, string name, object? value)
        {
            var parameter = command.CreateParameter();
            parameter.ParameterName = name;
            parameter.Value = value ?? DBNull.Value;
            return parameter;
        }


    }
}
