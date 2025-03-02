using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using BHEcom.Data;
using BHEcom.Data.Repositories;
using BHEcom.Services;
using BHEcom.Services.Implementations;
using BHEcom.Services.Interfaces;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Identity;
using BHEcom.Common.Helper;
using BHEcom.Common.Models;
namespace BHEcom.Api
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<FtpSettings>(Configuration.GetSection("FtpSettings"));

            services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigin",
                    builder => builder
                        .AllowAnyOrigin() // or .WithOrigins("https://example.com") for specific origins
                        .AllowAnyMethod()
                        .AllowAnyHeader());
            });

            services.AddControllers();

           
            // Database context
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            
            //services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
            //    .AddRoles<IdentityRole>()
            //    .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddIdentity<IdentityUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
        .AddEntityFrameworkStores<ApplicationDbContext>()
        .AddDefaultTokenProviders();

            services.AddDbContext<ApplicationDbContext>(options =>
       options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));


            services.AddControllersWithViews();
            services.AddRazorPages();

            services.AddScoped<FtpUploader>();


            // Repositories
            services.AddScoped<IBrandRepository, BrandRepository>();
            services.AddScoped<IAddressRepository, AddressRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IAgentRepository, AgentRepository>();
            services.AddScoped<IAttributeRepository, AttributeRepository>();
            services.AddScoped<ICartRepository, CartRepository>();
            services.AddScoped<ICartItemRepository, CartItemRepository>();
            services.AddScoped<IFormFieldRepository, FormFieldRepository>();
            services.AddScoped<IFormsRepository, FormsRepository>();
            services.AddScoped<IFormSubmissionRepository, FormSubmissionRepository>();
            services.AddScoped<IFormSubmissionFieldRepository, FormSubmissionFieldRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IOrderDetailRepository, OrderDetailRepository>();
            services.AddScoped<IPageRepository, PageRepository>();
            services.AddScoped<IPageContentRepository, PageContentRepository>();
            services.AddScoped<IPaymentRepository, PaymentRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IProductDetailRepository, ProductDetailRepository>();
            services.AddScoped<IReviewRepository, ReviewRepository>();
            services.AddScoped<ISEORepository, SEORepository>();
            services.AddScoped<IShippingRepository, ShippingRepository>();
            services.AddScoped<IWishlistRepository, WishlistRepository>();
            services.AddScoped<IAdminRepository, AdminRepository>();
            services.AddScoped<IAdmin2Repository, Admin2Repository>();
            services.AddScoped<IStoreRepository, StoreRepository>();
            services.AddScoped<IImageRepository, ImageRepository>();
            services.AddScoped<IBannerRepository, BannerRepository>();
            services.AddScoped<ISubscribeRepository, SubscribeRepository>();



            // Register Services
            services.AddScoped<IBrandService, BrandService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IAddressService, AddressService>();
            services.AddScoped<IAgentService, AgentService>();
            services.AddScoped<IAttributeService, AttributeService>();
            services.AddScoped<ICartService, CartService>();
            services.AddScoped<ICartItemService, CartItemService>();
            services.AddScoped<IFormFieldService, FormFieldService>();
            services.AddScoped<IFormsService, FormsService>();
            services.AddScoped<IFormSubmissionService, FormSubmissionService>();
            services.AddScoped<IFormSubmissionFieldService, FormSubmissionFieldService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IOrderDetailService, OrderDetailService>();
            services.AddScoped<IPageService, PageService>();
            services.AddScoped<IPageContentService, PageContentService>();
            services.AddScoped<IPaymentService, PaymentService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IProductDetailService, ProductDetailService>();
            services.AddScoped<IReviewService, ReviewService>();
            services.AddScoped<ISEOService, SEOService>();
            services.AddScoped<IShippingService, ShippingService>();
            services.AddScoped<IWishlistService, WishlistService>();
            services.AddScoped<IAdminService, AdminService>();
            services.AddScoped<IStoreService, StoreService>();
            services.AddScoped<IImageService, ImageService>();
            services.AddScoped<IBannerService, BannerService>();
            services.AddScoped<ISubscribeService, SubscribeService>();




            // Register Swagger services
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "My API",
                    Version = "v1",
                    Description = @"
