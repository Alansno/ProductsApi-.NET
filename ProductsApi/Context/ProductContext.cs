using Microsoft.EntityFrameworkCore;
using ProductsApi.Models;

namespace ProductsApi.Context
{
    public class ProductContext: DbContext
    {
        public ProductContext(DbContextOptions<ProductContext> options) : base(options)
        {
            
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .HasMany(p => p.Products)
                .WithOne(p => p.User)
                .HasForeignKey(p => p.UserId)
                .IsRequired();
        }
    }
}
