using System.ComponentModel.DataAnnotations.Schema;

namespace Thesis_LUab558.Server.Models
{
    public class Wishlist
    {
        [Column("wishlist_id")]
        public int WishlistId { get; set; } // Primärschlüssel

        [Column("user_id")]
        public int UserId { get; set; }

        [Column("product_id")]
        public int ProductId { get; set; }

        [Column("added_to_wishlist_at")]
        public DateTime AddedToWishlistAt { get; set; }
    }
}
