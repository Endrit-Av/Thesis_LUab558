using System.ComponentModel.DataAnnotations.Schema;

namespace Thesis_LUab558.Server.Entity
{
    [Table("products")]
    public class Product
    {
        [Column("product_id")]
        public int ProductId { get; set; } // Primärschlüssel

        [Column("brand")]
        public string Brand { get; set; }

        [Column("category")]
        public string Category { get; set; }

        [Column("product_name")]
        public string ProductName { get; set; }

        [Column("price")]
        public double Price { get; set; }

        [Column("physical_memory")]
        public int? PhysicalMemory { get; set; }

        [Column("ram")]
        public int? Ram { get; set; }

        [Column("color")]
        public string Color { get; set; }

        [Column("stock")]
        public int Stock { get; set; }

        [Column("description")]
        public string Description { get; set; }

        [Column("operating_system")]
        public string? OperatingSystem { get; set; }

        [Column("general_keyword")]
        public string GeneralKeyword { get; set; }

        [Column("added_to_database_at")]
        public DateTime AddedToDatabaseAt { get; set; }
    }
}
