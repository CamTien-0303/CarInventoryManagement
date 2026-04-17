using System.ComponentModel.DataAnnotations;

namespace phamthicamtien.Model
{
    public class Staff
    {
        [Key]
        public int StaffId { get; set; }

        [Required]
        [StringLength(100)]
        public string FullName { get; set; } = string.Empty;

        [Required]
        public string DepartmentRole { get; set; } = "Staff"; // Warehouse_Staff, Sales, Accountant, Manager, Technician

        [Required]
        public string Phone { get; set; } = string.Empty;

        [Required]
        public string Email { get; set; } = string.Empty;

        [Required]
        public bool Status { get; set; } = true;

        // Một nhân viên có thể thực hiện nhiều giao dịch
        public ICollection<Transaction>? Transactions { get; set; }
    }
}