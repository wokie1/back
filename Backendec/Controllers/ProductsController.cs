using Microsoft.AspNetCore.Mvc;
using System;
using Backendec.Models;
using Microsoft.EntityFrameworkCore;

namespace Backendec.Controllers
{
    [ApiController]
    [Route("api/products")]
    public class ProductsController : ControllerBase
    {
        private readonly AppDbContext _context;
        public ProductsController(AppDbContext context) => _context = context;

        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            var products = await _context.Products.ToListAsync();
            return Ok(products);
        }

        [HttpGet("expired")]
        public async Task<IActionResult> GetExpiredProducts()
        {
            var expired = await _context.Products
                .Where(p => p.ExpiryDate < DateTime.Now)
                .ToListAsync();
            return Ok(expired);
        }

        [HttpGet("{productId}/stores")]
        public async Task<IActionResult> GetStoresWithProduct(int productId)
        {
            var storesWithProduct = await _context.Availabilitys
                .Where(a => a.ProductId == productId && a.Quantity > 0)
                .Join(_context.Stores,
                      a => a.StoreId,
                      s => s.Id,
                      (a, s) => s)
                .Distinct()
                .ToListAsync();

            return Ok(storesWithProduct);
        }
    }
}
