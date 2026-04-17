using System.ComponentModel.DataAnnotations;

namespace phamthicamtien.Model
{
    public class Supplier
    {
        [Key]
        public int SupplierId { get; set; }

        [Required]
        [StringLength(200)]
        public string SupplierName { get; set; } = string.Empty;

        [Required]
        public string ContactInfo { get; set; } = string.Empty;
    }
}