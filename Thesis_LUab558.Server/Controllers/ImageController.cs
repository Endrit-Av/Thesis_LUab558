using Microsoft.AspNetCore.Mvc;
using Thesis_LUab558.Server.Services;

namespace Thesis_LUab558.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ImageController : ControllerBase
    {
        private readonly ImageService _imageService;

        public ImageController(ImageService imageService)
        {
            _imageService = imageService;
        }

        [HttpGet("attributes")]
        public async Task<IActionResult> GetImagesByAttributes(string productName, string color)
        {
            var images = await _imageService.GetImagesByProductAttributesAsync(productName, color);
            return Ok(images);
        }
    }
}
