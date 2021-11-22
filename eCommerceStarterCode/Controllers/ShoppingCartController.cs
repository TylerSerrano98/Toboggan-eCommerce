using eCommerceStarterCode.Data;
using eCommerceStarterCode.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eCommerceStarterCode.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShoppingCartsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
       

        public ShoppingCartsController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet("{userId}")]
        public IActionResult Get(string UserId)
        {
            var shoppingCart = _context.ShoppingCart;
            var specificCart = shoppingCart.Include(sc => sc.Product).Include(sc => sc.Product.Category).ToList().Where(sc => sc.UserId == UserId);
            return Ok(specificCart);
                 
        }
        
        [HttpPost("Add")]
        public IActionResult Post([FromBody] ShoppingCart value)
        {
            _context.ShoppingCart.Add(value);
            _context.SaveChanges();
            return StatusCode(201, value);
        }

        [HttpDelete("{productId} / {userId}")]
        public IActionResult Remove(int productId, string UserId)
        {
            var deleteProduct = _context.ShoppingCart.Where(dp => dp.UserId == UserId && dp.ProductId == productId).SingleOrDefault();
            if (deleteProduct == null) 
            {
                return NotFound();

            }
            _context.ShoppingCart.Remove(deleteProduct);
            _context.SaveChanges();
            return Ok(deleteProduct);
        }




            
    }
}
