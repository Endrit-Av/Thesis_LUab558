namespace Thesis_LUab558.Server.DTO
{
    public class CartDto
    {
        public int CartId { get; set; }

        public int UserId { get; set; }

        public int ProductId { get; set; }

        public int Quantity { get; set; }

        public ProductDto Product { get; set; }
    }
}
