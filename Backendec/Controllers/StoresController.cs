using Microsoft.AspNetCore.Mvc;
using System;
using Backendec.Models;
using Microsoft.EntityFrameworkCore;

namespace Backendec.Controllers
{
    /// <summary>
    /// данный контроллер нам нужен чтобы отдавать с бэка все необходимые данные о магазинах
    /// </summary>

    [ApiController]
    [Route("api/stores")]
    public class StoresController : ControllerBase
    {
        private readonly AppDbContext _context;
        public StoresController(AppDbContext context) => _context = context;

        [HttpGet]
        public async Task<IActionResult> GetStores()
        {
            var stores = await _context.Stores.ToListAsync();
            return Ok(stores);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetStoreById(int id)
        {
            var store = await _context.Stores.FindAsync(id);
            if (store == null)
                return NotFound();

            return Ok(store);
        }

        [HttpGet("{storeId}/products")]
        public async Task<IActionResult> GetProductsInStore(int storeId)
        {
            var productsInStore = await _context.Availabilitys
                .Where(a => a.StoreId == storeId && a.Quantity > 0)
                .Join(_context.Products,
                      a => a.ProductId,
                      p => p.Id,
                      (a, p) => p)
                .Distinct()
                .ToListAsync();

            return Ok(productsInStore);
        }
    }
}
