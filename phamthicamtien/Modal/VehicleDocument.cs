using phamthicamtien.Model;
using System.ComponentModel.DataAnnotations;

namespace phamthicamtien.Model
{
    public class VehicleDocument
    {
        [Key]
        public int DocumentId { get; set; }

        public string Vin { get; set; } = string.Empty;

        public string DocumentType { get; set; } = string.Empty;

        // Đây chính là dòng bị thiếu làm nó báo lỗi nè 👇
        public string Status { get; set; } = "Pending";

        public DateTime? IssueDate { get; set; }

        public string? FileUrl { get; set; }

        public Vehicle? Vehicle { get; set; }
    }
}