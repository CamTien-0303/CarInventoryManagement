namespace phamthicamtien.DTOs
{
    public class VehicleImportDto
    {
        public string Vin { get; set; } = string.Empty;
        public string EngineNumber { get; set; } = string.Empty;
        public string ChassisNumber { get; set; } = string.Empty;
        public int ProductId { get; set; }
        public int WarehouseId { get; set; }
        public decimal ImportPrice { get; set; }
        public int StaffId { get; set; }
        public string? CurrentLocationDetail { get; set; }
    }
}