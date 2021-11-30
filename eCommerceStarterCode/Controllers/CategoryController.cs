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
        public IActionResult getAllCategoriesResults(int categoryId)
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
            return Ok(categoriesWithProduct);

        }

        [HttpGet("searchresults/{searchTerm}")]
        public IActionResult getSearchResults(string searchTerm)
        {
            var category = _context.Categories.Where(p => p.CategoryName.ToLower().Contains(searchTerm.ToLower()));
            var productAndCategory = from cId in category
                                     join pcId in _context.Products
                                     on cId.CategoryId equals pcId.CategoryId
                                     select new
                                     {
                                         ProductId = pcId.ProductId,
                                         ProductName = pcId.ProductName,
                                         ProductPrice = pcId.ProductPrice,
                                         ProductDescription = pcId.ProductDescription,
                                         CategoryId = cId.CategoryId,
                                         CategoryName = cId.CategoryName
                                     };
            return Ok(productAndCategory);

        }

    }
}