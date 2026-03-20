using System.ComponentModel.DataAnnotations;

namespace ConnectDB.Models
{
    public class Warehouse
    {
        [Key]
        public int WarehouseId { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        public string? Address { get; set; }

        public int Capacity { get; set; }

        // Quan hệ: Một kho có thể chứa nhiều xe
        public ICollection<Vehicle>? Vehicles { get; set; }
    }
}