using Microsoft.EntityFrameworkCore;
using Thesis_LUab558.Server.Entity;
using Image = Thesis_LUab558.Server.Entity.Image;

namespace Thesis_LUab558.Server.Data

{
    public class NasDbContext : DbContext
    {
        public NasDbContext(DbContextOptions<NasDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        
        public DbSet<Product> Products { get; set; }
        
        public DbSet<Wishlist> Wishlists { get; set; }
        
        public DbSet<Review> Reviews { get; set; }
        
        public DbSet<Cart> Carts { get; set; }
        
        public DbSet<InvoiceDetail> InvoiceDetails { get; set; }
        
        public DbSet<InvoiceItem> InvoiceItems { get; set; }
        
        public DbSet<Image> Images { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("633867");

            modelBuilder.Entity<Cart>()
                .HasOne(c => c.Product)
                .WithMany()
                .HasForeignKey(c => c.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            
            modelBuilder.Entity<Review>()
                .HasOne(r => r.User) 
                .WithMany()
                .HasForeignKey(r => r.UserId) 
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
