﻿using Microsoft.EntityFrameworkCore;
using Thesis_LUab558.Server.Models;
using Image = Thesis_LUab558.Server.Models.Image;

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

            // Verweist auf die eigentlichen Tabellen
            modelBuilder.Entity<User>().ToTable("users");

            modelBuilder.Entity<Product>().ToTable("products");

            modelBuilder.Entity<Wishlist>().ToTable("wishlist");

            modelBuilder.Entity<Review>().ToTable("reviews");

            modelBuilder.Entity<Cart>().ToTable("cart");

            modelBuilder.Entity<InvoiceDetail>().ToTable("invoice_details");

            modelBuilder.Entity<InvoiceItem>().ToTable("invoice_items");

            modelBuilder.Entity<Image>().ToTable("images");
        }
    }
}
