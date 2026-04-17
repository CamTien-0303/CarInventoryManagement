using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using phamthicamtien.Data;
using phamthicamtien.Model;

namespace phamthicamtien.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProductController(AppDbContext context)
        {
            _context = context;
        }

        // 1. GET: api/Product (Lấy danh sách sản phẩm)
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            return await _context.Products.ToListAsync();
        }

        // 2. GET: api/Product/5 (Lấy chi tiết 1 sản phẩm)
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null) return NotFound("Không tìm thấy sản phẩm");

            return product;
        }

        // 3. POST: api/Product (Tạo mới sản phẩm)
        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            // Trả về sản phẩm vừa tạo kèm đường dẫn lấy chi tiết
            return CreatedAtAction(nameof(GetProduct), new { id = product.ProductId }, product);
        }

        // 4. PUT: api/Product/5 (Cập nhật sản phẩm)
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, Product product)
        {
            if (id != product.ProductId) return BadRequest("ID không khớp");

            _context.Entry(product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Products.Any(e => e.ProductId == id)) return NotFound();
                else throw;
            }

            return Ok("Cập nhật thành công");
        }

        // 5. DELETE: api/Product/5 (Xóa sản phẩm)
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return NotFound();

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return Ok("Đã xóa sản phẩm");
        }
    }
}