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
    [Route("api/category")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ApplicationDbContext _context;


        public CategoryController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult GetAllCategories()
        {
            var categories = _context.Categories;
            return Ok(categories);

        }

        [HttpPost]
        public IActionResult Post([FromBody] Category value)
        {
            _context.Categories.Add(value);
            _context.SaveChanges();
            return StatusCode(201, value);
        }

        [HttpGet("categoryresults")]
        public IActionResult getAllCategories(int categoryId)
        {
            var categories = _context.Categories
                .Where(p => p.CategoryId == categoryId);
            var categoriesWithProduct = from c in categories
                                        join cId in _context.Products
                                        on c.CategoryId equals cId.CategoryId
                                        select new
                                        {
                                            ProductId = cId.ProductId,
                                            ProductName = cId.ProductName,
                                            ProductPrice = cId.ProductPrice,
                                            ProductDescription = cId.ProductDescription,
                                            CategoryId = c.CategoryId,
                                            CategoryName = c.CategoryName
                                        };
            return Ok(categories);

        }

        [HttpGet("{productID}")]
        public IActionResult GetCategoryDetails(int productId)
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