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

        public List<string> GetCategories()
        {
            return new List<string> { "Smartphone", "Notebook", "Tablet" };
        }

        public async Task<List<string>> GetCategoriesAsync()
        {
            var categories = await _context.Products
                .Select(p => p.Category) // Nur die Kategorie-Spalte auswählen
                .Distinct() // Doppelte Kategorien entfernen
                .ToListAsync();

            return categories;
        }

        public async Task<List<ProductDto>> GetProductsByCategoryAsync(string category)
        {
            var products = await _context.Products
                .Where(p => p.Category.ToLower() == category.ToLower())
                .ToListAsync();

            return _mapper.Map<List<ProductDto>>(products);
        }
    }
}
