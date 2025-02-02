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
        [ResponseCache(Duration = 300, Location = ResponseCacheLocation.Client, NoStore = false)] // Cache für 5 Minuten
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCategories()
        {
            var categories = await _productService.GetCategoriesAsync();
            return Ok(categories);
        }

        [HttpGet("{category}")]
        [ResponseCache(Duration = 300, Location = ResponseCacheLocation.Client, NoStore = false)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ProductDto>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetProductsByCategory(string category)
        {
            var products = await _productService.GetProductsByCategoryAsync(category);
            if (products == null || !products.Any())
            {
                return NotFound("Keine Produkte für diese Kategorie gefunden.");
            }
            return Ok(products);
        }

        [HttpGet("variants/{productName}")]
        [ResponseCache(Duration = 300, Location = ResponseCacheLocation.Client, NoStore = false)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ProductVariantDto>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetProductVariants(string productName)
        {
            var variants = await _productService.GetProductVariantsAsync(productName);
            if (variants == null) return NotFound("Keine Varianten gefunden.");
            return Ok(variants);
        }

        [HttpGet("details")]
        [ResponseCache(Duration = 300, Location = ResponseCacheLocation.Client, NoStore = false)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ProductDto>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetProductDetails([FromQuery] string productName, [FromQuery] string color, [FromQuery] int ram, [FromQuery] int physicalMemory)
        {
            var product = await _productService.GetProductDetailsAsync(productName, color, ram, physicalMemory);
            if (product == null) return NotFound("Produkt nicht gefunden.");
            return Ok(product);
        }
    }
}
