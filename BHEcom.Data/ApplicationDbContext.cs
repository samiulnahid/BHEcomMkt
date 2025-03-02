using Microsoft.EntityFrameworkCore;
using BHEcom.Common.Models;
using System.Collections.Generic;
using System.Reflection.Emit;
using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace BHEcom.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Brand> Brands { get; set; }

        // DbSet is optional for stored procedure use
        public DbSet<Category> Categories { get; set; }

        public DbSet<Address> Addresses { get; set; }
        public DbSet<Agent> Agents { get; set; }
        public DbSet<Attributes> Attributes { get; set; }
        public DbSet<Cart> Cart { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<FormField> FormFields { get; set; }
        public DbSet<Forms> Forms { get; set; }
        public DbSet<FormSubmission> FormSubmissions { get; set; }
        public DbSet<FormSubmissionField> FormSubmissionFields { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Page> Pages { get; set; }
        public DbSet<PageContent> PageContents { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductDetail> ProductDetails { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<SEO> SEO { get; set; }
        public DbSet<Shipping> Shipping { get; set; }
        public DbSet<Wishlist> Wishlist { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Membership> Memberships { get; set; }
        public DbSet<Store> Stores { get; set; }
        public DbSet<Application> Applications { get; set; }
        public DbSet<StoreBrand> StoreBrands { get; set; }
        public DbSet<StoreCategory> StoreCategories { get; set; }
        public DbSet<StoreProductField> StoreProductFields { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UsersInRole> UsersInRoles { get; set; }
        public DbSet<Images> Images { get; set; }
        public DbSet<Banner> Banners { get; set; }
        public DbSet<CouponUsage> CouponUsages { get; set; }
        public DbSet<PageVisit> PageVisits { get; set; }
        public DbSet<Coupon> Coupons { get; set; }
        public DbSet<DeliveryDetails> DeliveryDetails { get; set; }
        public DbSet<DeliveryLog> DeliveryLogs { get; set; }
        public DbSet<Subscribe> Subscribe { get; set; }
        public DbSet<WishlistItem> WishlistItems { get; set; }
        public DbSet<CartManager> vw_EcommerceCart { get; set; }
        public DbSet<WishlistManager> vw_EcommerceWishlist { get; set; }
        public DbSet<OrderManager> vw_OrderListByUser { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Define mappings and configurations for EF if needed

            // Define composite No primary key for UsersInRole
           // modelBuilder.Entity<UsersInRole>().HasNoKey();
            modelBuilder.Entity<UsersInRole>()
           .HasKey(u => new { u.UserId, u.RoleId });

            modelBuilder.Entity<Membership>().HasNoKey();

            // Map the view to the CartItemViewModel
            modelBuilder.Entity<CartManager>().HasNoKey(); // Views don't have a primary key
            modelBuilder.Entity<CartManager>().ToView("vw_EcommerceCart"); // Replace with your actual view name

             modelBuilder.Entity<WishlistManager>().HasNoKey(); // Views don't have a primary key
            modelBuilder.Entity<WishlistManager>().ToView("vw_EcommerceWishlist"); // Replace with your actual view name
            
             modelBuilder.Entity<OrderManager>().HasNoKey(); // Views don't have a primary key
            modelBuilder.Entity<OrderManager>().ToView("vw_OrderListByUser"); // Replace with your actual view name

            modelBuilder.Entity<IdentityUser>(b =>
            {
                b.HasKey(u => u.Id);
                b.HasIndex(u => u.NormalizedUserName).HasDatabaseName("UserNameIndex").IsUnique();
                b.HasIndex(u => u.NormalizedEmail).HasDatabaseName("EmailIndex");
                b.ToTable("AspNetUsers");
            });

            modelBuilder.Entity<IdentityRole>(b =>
            {
                b.HasKey(r => r.Id);
                b.HasIndex(r => r.NormalizedName).HasDatabaseName("RoleNameIndex").IsUnique();
                b.ToTable("AspNetRoles");
            });

            modelBuilder.Entity<IdentityUserRole<string>>(b =>
            {
                b.HasKey(r => new { r.UserId, r.RoleId });
                b.ToTable("AspNetUserRoles");
            });

            modelBuilder.Entity<IdentityUserClaim<string>>(b =>
            {
                b.HasKey(uc => uc.Id);
                b.ToTable("AspNetUserClaims");
            });

            modelBuilder.Entity<IdentityUserLogin<string>>(b =>
            {
                b.HasKey(l => new { l.LoginProvider, l.ProviderKey });
                b.ToTable("AspNetUserLogins");
            });

            modelBuilder.Entity<IdentityRoleClaim<string>>(b =>
            {
                b.HasKey(rc => rc.Id);
                b.ToTable("AspNetRoleClaims");
            });

            modelBuilder.Entity<IdentityUserToken<string>>(b =>
            {
                b.HasKey(t => new { t.UserId, t.LoginProvider, t.Name });
                b.ToTable("AspNetUserTokens");
            });
        }
    }
}