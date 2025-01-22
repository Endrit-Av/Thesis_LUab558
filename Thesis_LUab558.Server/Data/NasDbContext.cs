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
            base.OnModelCreating(modelBuilder);

            // Setze das Standard-Schema auf "633867"
            modelBuilder.HasDefaultSchema("633867");

            // Konfiguration für die Cart-Entität
            modelBuilder.Entity<Cart>()
                .HasOne(c => c.Product) // Beziehung zur Product-Entität
                .WithMany() // Kein Rückverweis von Product zu Cart
                .HasForeignKey(c => c.ProductId) // Fremdschlüssel
                .OnDelete(DeleteBehavior.Restrict); // Verhindert Löschen von Produkten, die in einem Cart sind

            // Konfiguration für die Review-Entität
            modelBuilder.Entity<Review>()
                .HasOne(r => r.User) // Beziehung zur User-Entität
                .WithMany() // Kein Rückverweis von User zu Review
                .HasForeignKey(r => r.UserId) // Fremdschlüssel
                .OnDelete(DeleteBehavior.Restrict); // Verhindert Löschen von Benutzern mit Reviews
        }
    }
}
