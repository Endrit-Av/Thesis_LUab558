﻿using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Thesis_LUab558.Server.DTO;
using Thesis_LUab558.Server.Services;

namespace Thesis_LUab558.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MainpageController : ControllerBase
    {
        private readonly MainpageService _mainpageService;

        public MainpageController(MainpageService mainpageService)
        {
            _mainpageService = mainpageService;
        }

        [HttpGet("categories")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerResponse(200)]
        [SwaggerResponse(500, "Interner Serverfehler.")]
        public async Task<IActionResult> GetCategories()
        {
            var categories = await _mainpageService.GetCategoriesAsync();
            return Ok(categories);
        }

        [HttpGet("products/{category}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerResponse(200, "Die Produkte dieser Kategorie wurden erfolgreich geladen.", typeof(IEnumerable<ProductDto>))]
        [SwaggerResponse(500, "Interner Serverfehler.")]
        public async Task<IActionResult> GetProductsByCategory(string category)
        {
            var products = await _mainpageService.GetProductsByCategoryAsync(category);
            if (products == null || !products.Any())
            {
                return NotFound("Keine Produkte für diese Kategorie gefunden.");
            }
            return Ok(products);
        }

        [HttpGet("banner-images")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerResponse(200, "Bilder erfolgreich geladen.")]
        public IActionResult GetBannerImages()
        {
            try
            {
                var images = _mainpageService.GetBannerImages();
                return Ok(images);
            }
            catch (DirectoryNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ein Fehler ist aufgetreten: " + ex.Message);
            }
        }
    }
}
