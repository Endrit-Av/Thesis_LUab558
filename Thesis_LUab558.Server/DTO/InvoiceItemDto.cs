namespace Thesis_LUab558.Server.DTO
{
    public class InvoiceItemDto
    {
        public int ItemId { get; set; }

        public int InvoiceId { get; set; }

        public int ProductId { get; set; }

        public int Quantity { get; set; }

        public decimal PricePerUnit { get; set; }
    }
}
