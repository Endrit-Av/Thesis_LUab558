using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Thesis_LUab558.Server.Data;
using Thesis_LUab558.Server.DTO;
using Thesis_LUab558.Server.Entity;

namespace Thesis_LUab558.Server.Services
{
    public class CartService
    {
        private readonly NasDbContext _context;
        private readonly IMapper _mapper;

        public CartService(NasDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<CartDto> AddToCartAsync(int productId)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.ProductId == productId);

            if (product == null || product.Stock <= 0)
            {
                throw new InvalidOperationException("Produkt ist nicht verfügbar.");
            }

            var cartItem = await _context.Carts.FirstOrDefaultAsync(c => c.ProductId == productId);

            if (cartItem == null)
            {
                cartItem = new Cart
                {
                    UserId = 1,
                    ProductId = productId,
                    Quantity = 1,
                    AddedToCartAt = DateTime.UtcNow
                };
                _context.Carts.Add(cartItem);
            }
            else
            {
                cartItem.Quantity++;
            }

            product.Stock--;

            await _context.SaveChangesAsync();

            return _mapper.Map<CartDto>(cartItem);
        }

        public async Task RemoveFromCartAsync(int productId)
        {
            var cartItem = await _context.Carts.FirstOrDefaultAsync(c => c.ProductId == productId && c.UserId == 1);

            if (cartItem == null)
            {
                throw new InvalidOperationException("Das Produkt befindet sich nicht im Warenkorb.");
            }

            var product = await _context.Products.FirstOrDefaultAsync(p => p.ProductId == productId);
            if (product != null)
            {
                product.Stock += cartItem.Quantity;
            }

            _context.Carts.Remove(cartItem);

            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<CartDto>> GetCartItemsAsync()
        {
            var cartItems = await _context.Carts
                .Include(c => c.Product)
                .ToListAsync();

            var cartItemsDto = _mapper.Map<IEnumerable<CartDto>>(cartItems);

            foreach (var dto in cartItemsDto)
            {
                dto.Product.ImageUrl = GenerateImageUrl(dto.Product.ProductName, dto.Product.Color);
            }

            return cartItemsDto;
        }

        private string GenerateImageUrl(string productName, string color)
        {
            var sanitizedProductName = productName.Replace("Galaxy", "", StringComparison.OrdinalIgnoreCase).Trim();
            return $"https://localhost:7219/Images/ProductMain/{sanitizedProductName.Replace(" ", "_").ToLower()}_{color.ToLower()}_main.jpeg";
        }

    }
}
