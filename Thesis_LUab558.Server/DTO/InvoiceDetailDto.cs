namespace Thesis_LUab558.Server.DTO
{
    public class InvoiceDetailDto
    {
        public int InvoiceId { get; set; }

        public int UserId { get; set; }

        public string InvoiceDate { get; set; }

        public decimal TotalAmount { get; set; }

        public string PaymentMethod { get; set; }

        public string PaymentStatus { get; set; }
    }
}
