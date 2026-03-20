using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ConnectDB.Models
{
    public class Vehicle
    {
        [Key]
        [StringLength(17)] // Độ dài chuẩn mã VIN
        public string Vin { get; set; } = string.Empty;

        [Required]
        public string EngineNumber { get; set; } = string.Empty;

        [Required]
        public string ChassisNumber { get; set; } = string.Empty;

        public string Color { get; set; } = string.Empty;

        // Trạng thái: In_Stock, Reserved, Sold
        public string Status { get; set; } = "In_Stock";

        public string? CurrentLocationDetail { get; set; }

        // Khóa ngoại liên kết tới Product
        [Required]
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public Product? Product { get; set; }

        // Khóa ngoại liên kết tới Warehouse
        [Required]
        public int WarehouseId { get; set; }
        [ForeignKey("WarehouseId")]
        public Warehouse? Warehouse { get; set; }
    }
}