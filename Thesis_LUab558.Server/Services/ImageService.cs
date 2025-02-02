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
            var productId = await _context.Products
                .Where(p => p.ProductName.ToLower() == productName.ToLower() && p.Color.ToLower() == color.ToLower())
                .OrderBy(p => p.ProductId) 
                .Select(p => p.ProductId)
                .LastOrDefaultAsync(); 

            if (productId == 0)
            {
                return new List<ImageDto>();
            }

            var images = await _context.Images
                .Where(i => i.ProductId == productId)
                .ToListAsync();

            return _mapper.Map<IEnumerable<ImageDto>>(images);
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
    }
}
