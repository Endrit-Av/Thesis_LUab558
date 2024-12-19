using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Thesis_LUab558.Server.Models
{
    public class InvoiceItem
    {
        [Key]
        [Column("item_id")]
        public int ItemId { get; set; } // Primärschlüssel

        [Column("invoice_id")]
        public int InvoiceId { get; set; }

        [Column("product_id")]
        public int ProductId { get; set; }

        [Column("quantity")]
        public int Quantity { get; set; }

        [Column("price_per_unit")]
        public decimal PricePerUnit { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }
    }
}
