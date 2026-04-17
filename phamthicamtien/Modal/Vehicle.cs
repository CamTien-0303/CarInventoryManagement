using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace phamthicamtien.Model
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

        // ==========================================
        // PHẦN BỔ SUNG: NAVIGATION PROPERTIES CHO QUAN HỆ 1-NHIỀU
        // ==========================================

        // 1 chiếc xe có thể có nhiều giao dịch (Nhập, Xuất, Dời kho...)
        public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();

        // 1 chiếc xe có nhiều loại giấy tờ (Thuế, Hải quan, Kiểm định...)
        public ICollection<VehicleDocument> Documents { get; set; } = new List<VehicleDocument>();
    }
}