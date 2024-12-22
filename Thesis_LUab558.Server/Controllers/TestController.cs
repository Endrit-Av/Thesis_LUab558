//using Microsoft.AspNetCore.Mvc;
//using Swashbuckle.AspNetCore.Annotations;
//using Thesis_LUab558.Server.DTO;
//using Thesis_LUab558.Server.Services;

//namespace Thesis_LUab558.Server.Controllers
//{
//    [ApiController]
//    [Route("api/[controller]")]
//    public class TestController : ControllerBase
//    {
//        private readonly TestService _testService;

//        public TestController(TestService testService)
//        {
//            _testService = testService;
//        }

//        /// <summary>
//        /// Gibt die ersten 10 Benutzer zurück.
//        /// </summary>
//        /// <returns>Eine Liste von Benutzern</returns>
//        [HttpGet("users/first10")]
//        [ProducesResponseType(StatusCodes.Status200OK)]
//        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
//        [SwaggerResponse(200, "Die ersten 10 Benutzer wurden erfolgreich abgerufen.", typeof(IEnumerable<UserDto>))]
//        [SwaggerResponse(500, "Interner Serverfehler.")]
//        public async Task<IActionResult> GetFirst10Users()
//        {
//            var users = await _testService.GetFirst10UsersAsync();
//            return Ok(users);
//        }

//        /// <summary>
//        /// Gibt die ersten 10 Produkte zurück.
//        /// </summary>
//        /// <returns>Eine Liste von Produkten</returns>
//        [HttpGet("products/first10")]
//        [ProducesResponseType(StatusCodes.Status200OK)]
//        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
//        [SwaggerResponse(200, "Die ersten 10 Produkte wurden erfolgreich abgerufen.", typeof(IEnumerable<ProductDto>))]
//        [SwaggerResponse(500, "Interner Serverfehler.")]
//        public async Task<IActionResult> GetFirst10Products()
//        {
//            var products = await _testService.GetFirst10ProductsAsync();
//            return Ok(products);
//        }

//        /// <summary>
//        /// Gibt die ersten 10 Bewertungen zurück.
//        /// </summary>
//        /// <returns>Eine Liste von Bewertungen</returns>
//        [HttpGet("reviews/first10")]
//        [ProducesResponseType(StatusCodes.Status200OK)]
//        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
//        [SwaggerResponse(200, "Die ersten 10 Bewertungen wurden erfolgreich abgerufen.", typeof(IEnumerable<ReviewDto>))]
//        [SwaggerResponse(500, "Interner Serverfehler.")]
//        public async Task<IActionResult> GetFirst10Reviews()
//        {
//            var reviews = await _testService.GetFirst10ReviewsAsync();
//            return Ok(reviews);
//        }

//        /// <summary>
//        /// Gibt die ersten 10 Bilder zurück.
//        /// </summary>
//        /// <returns>Eine Liste von Bildern</returns>
//        [HttpGet("images/first10")]
//        [ProducesResponseType(StatusCodes.Status200OK)]
//        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
//        [SwaggerResponse(200, "Die ersten 10 Bilder wurden erfolgreich abgerufen.", typeof(IEnumerable<ImageDto>))]
//        [SwaggerResponse(500, "Interner Serverfehler.")]
//        public async Task<IActionResult> GetFirst10Images()
//        {
//            var images = await _testService.GetFirst10ImagesAsync();
//            return Ok(images);
//        }
//    }
//}
