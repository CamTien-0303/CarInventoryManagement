using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using phamthicamtien.Data;

namespace phamthicamtien.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ReportController(AppDbContext context)
        {
            _context = context;
        }

        // GET: /api/reports/inventory-aging
        [HttpGet("inventory-aging")]
        public async Task<IActionResult> GetInventoryAging(int alertDays = 90)
        {
            var today = DateTime.Now;

            var agingVehicles = await _context.Vehicles
                .Where(v => v.Status == "In_stock")
                .Select(v => new
                {
                    v.Vin,
                    v.CurrentLocationDetail,
                    ImportDate = _context.Transactions
                        .Where(t => t.Vin == v.Vin && t.Type == "Import")
                        .Select(t => t.TransactionDate)
                        .FirstOrDefault()
                })
                .ToListAsync();

            var result = agingVehicles
                .Select(v => new
                {
                    v.Vin,
                    v.CurrentLocationDetail,
                    DaysInStock = v.ImportDate != default ? (today - v.ImportDate).Days : 0,
                    NeedsMaintenance = v.ImportDate != default && (today - v.ImportDate).Days > alertDays
                })
                .OrderByDescending(v => v.DaysInStock)
                .ToList();

            return Ok(result);
        }
    }
}