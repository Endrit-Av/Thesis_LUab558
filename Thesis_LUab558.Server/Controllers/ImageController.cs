﻿using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
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
        [ResponseCache(Duration = 300, Location = ResponseCacheLocation.Client, NoStore = false)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetImagesByAttributes(string productName, string color)
        {
            var images = await _imageService.GetImagesByProductAttributesAsync(productName, color);
            if (images == null) return NotFound("Keine Bilder gefunden.");
            return Ok(images);
        }

        [HttpGet("banner-images")]
        [ResponseCache(Duration = 300, Location = ResponseCacheLocation.Client, NoStore = false)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetBannerImages()
        {
            try
            {
                var images = _imageService.GetBannerImages();
                return Ok(images);
            }
            catch (DirectoryNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Ein Fehler ist aufgetreten.", details = ex.Message });
            }
        }
    }
}
