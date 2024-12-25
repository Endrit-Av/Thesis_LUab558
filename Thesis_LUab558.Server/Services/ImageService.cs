using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Thesis_LUab558.Server.Data;
using Thesis_LUab558.Server.DTO;

namespace Thesis_LUab558.Server.Services
{
    public class ImageService
    {

        private readonly NasDbContext _context;
        private readonly IMapper _mapper;

        public ImageService(NasDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ImageDto>> GetImagesByProductAttributesAsync(string productName, string color)
        {
            // Schritt 1: Finde die product_id basierend auf product_name und color
            var productId = await _context.Products
                .Where(p => p.ProductName.ToLower() == productName.ToLower() && p.Color.ToLower() == color.ToLower())
                .OrderBy(p => p.ProductId) // Sortiere nach product_id, falls es mehrere Einträge gibt
                .Select(p => p.ProductId)
                .LastOrDefaultAsync(); // Nimm die höchste product_id

            // Schritt 2: Falls keine product_id gefunden wurde, gib eine leere Liste zurück
            if (productId == 0)
            {
                return new List<ImageDto>();
            }

            // Schritt 3: Lade die Bilder basierend auf der ermittelten product_id
            var images = await _context.Images
                .Where(i => i.ProductId == productId)
                .ToListAsync();

            // Schritt 4: Mappe die Bilder auf ImageDto
            return _mapper.Map<IEnumerable<ImageDto>>(images);
        }
    }
}
