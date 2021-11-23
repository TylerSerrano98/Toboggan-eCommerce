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
    [Route("api/[controller]")]
    [ApiController]
    public class ShoppingCartController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
       

        public ShoppingCartController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet("{userId}")]
        public IActionResult Get(string UserId)
        {
            var shoppingCart = _context.ShoppingCart;
            var specificCart = shoppingCart
                .Where(sc => sc.UserId == UserId)
                .Include(sc => sc.Product)
                .Include(sc => sc.Product.Category)
                .ToList();
            return Ok(specificCart);
                 
        }
        
        [HttpPost]
        public IActionResult Post([FromBody] ShoppingCart value)
        {
            _context.ShoppingCart.Add(value);
            _context.SaveChanges();
            return StatusCode(201, value);
        }

        [HttpDelete("{productId}")]
        public IActionResult Delete(int productId)
        {
            var userId = User.FindFirstValue("id");
            var user = _context.Users.Find(userId);
            if (user == null)
            { 
                return NotFound("User not found");
            }

            var deleteProduct = _context.ShoppingCart
                .Where(dp => dp.UserId == userId && dp.ProductId == productId)
                .SingleOrDefault();

            _context.ShoppingCart.Remove(deleteProduct);
            _context.SaveChanges();
            return StatusCode(204, deleteProduct);
        }




            
    }
}
