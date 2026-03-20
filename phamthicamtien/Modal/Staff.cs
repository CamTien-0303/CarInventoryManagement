using System.ComponentModel.DataAnnotations;

namespace ConnectDB.Models
{
    public class Staff
    {
        [Key]
        public int StaffId { get; set; }

        [Required]
        [StringLength(100)]
        public string FullName { get; set; } = string.Empty;

        [Required]
        public string Role { get; set; } = "Staff"; // Admin, Warehouse, Sales

        public string? Email { get; set; }

        // Một nhân viên có thể thực hiện nhiều giao dịch
        public ICollection<Transaction>? Transactions { get; set; }
    }
}