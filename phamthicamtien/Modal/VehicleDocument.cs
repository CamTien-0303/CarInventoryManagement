using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ConnectDB.Models
{
    public class VehicleDocument
    {
        [Key]
        public int DocumentId { get; set; }

        [Required]
        public string DocumentType { get; set; } = string.Empty; // VAT, Đăng kiểm, Tờ khai HQ

        public string? DocumentNo { get; set; }
        public DateTime? IssueDate { get; set; }

        // Khóa ngoại nối với Vehicle qua mã VIN
        [Required]
        public string Vin { get; set; } = string.Empty;

        [ForeignKey("Vin")]
        public Vehicle? Vehicle { get; set; }
    }
}