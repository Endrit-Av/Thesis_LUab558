using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Thesis_LUab558.Server.Data;
using Thesis_LUab558.Server.DTO;
using Thesis_LUab558.Server.Models;

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

            // Prüfen, ob das Produkt bereits im Warenkorb ist
            var cartItem = await _context.Carts.FirstOrDefaultAsync(c => c.ProductId == productId);

            if (cartItem == null)
            {
                cartItem = new Cart
                {
                    UserId = 0, // Dummy-User-ID
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

            // Reduziere den Vorrat
            product.Stock--;

            await _context.SaveChangesAsync();

            return _mapper.Map<CartDto>(cartItem);
        }

        public async Task RemoveFromCartAsync(int productId)
        {
            // Finde das Cart-Item basierend auf der ProductId und Dummy-User-ID
            var cartItem = await _context.Carts.FirstOrDefaultAsync(c => c.ProductId == productId && c.UserId == 0);

            if (cartItem == null)
            {
                throw new InvalidOperationException("Das Produkt befindet sich nicht im Warenkorb.");
            }

            // Erhöhe den Vorrat basierend auf der reservierten Menge
            var product = await _context.Products.FirstOrDefaultAsync(p => p.ProductId == productId);
            if (product != null)
            {
                product.Stock += cartItem.Quantity;
            }

            // Entferne den Warenkorbeintrag vollständig
            _context.Carts.Remove(cartItem);

            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<CartDto>> GetCartItemsAsync()
        {
            var cartItems = await _context.Carts
                .Include(c => c.Product) // Lade Produktinformationen
                .ToListAsync();

            var cartItemsDto = _mapper.Map<IEnumerable<CartDto>>(cartItems);

            foreach (var dto in cartItemsDto)
            {
                var cartItem = cartItems.First(p => p.ProductId == dto.ProductId);

                var sanitizedProductName = cartItem.Product.ProductName.Replace("Galaxy", "", StringComparison.OrdinalIgnoreCase).Trim();
                dto.Product.ImageUrl = $"https://localhost:7219/Images/ProductMain/{sanitizedProductName.Replace(" ", "_").ToLower()}_{cartItem.Product.Color.ToLower()}_main.jpeg";

            }

            return cartItemsDto;
        }

    }
}
