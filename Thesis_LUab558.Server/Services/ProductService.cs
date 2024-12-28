using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Thesis_LUab558.Server.Data;
using Thesis_LUab558.Server.DTO;
using Thesis_LUab558.Server.Models;

namespace Thesis_LUab558.Server.Services
{
    public class ProductService
    {
        private readonly NasDbContext _context;
        private readonly IMapper _mapper;

        public ProductService(NasDbContext context, IMapper mapper)
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

        public List<string> GetBannerImages()
        {
            var bannerDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images", "Banner");
            if (!Directory.Exists(bannerDirectory))
            {
                throw new DirectoryNotFoundException("Banner-Ordner nicht gefunden.");
            }

            var images = Directory.GetFiles(bannerDirectory)
                                  .Select(Path.GetFileName)
                                  .Select(fileName => $"https://localhost:7219/Images/Banner/{fileName}")
                                  .ToList();

            return images;
        }

        public async Task<ProductDto?> GetProductByIdAsync(int id)
        {
            var product = await _context.Products
                //.Include(p => p.Images) // Lade Bilder mit
                .FirstOrDefaultAsync(p => p.ProductId == id);

            if (product == null) return null;

            var dto = _mapper.Map<ProductDto>(product);
            //dto.Images = product.Images.Select(img => Convert.ToBase64String(img.ImageData)).ToList();
            return dto;
        }
        public async Task<ProductVariantDto> GetProductVariantsAsync(string productName)
        {
            var variants = await _context.Products
                .Where(p => p.ProductName == productName)
                .GroupBy(p => p.ProductName)
                .Select(group => new ProductVariantDto
                {
                    AvailableColors = group.Select(p => p.Color).Distinct().ToList(),
                    AvailableRam = group.Select(p => p.Ram).Distinct().ToList(),
                    AvailableMemory = group.Select(p => p.PhysicalMemory).Distinct().ToList()
                })
                .FirstOrDefaultAsync();

            return variants ?? new ProductVariantDto(); // Rückgabe eines leeren DTO, falls keine Varianten gefunden wurden
        }

        public async Task<ProductDto> GetProductDetailsAsync(string productName, string color, int ram, int physicalMemory)
        {
            var product = await _context.Products
                .FirstOrDefaultAsync(p =>
                    p.ProductName == productName &&
                    p.Color == color &&
                    p.Ram == ram &&
                    p.PhysicalMemory == physicalMemory);

            if (product == null)
            {
                throw new KeyNotFoundException("Das angeforderte Produkt wurde nicht gefunden.");
            }

            var productDto = _mapper.Map<ProductDto>(product);

            return productDto;
        }
    }
}
