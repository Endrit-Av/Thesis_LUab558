﻿using System.ComponentModel.DataAnnotations.Schema;

namespace Thesis_LUab558.Server.Entity
{
    [Table("cart")]
    public class Cart
    {
        [Column("cart_id")]
        public int CartId { get; set; }

        [Column("user_id")]
        public int UserId { get; set; }

        [Column("product_id")]
        public int ProductId { get; set; }

        [Column("quantity")]
        public int Quantity { get; set; }

        [Column("added_to_cart_at")]
        public DateTime AddedToCartAt { get; set; }

        [ForeignKey("ProductId")]
        public Product? Product { get; set; }
    }
}
