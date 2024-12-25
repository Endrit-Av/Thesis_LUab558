﻿using Microsoft.AspNetCore.Mvc;
using Thesis_LUab558.Server.Services;

namespace Thesis_LUab558.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReviewController : ControllerBase
    {
        private readonly ReviewService _reviewService;

        public ReviewController(ReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        [HttpGet("{productId}/reviews")]
        public async Task<IActionResult> GetReviews(int productId)
        {
            var reviews = await _reviewService.GetReviewsByProductIdAsync(productId);
            return Ok(reviews);
        }

        [HttpGet("{productId}/average-rating")]
        public async Task<IActionResult> GetAverageRating(int productId)
        {
            var averageRating = await _reviewService.GetAverageRatingByProductIdAsync(productId);
            return Ok(new { averageRating });
        }
    }
}