namespace Thesis_LUab558.Server.DTO
{
    public class ProductVariantDto
    {
            public List<string> AvailableColors { get; set; } = new List<string>();

            public List<int?> AvailableRam { get; set; } = new List<int?>();

            public List<int?> AvailableMemory { get; set; } = new List<int?>();
    }
}
