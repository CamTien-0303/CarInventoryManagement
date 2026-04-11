using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema; // Thêm dòng này vô để xài ForeignKey

namespace phamthicamtien.Model
{
    public class VehicleDocument
    {
        [Key]
        public int DocumentId { get; set; }

        public string Vin { get; set; } = string.Empty;

        public string DocumentType { get; set; } = string.Empty;

        public string Status { get; set; } = "Pending";

        public DateTime? IssueDate { get; set; }

        public string? FileUrl { get; set; }

        [ForeignKey("Vin")] // 
        public Vehicle? Vehicle { get; set; }
    }
}