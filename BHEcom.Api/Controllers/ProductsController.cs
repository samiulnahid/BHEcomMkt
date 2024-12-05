using BHEcom.Common.Models;
using BHEcom.Data.Repositories;
using BHEcom.Services.Implementations;
using BHEcom.Services.Interfaces;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc;
using static System.Net.Mime.MediaTypeNames;
using System.IO;

namespace BHEcom.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IStoreService _storeService;
        private readonly ICategoryService _categoryService;
        private readonly IProductDetailService _productDetailService;
        private readonly IWebHostEnvironment _environment;


        private readonly ILogger<ProductRepository> _logger;
        public ProductsController(IProductService productService, ILogger<ProductRepository> logger, IStoreService storeService, ICategoryService categoryService, IProductDetailService productDetailService, IWebHostEnvironment environment)
        {
            _productService = productService;
            _logger = logger;
            _storeService = storeService;
            _categoryService = categoryService;
            _productDetailService = productDetailService;
            _environment = environment;
        }

        [HttpPost("Create")]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult> Create([FromForm] Product product, [FromForm] IFormFile imageFile)
        {
            try
            {
                if (product == null)
                {
                    return BadRequest("Product data is required.");
                }

               
                if (imageFile != null && imageFile.Length > 0)
                {
                    var galleryPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Gallery");
                    var productPath = Path.Combine(galleryPath, "Product");

                    //var productPath = Path.Combine(galleryPath, "Product");

                    // Ensure "Gallery" folder exists
                    if (!Directory.Exists(galleryPath))
                    {
                        Directory.CreateDirectory(galleryPath);
                    }

                    // Ensure "Product" folder exists
                    if (!Directory.Exists(productPath))
                    {
                        Directory.CreateDirectory(productPath);
                    }


                    //if (!Directory.Exists(folderPath))
                    //{
                    //    Directory.CreateDirectory(folderPath);
                    //}
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(imageFile.FileName);
                    var fullPath = Path.Combine(productPath, uniqueFileName);


                    // Save the file
                    using (var fileStream = new FileStream(fullPath, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(fileStream);
                    }

                    // Set the image path in the product object
                     product.Image = Path.Combine("Gallery", "Product", Path.GetFileName(fullPath)).Replace("\\", "/");


                    //product.Image = Path.Combine("images", uniqueFileName).Replace("\\","/"); 

                    //if (imageFile != null && imageFile.Length > 0)
                    //{
                    //string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
                    //Directory.CreateDirectory(uploadsFolder); // Ensure directory exists

                    // Generate a unique filename to avoid conflicts
                    //string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(imageFile.FileName);
                    //string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    // Save the image to the specified local path
                    //using (var fileStream = new FileStream(filePath, FileMode.Create))
                    //{
                    // await imageFile.CopyToAsync(fileStream);
                    // }

                    // Set the product's Image property to the relative file path for database storage
                    //product.Image = Path.Combine("images", uniqueFileName);
                    //}
                }
                product.CreatedDate = DateTime.Now;
                Guid productId = await _productService.AddProductAsync(product);
                return Ok(new {ProductId = productId , Success = true});



            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding a product.");
                return StatusCode(500, ex.Message);
            }
        }




        [HttpGet("GetById/{id}")]
        public async Task<ActionResult<Product>> GetById(Guid id)
        {
            try
            {
                var product = await _productService.GetProductByIdAsync(id);
                if (product == null)
                {
                    return NotFound();
                }

                // Append base URL to image path if needed
                if (!string.IsNullOrEmpty(product.Image))
                {
                    //product.Image = $"{Request.Scheme}://{Request.Host}/{product.Image}";
                    var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", product.Image.Replace("/", Path.DirectorySeparatorChar.ToString()));
                    if (System.IO.File.Exists(imagePath))
                    {
                        product.Image = $"{Request.Scheme}://{Request.Host}/{product.Image}";
                    }
                    else
                    {
                        product.Image = $"{Request.Scheme}://{Request.Host}/images/default-product.jpg"; // Default image URL
                    }
                }

                return Ok(product);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting a product.");
                return StatusCode(500, ex.Message);
            }
        }



        [HttpGet("GetByCategoryId/{id}")]
        public async Task<ActionResult<List<Product>>> GetByCategoryId(Guid id)
        {
            try
            {
                var product = await _productService.GetProductByCategoryIdAsync(id);
                if (product == null)
                {
                    return NotFound();
                }
                var result = GenerateProductObjectList(product);

                return Ok(product);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting a product.");
                return StatusCode(500, ex.Message);
            }
        }


        [HttpGet("GetAll")]
        public async Task<ActionResult<List<Product>>> GetAll()
        {
            try
            {
                var products = await _productService.GetAllProductsAsync();

                //var result = GenerateProductObjectList(products);
                return Ok(products);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting all products.");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("GetByStoreId/{id}")]
        public async Task<ActionResult<List<Product>>> GetByStoreId(Guid id)
        {
            try
            {
                var products = await _productService.GetAllProductsByStoreIdAsync(id);

                var result = GenerateProductObjectList(products);
                return Ok(products);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting all products.");
                return StatusCode(500, ex.Message);
            }
        }


        [HttpPut("Update/{id}")]
        public async Task<ActionResult> Update(Guid id, [FromForm] Product product)
        {
            try
            {
                if (id != product.ProductID)
                {
                    return BadRequest("Product ID mismatch.");
                }

                //// Check if a new image file is provided
                //if (imageFile != null && imageFile.Length > 0)
                //{
                //    // Remove the old image if it exists
                //    if (!string.IsNullOrEmpty(product.Image))
                //    {
                //        string oldImagePath = Path.Combine("wwwroot", product.Image);
                //        if (System.IO.File.Exists(oldImagePath))
                //        {
                //            System.IO.File.Delete(oldImagePath);
                //        }
                //    }

                //    // Save the new image
                //    string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
                //    Directory.CreateDirectory(uploadsFolder);

                //    string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(imageFile.FileName);
                //    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                //    using (var fileStream = new FileStream(filePath, FileMode.Create))
                //    {
                //        await imageFile.CopyToAsync(fileStream);
                //    }

                //    // Update the product's Image property with the new image path
                //    product.Image = Path.Combine("images", uniqueFileName);
                //}

                await _productService.UpdateProductAsync(product);
                return Ok("Successfully Updated");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating a product.");
                return StatusCode(500, ex.Message);
            }
        }


        [HttpDelete("Delete/{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            try
            {
                await _productService.DeleteProductAsync(id);
                return Ok("Successfully Deleted");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting a product.");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("GetAllStore")]
        public async Task<ActionResult<IEnumerable<Store>>> GetAllStore()
        {
            try
            {
                var stores = await _storeService.GetAllStoresAsync();
                return Ok(stores);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting all stores.");
                return StatusCode(500, ex.Message);
            }
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

        [HttpGet("GetSubCategories/{id}")]
        public async Task<ActionResult<Category>> GetSubCategories(Guid id)
        {
            var subCategories = await _categoryService.GetSubCategoriesByParentCategoryIDAsync(id);

            if (!subCategories.Any())
            {
                return NotFound();
            }

            return Ok(subCategories);
        }

        [HttpGet("GetProductFieldsByCategoryId/{id}")]
        public async Task<ActionResult<StoreProductField>> GetProductFieldsByCategoryId(Guid id)
        {
            var subCategories = await _storeService.GetProductFieldsByCategoryId(id);

            if (!subCategories.Any())
            {
                return NotFound();
            }

            return Ok(subCategories);
        }

        [HttpPost("CreateProductDetails")]
        public async Task<ActionResult> CreateProductDetails([FromBody] ProductManager productManager)
        {
            try
            {
                if (productManager == null )
                {
                    return BadRequest("Invalid data.");
                }
                if (productManager.ShortDescription != null || productManager.Description != null)
                {
                    Product product = new Product
                    {
                        Description = productManager.Description,
                        ShortDescription = productManager.ShortDescription,
                    };
                    bool isChange = await _productService.UpdateProductAsync(product);
                    if (isChange) {
                        return BadRequest("Invalid data.");
                    }

                }

                if (productManager.ProductDetailList == null)
                {
                    return BadRequest("Invalid data.");
                }

                foreach (var productDetail in productManager.ProductDetailList)
                {
                    bool IsCreated = await _productDetailService.AddProductDetailAsync(productDetail);
                    if (!IsCreated)
                    {
                        return BadRequest("Invalid data.");
                    }
                }
                return Ok("created Successfully");

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding a productDetail.");
                return StatusCode(500, ex.Message);
            }
            
        }

        [HttpGet("GetAllProductDetailByProductId/{id}")]
        public async Task<ActionResult<ProductManager>> GetAllProductDetailByProductId(Guid id)
        {
            try
            {
                var product = await _productService.GetProductByIdAsync(id);


                var productDetails = await _productDetailService.GetAllProductDetailByProductId(id);

                ProductManager productManager = new ProductManager 
                { 
                
                    Description = product.Description,
                    ShortDescription = product.ShortDescription,
                    ProductDetailList = productDetails, 
                };


                return Ok(productManager);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting all Product Detail.");
                return StatusCode(500, ex.Message);
            }
        }

        //[HttpGet("GetRendomProductId/{num}")]
        //public async Task<ActionResult<Product>> GetRandomProduct(int num)
        //{
        //    try
        //    {

        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "An error occurred while getting random Products .");
        //        return StatusCode(500, ex.Message);
        //    }

        //}

        [HttpGet("GetRandomProduct/{num}")]
        public async Task<ActionResult<List<Product>>> GetRandomProduct(int num)
        {
            try
            {

                var products = await _productService.GetRandomProductsAsync(num);
                var result = GenerateProductObjectList(products);

                return Ok(products);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving random products.");
                return StatusCode(500, "An error occurred while retrieving products.");
            }
        }


        [HttpGet("GetLatestProduct/{num}")]
        public async Task<ActionResult<List<Product>>> GetLatestProduct(int num)
        {
            try
            {

                var products = await _productService.GetLatestProductsAsync(num);
                var result = GenerateProductObjectList(products);

                return Ok(products);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving random products.");
                return StatusCode(500, "An error occurred while retrieving products.");
            }
        }



        private async Task<List<object>> GenerateProductObjectList(IEnumerable<Product> products)
        {
            var result = new List<object>();

            foreach (var product in products)
            {
                // Get the image file path from the database field
                string imagePath = Path.Combine(_environment.WebRootPath, "images", product.Image ?? "");

                byte[]? imageData = null;
                if (!string.IsNullOrWhiteSpace(product.Image) && System.IO.File.Exists(imagePath))
                {
                    imageData = await System.IO.File.ReadAllBytesAsync(imagePath);
                }

                result.Add(new
                {
                    product.ProductID,
                    product.StoreID,
                    product.CategoryID,
                    product.BrandID,
                    product.SellerID,
                    product.ProductName,
                    product.Description,
                    product.ShortDescription,
                    product.Price,
                    product.Stock,
                    Image = imageData != null ? Convert.ToBase64String(imageData) : null
                });
            }
            return result;
        }

        [HttpPost("FilterProduct")]
        public async Task<ActionResult<List<Product>>> FilterProduct(ProductFilter filterEntity)
        {
            try
            {
                var (products, totalCount) = await _productService.FilterAllProduct(filterEntity);
                if (products != null && totalCount != null)
                {
                    if (totalCount == 0)
                    {
                        return Ok(new { Products = new List<Product>(), totalPage = 0 });
                    }
                    var result = GenerateProductObjectList(products);
                    int totalPages = (int)Math.Ceiling((double)totalCount / filterEntity.PageSize);
                    return Ok(new { Products = products, totalPage = totalPages });
                }

                return StatusCode(500, "An error occurred while retrieving products.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving random products.");
                return StatusCode(500, "An error occurred while retrieving products.");
            }
        }


    }
}
