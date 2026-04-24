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

        // GET: /api/Report/inventory-aging
        [HttpGet("inventory-aging")]
        public async Task<IActionResult> GetInventoryAging(int alertDays = 90)
        {
            var today = DateTime.Now;

            var agingVehicles = await _context.Vehicles
                .Include(v => v.Product)
                .Include(v => v.Warehouse)
                .Where(v => v.Status == "In_stock" || v.Status == "Reserved")
                .Select(v => new
                {
                    vin = v.Vin,
                    brand = v.Product != null ? v.Product.Brand : "N/A",
                    model_name = v.Product != null ? v.Product.ModelName : "N/A",
                    warehouse_name = v.Warehouse != null ? v.Warehouse.Name : "N/A",
                    status = v.Status,
                    current_location_detail = v.CurrentLocationDetail,
                    ImportDate = _context.Transactions
                        .Where(t => t.Vin == v.Vin && t.Type == "Import")
                        .Select(t => t.TransactionDate)
                        .FirstOrDefault()
                })
                .ToListAsync();

            var result = agingVehicles
                .Select(v => new
                {
                    v.vin,
                    v.brand,
                    v.model_name,
                    v.warehouse_name,
                    v.status,
                    v.current_location_detail,
                    days_in_inventory = v.ImportDate != default ? (today - v.ImportDate).Days : 0,
                    needs_maintenance = v.ImportDate != default && (today - v.ImportDate).Days > alertDays
                })
                .OrderByDescending(v => v.days_in_inventory)
                .ToList();

            return Ok(result);
        }

        // GET: /api/Report/summary
        [HttpGet("summary")]
        public async Task<IActionResult> GetSummary()
        {
            var totalProducts = await _context.Products.CountAsync();
            var totalVehicles = await _context.Vehicles.CountAsync();
            var inStock = await _context.Vehicles.CountAsync(v => v.Status == "In_stock" || v.Status == "In_Stock");
            var reserved = await _context.Vehicles.CountAsync(v => v.Status == "Reserved");
            var sold = await _context.Vehicles.CountAsync(v => v.Status == "Sold");
            var totalWarehouses = await _context.Warehouses.CountAsync();
            var totalStaff = await _context.Staffs.CountAsync();

            var result = new
            {
                totalProducts,
                totalVehicles,
                inStock,
                reserved,
                sold,
                totalWarehouses,
                totalStaff
            };

            return Ok(result);
        }
    }
}