A simple example ASP.NET Core Web API

<div  style='display: flex; flex-wrap: wrap;'>
   <h1> Download Json File </h1>
    <div  style='flex: 1; padding-right: 10px;'>
        <table style='width: 100%; border-collapse: collapse;'>
            <tr>
                <th style='text-align: left; padding: 8px; border-bottom: 2px solid #000;'>Name</th>
                <th style='text-align: left; padding: 8px; border-bottom: 2px solid #000;'>Download</th>
            </tr>
            <tr>
                <td style='padding: 8px; border-bottom: 1px solid #ddd;'>Address</td>
                <td style='padding: 8px; border-bottom: 1px solid #ddd;'>
                    <a href='/Download/Postscript/Address.json' download>Download</a>
                </td>
            </tr>
            <tr>
                <td style='padding: 8px; border-bottom: 1px solid #ddd;'>Agent</td>
                <td style='padding: 8px; border-bottom: 1px solid #ddd;'>
                    <a href='/Download/Postscript/Agent.json' download>Download</a>
                </td>
            </tr>
            <tr>
                <td style='padding: 8px; border-bottom: 1px solid #ddd;'>Attribute</td>
                <td style='padding: 8px; border-bottom: 1px solid #ddd;'>
                    <a href='/Download/Postscript/Attribute.json' download>Download</a>
                </td>
            </tr>
            <tr>
                <td style='padding: 8px; border-bottom: 1px solid #ddd;'>Brand</td>
                <td style='padding: 8px; border-bottom: 1px solid #ddd;'>
                    <a href='/Download/Postscript/Brand.json' download>Download</a>
                </td>
            </tr>
            <tr>
                <td style='padding: 8px; border-bottom: 1px solid #ddd;'>Cart</td>
                <td style='padding: 8px; border-bottom: 1px solid #ddd;'>
                    <a href='/Download/Postscript/Cart.json' download>Download</a>
                </td>
            </tr>
            <tr>
                <td style='padding: 8px; border-bottom: 1px solid #ddd;'>CartItem</td>
                <td style='padding: 8px; border-bottom: 1px solid #ddd;'>
                    <a href='/Download/Postscript/CartItem.json' download>Download</a>
                </td>
            </tr>
            <tr>
                <td style='padding: 8px; border-bottom: 1px solid #ddd;'>Category</td>
                <td style='padding: 8px; border-bottom: 1px solid #ddd;'>
                    <a href='/Download/Postscript/Category.json' download>Download</a>
                </td>
            </tr>
            <tr>
                <td style='padding: 8px; border-bottom: 1px solid #ddd;'>FormField</td>
                <td style='padding: 8px; border-bottom: 1px solid #ddd;'>
                    <a href='/Download/Postscript/FormField.json' download>Download</a>
                </td>
            </tr>
            <tr>
                <td style='padding: 8px; border-bottom: 1px solid #ddd;'>Form</td>
                <td style='padding: 8px; border-bottom: 1px solid #ddd;'>
                    <a href='/Download/Postscript/Form.json' download>Download</a>
                </td>
            </tr>
             <tr>
                <td style='padding: 8px; border-bottom: 1px solid #ddd;'>FormSubmission</td>
                <td style='padding: 8px; border-bottom: 1px solid #ddd;'>
                    <a href='/Download/Postscript/FormSubmission.json' download>Download</a>
                </td>
            </tr>
            <tr>
                <td style='padding: 8px; border-bottom: 1px solid #ddd;'>FormSubmissionField</td>
                <td style='padding: 8px; border-bottom: 1px solid #ddd;'>
                    <a href='/Download/Postscript/FormSubmissionField.json' download>Download</a>
                </td>
            </tr>
            <tr>
                <td style='padding: 8px; border-bottom: 1px solid #ddd;'>OrderDetail</td>
                <td style='padding: 8px; border-bottom: 1px solid #ddd;'>
                    <a href='/Download/Postscript/OrderDetail.json' download>Download</a>
                </td>
            </tr>
            <tr>
                <td style='padding: 8px; border-bottom: 1px solid #ddd;'>Order</td>
                <td style='padding: 8px; border-bottom: 1px solid #ddd;'>
                    <a href='/Download/Postscript/Order.json' download>Download</a>
                </td>
            </tr>
            <tr>
                <td style='padding: 8px; border-bottom: 1px solid #ddd;'>PageContent</td>
                <td style='padding: 8px; border-bottom: 1px solid #ddd;'>
                    <a href='/Download/Postscript/PageContent.json' download>Download</a>
                </td>
            </tr>
            <tr>
                <td style='padding: 8px; border-bottom: 1px solid #ddd;'>Page</td>
                <td style='padding: 8px; border-bottom: 1px solid #ddd;'>
                    <a href='/Download/Postscript/Page.json' download>Download</a>
                </td>
            </tr>
            <tr>
                <td style='padding: 8px; border-bottom: 1px solid #ddd;'>Payment</td>
                <td style='padding: 8px; border-bottom: 1px solid #ddd;'>
                    <a href='/Download/Postscript/Payment.json' download>Download</a>
                </td>
            </tr>
            <tr>
                <td style='padding: 8px; border-bottom: 1px solid #ddd;'>ProductDetail</td>
                <td style='padding: 8px; border-bottom: 1px solid #ddd;'>
                    <a href='/Download/Postscript/ProductDetail.json' download>Download</a>
                </td>
            </tr>
            <tr>
                <td style='padding: 8px; border-bottom: 1px solid #ddd;'>Product</td>
                <td style='padding: 8px; border-bottom: 1px solid #ddd;'>
                    <a href='/Download/Postscript/Product.json' download>Download</a>
                </td>
            </tr>
            <tr>
                <td style='padding: 8px; border-bottom: 1px solid #ddd;'>Review</td>
                <td style='padding: 8px; border-bottom: 1px solid #ddd;'>
                    <a href='/Download/Postscript/Review.json' download>Download</a>
                </td>
            </tr>
            <tr>
                <td style='padding: 8px; border-bottom: 1px solid #ddd;'>SEO</td>
                <td style='padding: 8px; border-bottom: 1px solid #ddd;'>
                    <a href='/Download/Postscript/SEO.json' download>Download</a>
                </td>
            </tr>
            <tr>
                <td style='padding: 8px; border-bottom: 1px solid #ddd;'>Shipping</td>
                <td style='padding: 8px; border-bottom: 1px solid #ddd;'>
                    <a href='/Download/Postscript/Shipping.json' download>Download</a>
                </td>
            </tr>
            <tr>
                <td style='padding: 8px; border-bottom: 1px solid #ddd;'>Wishlist</td>
                <td style='padding: 8px; border-bottom: 1px solid #ddd;'>
                    <a href='/Download/Postscript/Wishlist.json' download>Download</a>
                </td>
            </tr>
        </table>
    </div>
    <div style='flex: 1; padding-left: 10px;'>
        <table style='width: 100%; border-collapse: collapse;'>
        </table>
    </div>
</div>
",



                    Contact = new OpenApiContact
                    {
                        Name = "Your Name",
                        Email = string.Empty,
                        Url = new Uri("https://twitter.com/yourname"),
                    },
                });
            });


            // Other configurations...
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();

            // app.UseStaticFiles(); // Ensure static files are served

            // Enable serving static files (including files outside of wwwroot)
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new Microsoft.Extensions.FileProviders.PhysicalFileProvider(
                    System.IO.Path.Combine(env.ContentRootPath, "Download")),
                RequestPath = "/Download"
            });

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                c.RoutePrefix = string.Empty; // To serve Swagger UI at application's root (http://localhost:<port>/)

                // Include the custom JavaScript file
               // c.InjectJavascript("/js/swagger-custom.js");

            });
            app.UseCors("AllowSpecificOrigin"); // Enable the CORS policy

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        
    }
}
