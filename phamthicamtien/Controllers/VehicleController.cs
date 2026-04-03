using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using phamthicamtien.Data;
using phamthicamtien.Model;
using phamthicamtien.DTOs;

namespace phamthicamtien.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehicleController : ControllerBase
    {
        private readonly AppDbContext _context;
        public VehicleController(AppDbContext context) { _context = context; }

        // GET: /api/vehicles/{vin} - Lấy chi tiết xe theo VIN [cite: 85]
        [HttpGet("{vin}")]
        public async Task<IActionResult> GetVehicleByVin(string vin)
        {
            var vehicle = await _context.Vehicles
                .Include(v => v.Product)
                .Include(v => v.Warehouse)
                .Include(v => v.Documents) // Lấy kèm thông tin giấy tờ 
                .FirstOrDefaultAsync(v => v.Vin == vin);

            if (vehicle == null) return NotFound("Không tìm thấy xe.");
            return Ok(vehicle);
        }

        // PATCH: /api/vehicles/location - Cập nhật vị trí xe [cite: 85]
        [HttpPatch("location")]
        public async Task<IActionResult> UpdateLocation(string vin, string newLocation, int staffId)
        {
            var vehicle = await _context.Vehicles.FindAsync(vin);
            if (vehicle == null) return NotFound();

            var oldLocation = vehicle.CurrentLocationDetail;
             vehicle.CurrentLocationDetail = newLocation;

           // QA/QC: Lưu log ai đã dời xe từ đâu đến đâu [cite: 87]
            // Thực tế bạn có thể lưu vào bảng SystemLogs (nếu có thiết kế thêm)
            Console.WriteLine($"Staff {staffId} moved VIN {vin} from {oldLocation} to {newLocation}");

            await _context.SaveChangesAsync();
            return Ok("Cập nhật vị trí thành công.");
        }
    }
}