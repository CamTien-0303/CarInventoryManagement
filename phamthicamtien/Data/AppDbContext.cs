using Microsoft.EntityFrameworkCore;
using phamthicamtien.Model;

namespace phamthicamtien.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // Khai báo các bảng dữ liệu
        public DbSet<Product> Products { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<Warehouse> Warehouses { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Staff> Staffs { get; set; }
        public DbSet<VehicleDocument> VehicleDocuments { get; set; }
    }
}