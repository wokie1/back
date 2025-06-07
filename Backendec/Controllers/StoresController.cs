using Microsoft.AspNetCore.Mvc;
using System;
using Backendec.Models;
using Microsoft.EntityFrameworkCore;

namespace Backendec.Controllers
{
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
    }
}
