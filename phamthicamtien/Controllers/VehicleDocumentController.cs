using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using phamthicamtien.Data;
using phamthicamtien.Model;

namespace phamthicamtien.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehicleDocumentController : ControllerBase
    {
        private readonly AppDbContext _context;

        public VehicleDocumentController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/VehicleDocument
        [HttpGet]
        public async Task<ActionResult<IEnumerable<VehicleDocument>>> GetVehicleDocuments()
        {
            return await _context.VehicleDocuments.ToListAsync();
        }

        // GET: api/VehicleDocument/5
        [HttpGet("{id}")]
        public async Task<ActionResult<VehicleDocument>> GetVehicleDocument(int id)
        {
            var document = await _context.VehicleDocuments.FindAsync(id);

            if (document == null) return NotFound("Không tìm thấy giấy tờ xe");

            return document;
        }

        // POST: api/VehicleDocument
        [HttpPost]
        public async Task<ActionResult<VehicleDocument>> PostVehicleDocument(VehicleDocument document)
        {
            _context.VehicleDocuments.Add(document);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetVehicleDocument), new { id = document.DocumentId }, document);
        }

        // PUT: api/VehicleDocument/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutVehicleDocument(int id, VehicleDocument document)
        {
            if (id != document.DocumentId) return BadRequest("ID không khớp");

            _context.Entry(document).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.VehicleDocuments.Any(e => e.DocumentId == id)) return NotFound();
                else throw;
            }

            return NoContent();
        }

        // DELETE: api/VehicleDocument/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVehicleDocument(int id)
        {
            var document = await _context.VehicleDocuments.FindAsync(id);
            if (document == null) return NotFound();

            _context.VehicleDocuments.Remove(document);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
