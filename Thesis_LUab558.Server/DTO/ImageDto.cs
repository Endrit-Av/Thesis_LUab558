using System.Text.Json.Serialization;

namespace Thesis_LUab558.Server.DTO
{
    public class ImageDto
    {
        public int ImageId { get; set; }

        public int ProductId { get; set; }

        [JsonIgnore]
        public byte[] ImageByte { get; set; }

        public string Base64Image => Convert.ToBase64String(ImageByte);
    }
}
