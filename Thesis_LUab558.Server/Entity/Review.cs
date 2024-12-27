using System.ComponentModel.DataAnnotations.Schema;

namespace Thesis_LUab558.Server.Models
{
    public class Review
    {
        [Column("review_id")]
        public int ReviewId { get; set; } // Primärschlüssel

        [Column("user_id")]
        public int UserId { get; set; }

        [Column("product_id")]
        public int ProductId { get; set; }

        [Column("rating")]
        public int Rating { get; set; }

        [Column("review_text")]
        public string ReviewText { get; set; }

        [Column("review_date")]
        public string ReviewDate { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        // Navigationseigenschaft für User
        [ForeignKey("UserId")]
        public User User { get; set; }
    }
}
