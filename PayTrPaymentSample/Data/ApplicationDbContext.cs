using Microsoft.EntityFrameworkCore;
using PayTrPaymentSample.Data.Entities;

namespace PayTrPaymentSample
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed data
            modelBuilder.Entity<User>().HasData(
                new User { Id = 1, UserName = "single", FirstName = "Single", LastName = "User", Address = "address", PhoneNumber = "1231231231", Email = "single@mail.com" }
            );

            modelBuilder.Entity<Product>().HasData(
                new Product { Id = 1, Name = "Product 1", Price = 10m },
                new Product { Id = 2, Name = "Product 2", Price = 20m },
                new Product { Id = 3, Name = "Product 3", Price = 30m }
            );
        }


        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
    }
}
