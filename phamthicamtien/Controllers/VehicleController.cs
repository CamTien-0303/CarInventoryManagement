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

        // GET: /api/Vehicle
        [HttpGet]
        public async Task<IActionResult> GetVehicles()
        {
            var vehicles = await _context.Vehicles
                .Include(v => v.Product)
                .Include(v => v.Warehouse)
                .ToListAsync();
            return Ok(vehicles);
        }

        // GET: /api/Vehicle/{vin}
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

        // POST: /api/Vehicle
        [HttpPost]
        public async Task<IActionResult> CreateVehicle(Vehicle vehicle)
        {
            if (vehicle.Vin.Length != 17) return BadRequest("VIN must be 17 characters");
            if (await _context.Vehicles.AnyAsync(v => v.Vin == vehicle.Vin)) return Conflict("VIN already exists.");
            
            _context.Vehicles.Add(vehicle);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetVehicleByVin), new { vin = vehicle.Vin }, vehicle);
        }

        // PUT: /api/Vehicle/{vin}
        [HttpPut("{vin}")]
        public async Task<IActionResult> UpdateVehicle(string vin, Vehicle vehicle)
        {
            if (vin != vehicle.Vin) return BadRequest("VIN mismatch");
            
            _context.Entry(vehicle).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.Vehicles.AnyAsync(v => v.Vin == vin)) return NotFound();
                else throw;
            }
            return NoContent();
        }

        // PATCH: /api/Vehicle/location
        [HttpPatch("location")]
        public async Task<IActionResult> UpdateLocation(string vin, [FromQuery] string newLocation, [FromQuery] int staffId)
        {
            var vehicle = await _context.Vehicles.FindAsync(vin);
            if (vehicle == null) return NotFound();

            var oldLocation = vehicle.CurrentLocationDetail;
            vehicle.CurrentLocationDetail = newLocation;

            // Optional: log to console or db
            Console.WriteLine($"Staff {staffId} moved VIN {vin} from {oldLocation} to {newLocation}");

            await _context.SaveChangesAsync();
            return Ok("Cập nhật vị trí thành công.");
        }

        // DELETE: /api/Vehicle/{vin}
        [HttpDelete("{vin}")]
        public async Task<IActionResult> DeleteVehicle(string vin)
        {
            var vehicle = await _context.Vehicles.FindAsync(vin);
            if (vehicle == null) return NotFound();
            
            _context.Vehicles.Remove(vehicle);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}