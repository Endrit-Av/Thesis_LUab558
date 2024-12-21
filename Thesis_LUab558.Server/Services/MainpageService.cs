using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Thesis_LUab558.Server.Data;
using Thesis_LUab558.Server.DTO;
using Thesis_LUab558.Server.Models;

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

        public List<string> GetCategories()
        {
            return new List<string> { "Smartphone", "Notebook", "Tablet" };
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
                .Select(g => g.OrderBy(p => p.Price).FirstOrDefault()) // Wähle das günstigste Produkt pro Marke
                .ToListAsync();

            var productDtos = _mapper.Map<List<ProductDto>>(products);

            foreach (var dto in productDtos)
            {
                var product = products.First(p => p.ProductId == dto.ProductId);
                dto.ImageUrl = $"/assets/images/productmain/{product.ProductName.Replace(" ", "_")}_{product.Color}_Main.jpeg";
                dto.ImageUrl = dto.ImageUrl.ToLower();
                dto.Url = GetProductUrl(product);
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

        private string GetProductUrl(Product product)
        {
            var url = $"../Appls/ProductAppl.jsp?product_name={Uri.EscapeDataString(product.ProductName)}";
            if (!string.IsNullOrWhiteSpace(product.Color)) url += $"&color={Uri.EscapeDataString(product.Color)}";
            if (product.Ram > 0) url += $"&ram={product.Ram}";
            if (product.PhysicalMemory > 0) url += $"&physical_memory={product.PhysicalMemory}";
            return url;
        }
    }
}
