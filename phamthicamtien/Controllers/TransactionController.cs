using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using phamthicamtien.Data;
using phamthicamtien.Model;
using phamthicamtien.DTOs;

namespace phamthicamtien.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TransactionController(AppDbContext context)
        {
            _context = context;
        }

        // GET: /api/Transaction
        [HttpGet]
        public async Task<IActionResult> GetTransactions()
        {
            var transactions = await _context.Transactions.ToListAsync();
            return Ok(transactions);
        }

        // GET: /api/Transaction/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTransaction(long id)
        {
            var transaction = await _context.Transactions.FindAsync(id);
            if (transaction == null) return NotFound("Không tìm thấy giao dịch.");
            return Ok(transaction);
        }

        // POST: /api/Transaction/import
        [HttpPost("import")]
        public async Task<IActionResult> ImportVehicle([FromBody] VehicleImportDto dto)
        {
            if (dto.Vin.Length != 17)
                return BadRequest("Số VIN phải đủ 17 ký tự chuẩn quốc tế.");

            if (await _context.Vehicles.AnyAsync(v => v.Vin == dto.Vin))
                return Conflict("Lỗi trùng số VIN. Dữ liệu đã tồn tại!");

            // Warehouse validation
            var warehouse = await _context.Warehouses.FindAsync(dto.WarehouseId);
            if (warehouse == null) return NotFound("Kho không tồn tại.");

            var currentCount = await _context.Vehicles.CountAsync(v => v.WarehouseId == dto.WarehouseId && (v.Status == "In_stock" || v.Status == "Reserved"));
            if (currentCount >= warehouse.Capacity)
            {
                return BadRequest("Kho đã đầy, không thể nhập thêm xe.");
            }

            var vehicle = new Vehicle
            {
                Vin = dto.Vin,
                EngineNumber = dto.EngineNumber,
                ChassisNumber = dto.ChassisNumber,
                ProductId = dto.ProductId,
                WarehouseId = dto.WarehouseId,
                CurrentLocationDetail = dto.CurrentLocationDetail,
                Status = "In_stock"
            };

            var transaction = new Transaction
            {
                Vin = dto.Vin,
                StaffId = dto.StaffId,
                Type = "Import",
                TransactionDate = DateTime.Now,
                Price = dto.ImportPrice
            };

            _context.Vehicles.Add(vehicle);
            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();

            return Ok("Nhập kho thành công.");
        }

        // POST: /api/Transaction/export
        [HttpPost("export")]
        public async Task<IActionResult> ExportVehicle([FromBody] ExportRequestDto dto)
        {
            var vehicle = await _context.Vehicles
                .Include(v => v.Documents)
                .FirstOrDefaultAsync(v => v.Vin == dto.Vin);

            if (vehicle == null)
                return NotFound("Không tìm thấy xe.");

            if (vehicle.Status != "Reserved" && vehicle.Status != "In_stock")
            {
                return BadRequest("Xe không ở trạng thái hợp lệ để xuất kho.");
            }

            var hasIncompleteDocs = vehicle.Documents.Any(d => d.Status != "Completed");
            if (hasIncompleteDocs)
            {
                return BadRequest("Lỗi Kế toán: Xe chưa hoàn tất các thủ tục pháp lý. Từ chối xuất kho!");
            }

            vehicle.Status = "Sold";

            var exportTransaction = new Transaction
            {
                Vin = dto.Vin,
                StaffId = dto.StaffId,
                Type = "Export",
                TransactionDate = DateTime.Now,
                Price = dto.ExportPrice
            };

            _context.Transactions.Add(exportTransaction);
            await _context.SaveChangesAsync();

            return Ok("Xuất kho và bàn giao xe thành công!");
        }
    }
}