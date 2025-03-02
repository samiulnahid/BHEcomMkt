using BHEcom.Common.Models;
using BHEcom.Data.Repositories;
using BHEcom.Services.Implementations;
using BHEcom.Services.Interfaces;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc;
using static System.Net.Mime.MediaTypeNames;
using System.IO;
using BHEcom.Common.Helper;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

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
        private readonly IImageService _imageService;
        private readonly FtpUploader _ftpUploader;


        private readonly ILogger<ProductRepository> _logger;
        public ProductsController(IProductService productService, ILogger<ProductRepository> logger, IStoreService storeService, ICategoryService categoryService, IProductDetailService productDetailService, IWebHostEnvironment environment, IImageService imageService, FtpUploader ftpUploader)
        {
            _productService = productService;
            _logger = logger;
            _storeService = storeService;
            _categoryService = categoryService;
            _productDetailService = productDetailService;
            _environment = environment;
            _ftpUploader = ftpUploader;
            _imageService = imageService;
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

                #region Old Image Path Code
                //if (imageFile != null && imageFile.Length > 0)
                //{
                //var galleryPath = Path.Combine(Directory.GetCurrentDirectory(), "Images", "Gallery");
                //var productPath = Path.Combine(galleryPath, "Product");

                //// Ensure "Gallery" folder exists
                //if (!Directory.Exists(galleryPath))
                //{
                //    Directory.CreateDirectory(galleryPath);
                //}

                //// Ensure "Product" folder exists
                //if (!Directory.Exists(productPath))
                //{
                //    Directory.CreateDirectory(productPath);
                //}

                // Use the common FTP upload method

                //string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(imageFile.FileName);
                //var fullPath = Path.Combine(productPath, uniqueFileName);

                //// Save the file
                //using (var fileStream = new FileStream(fullPath, FileMode.Create))
                //{
                //    await imageFile.CopyToAsync(fileStream);
                //}
                //// Set the image path in the product object
                // product.Image = Path.Combine("Gallery", "Product", Path.GetFileName(fullPath)).Replace("\\", "/");

                //}

                #endregion

                if (imageFile != null && imageFile.Length > 0)
                {
                    string folderName = "ecom/product"; // Adjust based on the product folder
                    string imageUrl = await _ftpUploader.UploadFileAsync(imageFile, folderName);

                    // Assign the uploaded image URL to the product
                    product.Image = imageUrl;
                }

                product.CreatedDate = DateTime.Now;
                Guid productId = await _productService.AddProductAsync(product);
                return Ok(new { id = productId, Success = true });

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding a product.");
                return StatusCode(500, new { Message = ex.Message, Success = false });
            }
        }

        // [HttpPost("CreateProduct")]
        //public async Task<ActionResult> CreateProduct([FromBody] Product product)
        //{
        //    try
        //    {
        //        if (product == null)
        //        {
        //            return BadRequest("Product data is required.");
        //        }

        //        product.CreatedDate = DateTime.Now;
        //        Guid productId = await _productService.AddProductAsync(product);
        //        return Ok(new {ProductId = productId , Success = true});



        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "An error occurred while adding a product.");
        //        return StatusCode(500, new { Message = ex.Message, Success = false });
        //    }
        //}

        [HttpGet("GetById/{id}")]
        public async Task<ActionResult<Product>> GetById(Guid id)
        {
            try
            {
                var product = await _productService.GetProductByIdAsync(id);
                if (product == null)
                {
                    return Ok(new { data = product, message = "Not Found!", success = true });
                }

                return Ok(new { data = product, Success = true });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting a product.");
                return StatusCode(500, new { Message = ex.Message, Success = false });
            }
        }

        [HttpGet("GetByCategoryId/{id}/{pageNumber}/{pageSize}")]
        public async Task<ActionResult<List<Product>>> GetByCategoryId(Guid id, int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                var (products, totalCount) = await _productService.GetProductByCategoryIdAsync(id, pageNumber, pageSize);
                if (products == null)
                {
                    return Ok(new { Products = products, totalPage = 0, Success = true });
                }

                // Calculate the total number of pages
                int totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

                return Ok(new { data = products, totalPage = totalPages, Success = true });


            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting a product.");
                return StatusCode(500, new { Message = ex.Message, Success = false });
            }
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<List<Product>>> GetAll()
        {
            try
            {
                var products = await _productService.GetAllProductsAsync();

                //var result = GenerateProductImageUrl(products);
                return Ok(new { data = products, Success = true });

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting all products.");
                return StatusCode(500, new { Message = ex.Message, Success = false });
            }
        }

        [HttpGet("GetByStoreId/{id}")]
        public async Task<ActionResult<List<Product>>> GetByStoreId(Guid id)
        {
            try
            {
                var products = await _productService.GetAllProductsByStoreIdAsync(id);

                //var result = GenerateProductImageUrl(products);
                return Ok(new { data = products, Success = true });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting all products.");
                return StatusCode(500, new { Message = ex.Message, Success = false });
            }
        }

        [HttpPut("Update/{id}")]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult> Update(Guid id, [FromForm] Product product, [FromForm] IFormFile? imageFile)
        {
            try
            {
                if (id != product.ProductID)
                {
                    return BadRequest("Product ID mismatch.");
                }

                #region old Image Url Code
                //if (imageFile != null && imageFile.Length > 0)
                //{
                //    var galleryPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Gallery");
                //    var productPath = Path.Combine(galleryPath, "Product");

                //    // Ensure "Gallery" folder exists
                //    if (!Directory.Exists(galleryPath))
                //    {
                //        Directory.CreateDirectory(galleryPath);
                //    }

                //    // Ensure "Product" folder exists
                //    if (!Directory.Exists(productPath))
                //    {
                //        Directory.CreateDirectory(productPath);
                //    }

                //    string fileName = Path.GetFileName(imageFile.FileName);
                //    string uniqueFileName = Guid.NewGuid().ToString() + "_" + fileName;
                //    var fullPath = Path.Combine(productPath, uniqueFileName);

                //    // Check if the file already exists
                //    var existingFile = Directory.GetFiles(productPath, $"*_{fileName}").FirstOrDefault();
                //    if (existingFile != null)
                //    {
                //        // Use the existing file's relative path
                //        product.Image = Path.Combine("Gallery", "Product", Path.GetFileName(existingFile)).Replace("\\", "/");
                //    }
                //    else
                //    {
                //        // Save the file
                //        using (var fileStream = new FileStream(fullPath, FileMode.Create))
                //        {
                //            await imageFile.CopyToAsync(fileStream);
                //        }

                //        // Set the image path in the product object
                //        product.Image = Path.Combine("Gallery", "Product", Path.GetFileName(fullPath)).Replace("\\", "/");
                //    }
                //}

                #endregion

                if (imageFile != null && imageFile.Length > 0)
                {
                    string folderName = "ecom/product"; // Adjust based on the product folder
                    string imageUrl = await _ftpUploader.UploadFileAsync(imageFile, folderName);

                    // Assign the uploaded image URL to the product
                    product.Image = imageUrl;
                }

                var (isUpdated, oldImageUrl) = await _productService.UpdateProductAsync(product);

                if (!isUpdated)
                {
                    return StatusCode(500, new { Message = "Unsuccessfully Updated", Success = false });
                }

                if (!string.IsNullOrEmpty(oldImageUrl))
                {
                    // Image delete code
                    _ftpUploader.DeleteFile(oldImageUrl);
                }

                return Ok(new { Message = "Successfully Updated", Success = true });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating a product.");
                return StatusCode(500, new { Message = ex.Message, Success = false });
            }
        }

        [HttpDelete("Delete/{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            try
            {
                string xType = ResourceType.Product.ToString().ToLower();

                var images = await _imageService.GetImagesByXIdAsync(id, xType);

                // Delete Product From product table
                var (isDelete, oldImageUrl) = await _productService.DeleteProductAsync(id);

                if (!isDelete)
                {
                    return NotFound(new { Message = "No product found for the provided id.", Success = false });
                }

                if (!string.IsNullOrEmpty(oldImageUrl))
                {
                    // Image delete code
                    _ftpUploader.DeleteFile(oldImageUrl);
                }

                // Call repository to delete all product images
                bool isDeleted = await _imageService.DeleteImagesByXIdAsync(id, xType);

                if (!isDeleted)
                {
                    return NotFound(new { Message = "No images found for the provided XID and XType.", Success = false });
                }

                foreach (var item in images)
                {
                    if (!string.IsNullOrEmpty(item.ImagePath))
                    {
                        // Image delete code
                        _ftpUploader.DeleteFile(item.ImagePath);
                    }
                }


                return Ok(new { Message = "Successfully Deleted", Success = true });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting a product.");
                return StatusCode(500, new { Message = ex.Message, Success = false });
            }
        }

        [HttpGet("GetAllStore")]
        public async Task<ActionResult<IEnumerable<Store>>> GetAllStore()
        {
            try
            {
                var stores = await _storeService.GetAllStoresAsync();
                return Ok(new { data = stores, Success = true });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting all stores.");
                return StatusCode(500, new { Message = ex.Message, Success = false });
            }
        }

        [HttpGet("GetStoreConfigById/{id}")]
        public async Task<ActionResult<StoreConfig>> GetStoreConfigById(Guid id)
        {
            var storeData = await _storeService.GetStoreConfigAsync(id);

            if (storeData == null)
            {
                return Ok(new { data = storeData,Message = "Data not Found!", Success = true });
            }
            return Ok(new { data = storeData, Success = true });
        }

        [HttpGet("GetSubCategories/{id}")]
        public async Task<ActionResult<Category>> GetSubCategories(Guid id)
        {
            var subCategories = await _categoryService.GetSubCategoriesByParentCategoryIDAsync(id);

            if (!subCategories.Any())
            {
                return Ok(new { data = subCategories, Message = "Data not Found!", Success = true });
            }
            return Ok(new { data = subCategories, Success = true });
        }

        [HttpGet("GetProductFieldsByCategoryId/{id}")]
        public async Task<ActionResult<StoreProductField>> GetProductFieldsByCategoryId(Guid id)
        {
            var productFields = await _storeService.GetProductFieldsByCategoryId(id);

            if (!productFields.Any())
            {
                return Ok(new { data = productFields, Message = "Data not Found!", Success = true });
            }
            return Ok(new { data = productFields, Success = true });
        }

        [HttpPost("CreateProductDetails")]
        public async Task<ActionResult> CreateProductDetails([FromBody] ProductManager productManager)
        {
            try
            {
                if (productManager == null)
                {
                    return BadRequest("Invalid data.");
                }
                if (productManager.ShortDescription != null || productManager.Description != null)
                {
                    Product product = new Product
                    {
                        ProductID = productManager.ProductId,
                        Description = productManager.Description,
                        ShortDescription = productManager.ShortDescription,

                    };
                    bool isChange = await _productService.UpdateProductDescripAsync(product);
                    if (!isChange)
                    {
                        return Ok(new { Message = "Update Unsuccessful!", Success = false });
                    }

                }

                if (productManager.ProductDetailList == null)
                {
                    return Ok(new { Message = "ProductList Empty!", Success = false });
                }

                foreach (var productDetail in productManager.ProductDetailList)
                {
                    bool IsCreated = await _productDetailService.AddProductDetailAsync(productDetail);
                    if (!IsCreated)
                    {
                        return Ok(new { Message = "Update Unsuccessful!", Success = false });
                    }
                }
                return Ok(new { Message = "Create Succefully", Success = true });

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding a productDetail.");
                return StatusCode(500, new { Message = ex.Message, Success = false });
            }

        }

        [HttpPost("CreateandUpdateDetails")]
        public async Task<ActionResult> CreateandUpdateDetails([FromBody] ProductManager productManager)
        {
            try
            {
                if (productManager == null)
                {
                    return BadRequest("Invalid data.");
                }
                if (productManager.ShortDescription != null || productManager.Description != null)
                {
                    Product product = new Product
                    {
                        ProductID = productManager.ProductId,
                        Description = productManager.Description,
                        ShortDescription = productManager.ShortDescription,

                    };
                    bool isChange = await _productService.UpdateProductDescripAsync(product);
                    if (!isChange)
                    {
                        return BadRequest("Invalid data.");
                    }

                }

                if (productManager.ProductDetailList == null)
                {
                    return BadRequest("Invalid data.");
                }

                foreach (var productDetail in productManager.ProductDetailList)
                {
                    if (productDetail.DetailID == Guid.Empty)
                    {
                        bool IsCreated = await _productDetailService.AddProductDetailAsync(productDetail);
                        if (!IsCreated)
                        {
                            return BadRequest("Invalid data.");
                        }
                    }
                    else
                    {
                        bool IsUpdated = await _productDetailService.UpdateProductDetailAsync(productDetail);
                        if (!IsUpdated)
                        {
                            return NotFound($"ProductDetail with ID {productDetail.DetailID} not found.");
                        }
                    }

                }
                return Ok(new { Message = "Create Succefully", Success = true });

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding a productDetail.");
                return StatusCode(500, new { Message = ex.Message, Success = false });
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
                    ProductId = id,
                    Description = product.Description,
                    ShortDescription = product.ShortDescription,
                    ProductDetailList = productDetails,
                };

                return Ok(new { data = productManager, Success = true });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting all Product Detail.");
                return StatusCode(500, new { Message = ex.Message, Success = false });
            }
        }

        [HttpGet("GetProductFullInformation/{id}")]
        public async Task<ActionResult<ProductManager>> GetProductFullInformation(Guid id)
        {
            try
            {
                var product = await _productService.GetProductByIdAsync(id);

                var productDetails = await _productDetailService.GetAllProductDetailByProductId(id);

                ProductManager productManager = new ProductManager
                {
                    ProductId = id,
                    Description = product.Description,
                    ShortDescription = product.ShortDescription,
                    ProductDetailList = productDetails,
                };
                var fullData = new
                {
                    ProductData = product,
                    DetailsData = productManager,

                };

                string xType = ResourceType.Product.ToString().ToLower();

                // Update PageVisits table
                await CreateAndUpdatePageVisits(id, xType);

                return Ok(new { Deta = fullData, Success = true });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting all Product Detail.");
                return StatusCode(500, new { Message = ex.Message, Success = false });
            }
        }

        [HttpGet("GetRandomProduct/{num}")]
        public async Task<ActionResult<List<Product>>> GetRandomProduct(int num)
        {
            try
            {

                var products = await _productService.GetRandomProductsAsync(num);
                // var result = GenerateProductImageUrl(products);

                return Ok(new { data = products, Success = true });
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
                // var result = GenerateProductImageUrl(products);

                return Ok(new { data = products, Success = true });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving random products.");
                return StatusCode(500, "An error occurred while retrieving products.");
            }
        }

        [HttpGet("GetFeatureProduct/{num}")]
        public async Task<ActionResult<List<Product>>> GetFeatureProduct(int num)
        {
            try
            {
                int featureCount = 50;
                var randomProducts = await _productService.GetTopSellingRandomProductsAsync(featureCount, num);
                // var result = GenerateProductImageUrl(products);

                return Ok(new { data = randomProducts, Success = true });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving feature products.");
                return StatusCode(500, "An error occurred while feature products.");
            }
        }

        [HttpPost("GetAllFeatureProduct/{num}")]
        public async Task<ActionResult<List<Product>>> GetAllFeatureProduct(int num, ProductFilter filterEntity)
        {
            try
            {
               // var (allProducts, randomProducts) = await _productService.GetTopSellingAndRandomProductsAsync(num, 10);
                var (products, totalCount) = await _productService.GetAllTopSellingProductsAsync(num, filterEntity);
                if (products != null && totalCount != null)
                {
                    if (totalCount == 0)
                    {
                        return Ok(new { Products = new List<Product>(), totalPage = 0, Success = true });
                    }
                    // var result = GenerateProductImageUrl(products);
                    int totalPages = (int)Math.Ceiling((double)totalCount / filterEntity.PageSize);
                    return Ok(new { data = products, totalPage = totalPages, Success = true });
                }

                return StatusCode(500, "An error occurred while search products.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving feature products.");
                return StatusCode(500, "An error occurred while feature products.");
            }
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
                        return Ok(new { data = products, Message = "Data not found!", totalPage = 0, Success = true });
                    }
                    // var result = GenerateProductImageUrl(products);
                    int totalPages = (int)Math.Ceiling((double)totalCount / filterEntity.PageSize);
                    return Ok(new { data = products, totalPage = totalPages, Success = true });
                }

                return StatusCode(500, "An error occurred while retrieving products.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving random products.");
                return StatusCode(500, "An error occurred while retrieving products.");
            }
        }

        [HttpGet("GetByBrandId/{id}/{pageNumber}/{pageSize}")]
        public async Task<ActionResult<List<Product>>> GetByBrandId(Guid id, int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                var (products, totalCount) = await _productService.GetProductByBrandIdAsync(id, pageNumber, pageSize);
                if (products == null)
                {
                    //return NotFound(new { Products = new List<Product>(), totalPage = 0, Success = false });
                    return Ok(new { Products = products, totalPage = 0, Message = "Data not found!", Success = true });
                }

                // Calculate the total number of pages
                int totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

                return Ok(new { data = products, totalPage = totalPages, Success = true });


            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting a product.");
                return StatusCode(500, new { Message = ex.Message, Success = false });
            }
        }

       // [HttpPost("SearchProducts/{searchTerm}")]
        [HttpPost("SearchProducts")]
        public async Task<ActionResult<List<Product>>> SearchProducts(ProductFilter filterEntity)
        {
            try
            {
                var (products, totalCount) = await _productService.SearchProductsAsync(filterEntity);
                if (products != null && totalCount != null)
                {
                    if (totalCount == 0)
                    {
                        //return Ok(new { Products = new List<Product>(), totalPage = 0 });
                        return Ok(new { Products = products, totalPage = 0, Message = "Data not found!", Success = true });
                    }
                    // var result = GenerateProductImageUrl(products);
                    int totalPages = (int)Math.Ceiling((double)totalCount / filterEntity.PageSize);
                    return Ok(new { data = products, totalPage = totalPages, Success = true });
                }

                return StatusCode(500, "An error occurred while search products.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving search products.");
                return StatusCode(500, "An error occurred while search products.");
            }
        }

       

        [HttpPost("CreateProductGallery/{id}")]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult> CreateProductGallery(Guid id, [FromForm] List<IFormFile>? imageFiles)
        {
            try
            {
                if (imageFiles == null || imageFiles.Count == 0)
                {
                    return Ok(new { Message = "No files were provided.", Success = true });
                }

                string folderName = "ecom/product"; // Adjust based on the product folder

                List<Images> imagesToAdd = new List<Images>();

                foreach (var imageFile in imageFiles)
                {
                    if (imageFile.Length > 0)
                    {
                        string imageUrl = await _ftpUploader.UploadFileAsync(imageFile, folderName);

                        // Create Image object
                        imagesToAdd.Add(new Images
                        {
                            ImageId = Guid.NewGuid(),
                            ImagePath = imageUrl,
                            XID = id,
                            XType = ResourceType.Product.ToString().ToLower(),
                            CreatedAt = DateTime.UtcNow
                        });
                    }
                }
                // Save images to the database
                bool isCreated = await _imageService.AddImagelistAsync(imagesToAdd);
                if (!isCreated)
                {
                    return Ok(new
                    {
                        Message = "Added unsuccessful!",
                        Success = false,
                        UploadedImages = imagesToAdd.Select(i => i.ImagePath).ToList()
                    });
                }

                return Ok(new
                {
                    Message = "Successfully added.",
                    Success = true,
                    UploadedImages = imagesToAdd.Select(i => i.ImagePath).ToList()
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while uploading product images.");
                return StatusCode(500, new { Message = ex.Message, Success = false });
            }
        }


        [HttpDelete("DeleteProductGallery")]
        public async Task<ActionResult> DeleteProductGallery(Guid id)
        {
            try
            {
                string xType = ResourceType.Product.ToString().ToLower();

                var images = await _imageService.GetImagesByXIdAsync(id, xType);

                // Call repository to delete images
                bool isDeleted = await _imageService.DeleteImagesByXIdAsync(id, xType);

                if (!isDeleted)
                {
                    return NotFound(new { Message = "No images found for the provided XID and XType.", Success = false });
                }

                foreach (var item in images)
                {
                    if (!string.IsNullOrEmpty(item.ImagePath))
                    {
                        // Image delete code
                        _ftpUploader.DeleteFile(item.ImagePath);
                    }
                }

                return Ok(new { Message = "Images successfully deleted.", Success = true });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting product images.");
                return StatusCode(500, new { Message = ex.Message, Success = false });
            }
        }


        [HttpDelete("DeleteSingleImage/{id}")]
        public async Task<ActionResult> DeleteSingleImage(Guid id)
        {
            try

            {
                // Call repository to delete images
                var (isDeleted, oldImageUrl) = await _imageService.DeleteImagesByImageIdAsync(id);

                if (!isDeleted)
                {
                    return NotFound(new { Message = "No images found for the provided Image ID", Success = false });
                }
                if (!string.IsNullOrEmpty(oldImageUrl))
                {
                    // Image delete code
                    _ftpUploader.DeleteFile(oldImageUrl);
                }

                return Ok(new { Message = "Images successfully deleted.", Success = true });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting product images.");
                return StatusCode(500, new { Message = ex.Message, Success = false });
            }
        }


        [HttpGet("GetProductGallery/{id}")]
        public async Task<ActionResult> GetProductGallery(Guid id)
        {
            try
            {
                string xType = ResourceType.Product.ToString().ToLower();

                // Call repository to fetch images by XID and XType
                var images = await _imageService.GetImagesByXIdAsync(id, xType);

                if (images == null || !images.Any())
                {
                    return NotFound(new { Message = "No images found for the provided XID and XType.", Success = false });
                }

                return Ok(new
                {
                    Message = "Images retrieved successfully.",
                    Success = true,
                    Images = images.Select(i => new
                    {
                        i.ImageId,
                        i.ImagePath,
                        i.XID,
                        i.XType,
                        i.CreatedAt
                    }).ToList()
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving product images.");
                return StatusCode(500, new { Message = ex.Message, Success = false });
            }
        }

        private IEnumerable<Product> GenerateProductImageUrl(IEnumerable<Product> products)
        {
            if (products == null || !products.Any())
                return Enumerable.Empty<Product>();

            var productList = products.ToList();
            var baseUrl = $"{Request.Scheme}://{Request.Host}";
            var defaultImage = $"{baseUrl}/images/default-product.jpg";

            foreach (var product in productList)
            {
                if (!string.IsNullOrEmpty(product.Image))
                {
                    // Normalize the path to use the correct directory separator for local file system
                    var sanitizedPath = product.Image.Replace("\\", "/").Replace("//", "/");
                    // var sanitizedPath = product.Image.Replace("/", Path.DirectorySeparatorChar.ToString());

                    // Build the full image path for file existence check
                    var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "Images", sanitizedPath.Replace("/", Path.DirectorySeparatorChar.ToString()));

                    //imagePath = imagePath.Replace("\\", "/").Replace("//", "/");

                    // Check if the file exists
                    if (System.IO.File.Exists(imagePath))
                    {
                        // Convert sanitized path to a proper URL format
                        product.Image = $"{baseUrl}/Images/{sanitizedPath}";
                    }
                    else
                    {
                        product.Image = defaultImage;
                    }
                }
                else
                {
                    product.Image = defaultImage;
                }
            }

            return productList;
        }

        private async Task CreateAndUpdatePageVisits(Guid productId, string type)
        {
            try
            {
                string currentMonth = DateTime.UtcNow.ToString("MMMM yyyy");
                var pageVisit = await _productService.GetPageVisitAsync(productId, type, currentMonth);

                if (pageVisit != null)
                {
                    // Update existing page visit
                    pageVisit.Hit += 1;
                    await _productService.UpdatePageVisitAsync(pageVisit);
                }
                else
                {
                    // Create new page visit
                    var newPageVisit = new PageVisit
                    {
                        PageVisitID = Guid.NewGuid(),
                        XID = productId,
                        XType = type,
                        Month = currentMonth,
                        Hit = 1
                    };

                    await _productService.CreatePageVisitAsync(newPageVisit);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating page visits.");
                throw;
            }

        }
    }
}
