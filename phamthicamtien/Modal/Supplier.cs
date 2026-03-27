using System.ComponentModel.DataAnnotations;

namespace phamthicamtien.Model
{
    public class Supplier
    {
        [Key]
        public int SupplierId { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        public string? Address { get; set; }
        public string? ContactPerson { get; set; }
        public string? Phone { get; set; }

        // Một nhà cung cấp có thể cung cấp nhiều mẫu xe (Product)
        public ICollection<Product>? Products { get; set; }
    }
}