using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace phamthicamtien.Model
{
    public class Transaction
    {
        [Key]
        public long TransactionId { get; set; }

        [Required]
        public string Vin { get; set; } = string.Empty;

        [ForeignKey("Vin")]
        public Vehicle? Vehicle { get; set; }


        [Required]
        public string Type { get; set; } = "Import"; // Import hoặc Export

        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        public DateTime TransactionDate { get; set; } = DateTime.Now;

        [Required]
        public int StaffId { get; set; } // Khóa ngoại
        [ForeignKey("StaffId")]
        public Staff? Staff { get; set; }


    }
}