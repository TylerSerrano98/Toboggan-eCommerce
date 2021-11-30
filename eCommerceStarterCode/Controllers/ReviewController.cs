using eCommerceStarterCode.Data;
using eCommerceStarterCode.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace eCommerceStarterCode.Controllers
{
    [Route("api/review")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly ApplicationDbContext _context;


        public ReviewController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult GetAllReviews()
        {
            var reviews = _context.Reviews;
            return Ok(reviews);

        }

        [HttpPost]
        public IActionResult Post([FromBody] Review value)
        {
            _context.Reviews.Add(value);
            _context.SaveChanges();
            return StatusCode(201, value);
        }

        [HttpGet("{productId}")]
        public IActionResult getReviewByProductId(int productId)
        {
            var reviews = _context.Reviews.Include(r => r.User).Where(r => r.ProductId == productId);                                                      
            return Ok(reviews);

        }

        [HttpGet("averagerating{productId}")]
        public IActionResult getAverageRating(int productId)
        {
            var ratingSum = _context.Reviews.Where(r => r.ProductId == productId).Select(a => a.Rating).Sum();
            var reviewsCount = _context.Reviews.Where(r => r.ProductId == productId).Count();
            var averageRating = 0;

            if (reviewsCount == 0)
            {
                averageRating = 0;
            }
            else if (reviewsCount > 0)
            {
                averageRating = ratingSum / reviewsCount;
            }
            return Ok(averageRating);

        }

    }
}