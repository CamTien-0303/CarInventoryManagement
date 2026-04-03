namespace phamthicamtien.DTOs
{
    public class ExportRequestDto
    {
        public string Vin { get; set; } = string.Empty;
        public int StaffId { get; set; }
        public decimal ExportPrice { get; set; }
    }
}