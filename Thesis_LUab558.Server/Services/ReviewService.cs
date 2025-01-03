using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Thesis_LUab558.Server.Data;
using Thesis_LUab558.Server.DTO;

namespace Thesis_LUab558.Server.Services
{
    public class ReviewService
    {
        private readonly NasDbContext _context;
        private readonly IMapper _mapper;

        public ReviewService(NasDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        //public async Task<IEnumerable<ReviewDto>> GetReviewsByProductIdAsync(int productId)
        //{
        //    var reviews = await _context.Reviews
        //        .Where(r => r.ProductId == productId)
        //        .ToListAsync();

        //    return _mapper.Map<IEnumerable<ReviewDto>>(reviews);
        //}

        public async Task<IEnumerable<ReviewDto>> GetReviewsByProductIdAsync(int productId)
        {
            // Lade Reviews und bindet die User-Navigationseigenschaft mit ein
            var reviews = await _context.Reviews
                .Include(r => r.User)
                .Where(r => r.ProductId == productId)
                .ToListAsync();

            // Mappe die geladenen Reviews auf ReviewDto
            return _mapper.Map<IEnumerable<ReviewDto>>(reviews);
        }

        public async Task<double> GetAverageRatingByProductIdAsync(int productId)
        {
            var ratings = await _context.Reviews
                .Where(r => r.ProductId == productId)
                .Select(r => r.Rating)
                .ToListAsync();

            if (!ratings.Any()) return 0;

            return Math.Round(ratings.Average(), 0);
        }
    }
}
