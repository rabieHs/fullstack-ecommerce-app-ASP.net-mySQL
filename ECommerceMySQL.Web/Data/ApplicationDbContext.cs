using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using ECommerceMySQL.Web.Models;

namespace ECommerceMySQL.Web.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed Categories
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Electronics", Description = "Electronic devices and gadgets" },
                new Category { Id = 2, Name = "Clothing", Description = "Fashion and apparel" },
                new Category { Id = 3, Name = "Books", Description = "Books and literature" },
                new Category { Id = 4, Name = "Home & Garden", Description = "Home decor and garden supplies" },
                new Category { Id = 5, Name = "Sports", Description = "Sports equipment and accessories" },
                new Category { Id = 6, Name = "Toys", Description = "Toys and games" }
            );

            // Seed Products
            modelBuilder.Entity<Product>().HasData(
                // Electronics
                new Product { Id = 1, Name = "Smartphone", Description = "Latest model smartphone", Price = 699.99M, ImageUrl = "/images/products/phone.jpg", CategoryId = 1 },
                new Product { Id = 2, Name = "Laptop", Description = "Powerful laptop for work and gaming", Price = 1299.99M, ImageUrl = "/images/products/laptop.jpg", CategoryId = 1 },
                new Product { Id = 3, Name = "Headphones", Description = "Wireless noise-canceling headphones", Price = 199.99M, ImageUrl = "/images/products/headphones.jpg", CategoryId = 1 },
                new Product { Id = 4, Name = "Smartwatch", Description = "Fitness tracking smartwatch", Price = 249.99M, ImageUrl = "/images/products/smartwatch.jpg", CategoryId = 1 },

                // Clothing
                new Product { Id = 5, Name = "T-Shirt", Description = "Cotton casual t-shirt", Price = 19.99M, ImageUrl = "/images/products/tshirt.jpg", CategoryId = 2 },
                new Product { Id = 6, Name = "Jeans", Description = "Classic blue jeans", Price = 49.99M, ImageUrl = "/images/products/jeans.jpg", CategoryId = 2 },
                new Product { Id = 7, Name = "Sneakers", Description = "Comfortable running shoes", Price = 79.99M, ImageUrl = "/images/products/sneakers.jpg", CategoryId = 2 },
                new Product { Id = 8, Name = "Jacket", Description = "Waterproof winter jacket", Price = 89.99M, ImageUrl = "/images/products/jacket.jpg", CategoryId = 2 },

                // Books
                new Product { Id = 9, Name = "Novel", Description = "Bestselling fiction novel", Price = 14.99M, ImageUrl = "/images/products/novel.jpg", CategoryId = 3 },
                new Product { Id = 10, Name = "Cookbook", Description = "Gourmet cooking recipes", Price = 24.99M, ImageUrl = "/images/products/cookbook.jpg", CategoryId = 3 },
                new Product { Id = 11, Name = "Self-Help Book", Description = "Personal development guide", Price = 19.99M, ImageUrl = "/images/products/selfhelp.jpg", CategoryId = 3 },
                new Product { Id = 12, Name = "History Book", Description = "World history encyclopedia", Price = 29.99M, ImageUrl = "/images/products/history.jpg", CategoryId = 3 },

                // Home & Garden
                new Product { Id = 13, Name = "Table Lamp", Description = "Modern LED table lamp", Price = 39.99M, ImageUrl = "/images/products/lamp.jpg", CategoryId = 4 },
                new Product { Id = 14, Name = "Plant Pot", Description = "Ceramic plant pot set", Price = 24.99M, ImageUrl = "/images/products/pot.jpg", CategoryId = 4 },
                new Product { Id = 15, Name = "Throw Pillow", Description = "Decorative throw pillow", Price = 19.99M, ImageUrl = "/images/products/pillow.jpg", CategoryId = 4 },
                new Product { Id = 16, Name = "Wall Clock", Description = "Modern wall clock", Price = 34.99M, ImageUrl = "/images/products/clock.jpg", CategoryId = 4 },

                // Sports
                new Product { Id = 17, Name = "Yoga Mat", Description = "Non-slip yoga mat", Price = 29.99M, ImageUrl = "/images/products/yoga.jpg", CategoryId = 5 },
                new Product { Id = 18, Name = "Dumbbell Set", Description = "Adjustable dumbbell set", Price = 149.99M, ImageUrl = "/images/products/dumbbells.jpg", CategoryId = 5 },
                new Product { Id = 19, Name = "Basketball", Description = "Official size basketball", Price = 24.99M, ImageUrl = "/images/products/basketball.jpg", CategoryId = 5 },
                new Product { Id = 20, Name = "Tennis Racket", Description = "Professional tennis racket", Price = 89.99M, ImageUrl = "/images/products/racket.jpg", CategoryId = 5 },

                // Toys
                new Product { Id = 21, Name = "LEGO Set", Description = "Building blocks set", Price = 49.99M, ImageUrl = "/images/products/lego.jpg", CategoryId = 6 },
                new Product { Id = 22, Name = "Board Game", Description = "Family board game", Price = 29.99M, ImageUrl = "/images/products/boardgame.jpg", CategoryId = 6 },
                new Product { Id = 23, Name = "RC Car", Description = "Remote control car", Price = 59.99M, ImageUrl = "/images/products/rccar.jpg", CategoryId = 6 },
                new Product { Id = 24, Name = "Puzzle", Description = "1000-piece puzzle", Price = 19.99M, ImageUrl = "/images/products/puzzle.jpg", CategoryId = 6 }
            );
        }
    }
}
