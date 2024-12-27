using Microsoft.AspNetCore.Mvc;
using Thesis_LUab558.Server.Services;

namespace Thesis_LUab558.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartController : ControllerBase
    {
        private readonly CartService _cartService;

        public CartController(CartService cartService)
        {
            _cartService = cartService;
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddToCart(int productId)
        {
            try
            {
                var cartItem = await _cartService.AddToCartAsync(productId);
                return Ok(cartItem);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("remove")]
        public async Task<IActionResult> RemoveFromCart(int productId)
        {
            try
            {
                await _cartService.RemoveFromCartAsync(productId);
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetCartItems()
        {
            var cartItems = await _cartService.GetCartItemsAsync();
            return Ok(cartItems);
        }
    }
}
