using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Thesis_LUab558.Server.Data;
using Thesis_LUab558.Server.DTO;

namespace Thesis_LUab558.Server.Services
{
    public class MainpageService
    {
        private readonly NasDbContext _context;
        private readonly IMapper _mapper;

        public MainpageService(NasDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<string>> GetCategoriesAsync()
        {
            var categories = await _context.Products
                .Where(p => p.Category.ToLower() != "kopfhörer") // Kopfhörer ausschließen
                .Select(p => p.Category) // Nur die Kategorie-Spalte auswählen
                .Distinct() // Doppelte Kategorien entfernen
                .ToListAsync();

            return categories;
        }

        public async Task<List<ProductDto>> GetProductsByCategoryAsync(string category)
        {
            var products = await _context.Products
                .Where(p => p.Category.ToLower() == category.ToLower()) // Filter nach Kategorie
                .GroupBy(p => p.Brand) // Gruppieren nach Brand
                .OrderBy(g => g.Key == "Apple" ? 0 : g.Key == "Samsung" ? 1 : 2) // Sortiere nach Markenreihenfolge
                .Select(g => g.OrderBy(p => p.Price).FirstOrDefault()) // Wähle das günstigste Produkt pro Marke
                .ToListAsync();

            var productDtos = _mapper.Map<List<ProductDto>>(products);

            foreach (var dto in productDtos)
            {
                var product = products.First(p => p.ProductId == dto.ProductId);

                var sanitizedProductName = product.ProductName.Replace("Galaxy", "", StringComparison.OrdinalIgnoreCase).Trim();
                dto.ImageUrl = $"https://localhost:7219/Images/ProductMain/{sanitizedProductName.Replace(" ", "_").ToLower()}_{product.Color.ToLower()}_main.jpeg";

                dto.Description = GetDescriptionFirstSentence(product.Description);
            }

            return productDtos;
        }

        private string GetDescriptionFirstSentence(string description)
        {
            if (string.IsNullOrWhiteSpace(description)) return "Keine Beschreibung verfügbar.";
            var sentences = description.Split(new[] { ". " }, StringSplitOptions.RemoveEmptyEntries);
            return sentences.Length > 0 ? sentences[0] : "Keine Beschreibung verfügbar.";
        }
    }
}
