﻿using Microsoft.AspNetCore.Mvc;
using System;
using Backendec.Models;
using Microsoft.EntityFrameworkCore;

namespace Backendec.Controllers
{
    /// <summary>
    /// данный контроллер нужен чтобы отдавать данные о статистике и выборку товаров
    /// </summary>

    [ApiController]
    [Route("api/stats")]
    public class StatsController : ControllerBase
    {
        private readonly AppDbContext _context;
        public StatsController(AppDbContext context) => _context = context;

        [HttpGet("avg-prices")]
        public async Task<IActionResult> GetAveragePricesByGroup()
        {
            var result = await _context.Products
                .GroupBy(p => p.GroupCode)
                .Select(g => new {
                    GroupCode = g.Key,
                    AvgPrice = g.Average(p => p.Price)
                })
                .ToListAsync();

            return Ok(result);
        }

        [HttpGet("top-sales")]
        public async Task<IActionResult> GetTopSales()
        {
            var result = await _context.Sales
                .GroupBy(s => s.ProductId)
                .Select(g => new {
                    ProductId = g.Key,
                    TotalSales = g.Count()
                })
                .OrderByDescending(x => x.TotalSales)
                .Take(10)
                .ToListAsync();

            return Ok(result);
        }
    }
}
