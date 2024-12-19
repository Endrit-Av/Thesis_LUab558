using Microsoft.EntityFrameworkCore;
using Thesis_LUab558.Server.Data;
using Thesis_LUab558.Server.Models;
using AutoMapper;
using Thesis_LUab558.Server.DTO;

namespace Thesis_LUab558.Server.Services
{
    public class TestService
    {
        private readonly NasDbContext _context;
        private readonly IMapper _mapper;

        public TestService(NasDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<UserDto>> GetFirst10UsersAsync()
        {
            var users = await _context.Users
                .OrderBy(u => u.UserId) // Sortierung nach ID
                .Take(10) // Nur die ersten 10 Produkte
                .ToListAsync();

            return _mapper.Map<List<UserDto>>(users);
        }

        public async Task<List<ProductDto>> GetFirst10ProductsAsync()
        {
            var products = await _context.Products
                .OrderBy(p => p.ProductId) 
                .Take(10) 
                .ToListAsync();

            return _mapper.Map<List<ProductDto>>(products);
        }

        public async Task<List<ReviewDto>> GetFirst10ReviewsAsync()
        {
            var reviews = await _context.Reviews
                .OrderBy(r => r.ReviewId)
                .Take(10)
                .ToListAsync();

            return _mapper.Map<List<ReviewDto>>(reviews);
        }

        public async Task<List<ImageDto>> GetFirst10ImagesAsync()
        {
            var images = await _context.Images
                .OrderBy(i => i.ImageId)
                .Take(10)
                .ToListAsync();

            return _mapper.Map<List<ImageDto>>(images);
        }
    }
}
