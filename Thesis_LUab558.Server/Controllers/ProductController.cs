using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Thesis_LUab558.Server.DTO;
using Thesis_LUab558.Server.Services;

namespace Thesis_LUab558.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly ProductService _productService;

        public ProductController(ProductService productService)
        {
            _productService = productService;
        }

        [HttpGet("categories")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerResponse(200)]
        [SwaggerResponse(500, "Interner Serverfehler.")]
        public async Task<IActionResult> GetCategories()
        {
            var categories = await _productService.GetCategoriesAsync();
            return Ok(categories);
        }

        [HttpGet("products/{category}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerResponse(200, "Die Produkte dieser Kategorie wurden erfolgreich geladen.", typeof(IEnumerable<ProductDto>))]
        [SwaggerResponse(500, "Interner Serverfehler.")]
        public async Task<IActionResult> GetProductsByCategory(string category)
        {
            var products = await _productService.GetProductsByCategoryAsync(category);
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
                var images = _productService.GetBannerImages();
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

        [HttpGet("product/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetProductById(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null) return NotFound("Produkt nicht gefunden.");
            return Ok(product);
        }

        [HttpGet("product/variants/{productName}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetProductVariants(string productName)
        {
            var variants = await _productService.GetProductVariantsAsync(productName);
            if (variants == null) return NotFound("Keine Varianten gefunden.");
            return Ok(variants);
        }

        [HttpGet("product/details")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetProductDetails([FromQuery] string productName, [FromQuery] string color, [FromQuery] int ram, [FromQuery] int physicalMemory)
        {
            var product = await _productService.GetProductDetailsAsync(productName, color, ram, physicalMemory);
            if (product == null) return NotFound("Produkt nicht gefunden.");
            return Ok(product);
        }
    }
}
