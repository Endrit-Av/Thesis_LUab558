using System.ComponentModel.DataAnnotations.Schema;

namespace Thesis_LUab558.Server.Entity
{
    [Table("images")]
    public class Image
    {
        [Column("image_id")]
        public int ImageId { get; set; } // Primärschlüssel

        [Column("product_id")]
        public int ProductId { get; set; }

        [Column("image_byte")]
        public byte[] ImageByte { get; set; }
    }
}
