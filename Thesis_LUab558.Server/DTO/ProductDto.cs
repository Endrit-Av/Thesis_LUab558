namespace Thesis_LUab558.Server.DTO
{
    public class ProductDto
    {
        public int ProductId { get; set; }

        public required string Brand { get; set; }

        public required string Category { get; set; }

        public required string ProductName { get; set; }

        public double Price { get; set; }

        public int PhysicalMemory { get; set; }

        public int Ram { get; set; }

        public required string Color { get; set; }

        public int Stock { get; set; }

        public required string Description { get; set; }

        public string OperatingSystem { get; set; }

        public string GeneralKeyword { get; set; }

        public string ImageUrl { get; set; }
    }
}
