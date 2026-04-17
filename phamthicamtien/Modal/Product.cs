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

        [Required]
        public int Year { get; set; }

        [Required]
        [StringLength(50)]
        public string EngineType { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string BaseColor { get; set; } = string.Empty;

        // Quan hệ: Một mẫu xe có thể có nhiều xe cụ thể (Vehicle)
        public ICollection<Vehicle>? Vehicles { get; set; }
    }
}