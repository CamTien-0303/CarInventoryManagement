using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace phamthicamtien.Model
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }

        [Required]
        [StringLength(100)]
        public string Brand { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string ModelName { get; set; } = string.Empty;

        public int Year { get; set; }

        public string? EngineType { get; set; }

        public string? BaseColor { get; set; }

        public int? SupplierId { get; set; } // Khóa ngoại
        [ForeignKey("SupplierId")]
        public Supplier? Supplier { get; set; }

        // Quan hệ: Một mẫu xe có thể có nhiều xe cụ thể (Vehicle)
        public ICollection<Vehicle>? Vehicles { get; set; }
    }
}