using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Thesis_LUab558.Server.Entity
{
    [Table("invoice_details")]
    public class InvoiceDetail
    {
        [Key]
        [Column("invoice_id")]
        public int InvoiceId { get; set; } // Primärschlüssel

        [Column("user_id")]
        public int UserId { get; set; }

        [Column("invoice_date")]
        public string InvoiceDate { get; set; }

        [Column("total_amount")]
        public decimal TotalAmount { get; set; }

        [Column("payment_method")]
        public string PaymentMethod { get; set; }

        [Column("payment_status")]
        public string PaymentStatus { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }
    }
}
