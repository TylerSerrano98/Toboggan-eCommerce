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
    [Route("api/product")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ApplicationDbContext _context;


        public ProductController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult GetAllProducts()
        {
            var product = _context.Products;
            return Ok(product);

        }

        [HttpPost]
        public IActionResult Post([FromBody] Product value)
        {
            _context.Products.Add(value);
            _context.SaveChanges();
            return StatusCode(201, value);
        }

        [HttpGet("searchresults/{searchTerm}")]
        public IActionResult getSearchResults(string searchTerm)
        {
            var product = _context.Products.Include(p => p.Category)
                .Where(p => p.ProductName.ToLower()
                .Contains(searchTerm.ToLower()))
                .ToList();
            return Ok(product);

        }

        [HttpGet("{productID}")]
        public IActionResult GetProductDetails(int productId)
        {
            var product = _context.Products.Include(p => p.Category).Where(p => p.ProductId == productId);
            return Ok(product);

        }

        [HttpGet("{productId}details")]
        public IActionResult GetProductDetailsOnly(int productId)
        {
            var productOnly = _context.Products.Where(p => p.ProductId == productId);
            return Ok(productOnly);

        }

    }
}
