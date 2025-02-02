using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Thesis_LUab558.Server.Data;
using Thesis_LUab558.Server.DTO;

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
                .Where(p => p.Category.ToLower() != "kopfhörer")
                .Select(p => p.Category)
                .Distinct()
                .ToListAsync();

            return categories;
        }

        public async Task<List<ProductDto>> GetProductsByCategoryAsync(string category)
        {
            var products = await _context.Products
                .Where(p => p.Category.ToLower() == category.ToLower())
                .GroupBy(p => p.Brand)
                .OrderBy(g => g.Key == "Apple" ? 0 : g.Key == "Samsung" ? 1 : 2)
                .Select(g => g.OrderBy(p => p.Price).FirstOrDefault())
                .ToListAsync();

            var productDtos = _mapper.Map<List<ProductDto>>(products);

            foreach (var dto in productDtos)
            {
                dto.ImageUrl = GenerateImageUrl(dto.ProductName, dto.Color);
                dto.Description = GetDescriptionFirstSentence(dto.Description);
            }

            return productDtos;
        }

        private string GenerateImageUrl(string productName, string color)
        {
            var sanitizedProductName = productName.Replace("Galaxy", "", StringComparison.OrdinalIgnoreCase).Trim();
            return $"https://localhost:7219/Images/ProductMain/{sanitizedProductName.Replace(" ", "_").ToLower()}_{color.ToLower()}_main.jpeg";
        }

        private string GetDescriptionFirstSentence(string description)
        {
            if (string.IsNullOrWhiteSpace(description)) return "Keine Beschreibung verfügbar.";
            var sentences = description.Split(new[] { ". " }, StringSplitOptions.RemoveEmptyEntries);
            return sentences.Length > 0 ? sentences[0]  + "." : "Keine Beschreibung verfügbar.";
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

            return variants ?? new ProductVariantDto();
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
