using phamthicamtien.Model;

namespace phamthicamtien.Data
{
    public static class DbSeeder
    {
        public static void Seed(AppDbContext context)
        {
            // Seed Warehouses
            if (!context.Warehouses.Any())
            {
                var warehouses = new List<Warehouse>
                {
                    new Warehouse { Name = "Kho Tổng Hồ Chí Minh", Address = "Quận 9, TP.HCM", Capacity = 500 },
                    new Warehouse { Name = "Kho Miền Bắc", Address = "Gia Lâm, Hà Nội", Capacity = 300 }
                };
                context.Warehouses.AddRange(warehouses);
                context.SaveChanges();
            }

            // Seed Staffs
            if (!context.Staffs.Any())
            {
                var staffs = new List<Staff>
                {
                    new Staff { FullName = "Nguyễn Văn Admin", DepartmentRole = "Manager", Phone = "0901234567", Email = "admin@example.com", Status = true },
                    new Staff { FullName = "Trần Thị Kho", DepartmentRole = "Warehouse_Staff", Phone = "0901234568", Email = "kho@example.com", Status = true },
                    new Staff { FullName = "Lê Văn Sale", DepartmentRole = "Sales", Phone = "0901234569", Email = "sale@example.com", Status = true }
                };
                context.Staffs.AddRange(staffs);
                context.SaveChanges();
            }

            // Seed Suppliers
            if (!context.Suppliers.Any())
            {
                var suppliers = new List<Supplier>
                {
                    new Supplier { SupplierName = "Toyota Vietnam", ContactInfo = "contact@toyota.vn - 0987654321" },
                    new Supplier { SupplierName = "Honda Vietnam", ContactInfo = "contact@honda.vn - 0987654322" }
                };
                context.Suppliers.AddRange(suppliers);
                context.SaveChanges();
            }

            // Seed Products
            if (!context.Products.Any())
            {
                var products = new List<Product>
                {
                    new Product { Brand = "Toyota", ModelName = "Camry 2.0Q", Year = 2024, EngineType = "Gasoline", BaseColor = "White" },
                    new Product { Brand = "Toyota", ModelName = "Fortuner Legender", Year = 2024, EngineType = "Diesel", BaseColor = "Black" },
                    new Product { Brand = "Honda", ModelName = "CR-V L", Year = 2024, EngineType = "Turbo", BaseColor = "Red" },
                    new Product { Brand = "Ford", ModelName = "Everest Titanium", Year = 2024, EngineType = "Bi-Turbo Diesel", BaseColor = "Silver" },
                    new Product { Brand = "Mazda", ModelName = "CX-5 Premium", Year = 2024, EngineType = "Gasoline", BaseColor = "Red" }
                };
                context.Products.AddRange(products);
                context.SaveChanges();
            }

            // Seed Vehicles
            if (!context.Vehicles.Any() && context.Products.Any() && context.Warehouses.Any())
            {
                var pId1 = context.Products.FirstOrDefault(p => p.ModelName.Contains("Camry"))?.ProductId ?? 1;
                var pId2 = context.Products.FirstOrDefault(p => p.ModelName.Contains("CR-V"))?.ProductId ?? 2;
                var pId3 = context.Products.FirstOrDefault(p => p.ModelName.Contains("Everest"))?.ProductId ?? 3;

                var wId1 = context.Warehouses.First().WarehouseId;
                var wId2 = context.Warehouses.Skip(1).FirstOrDefault()?.WarehouseId ?? wId1;

                var vehicles = new List<Vehicle>
                {
                    new Vehicle { Vin = "JT153WKA000123456", EngineNumber = "2AR-123456", ChassisNumber = "WKA-123456", ProductId = pId1, WarehouseId = wId1, Status = "In_stock", CurrentLocationDetail = "Khu A - Lô 1" },
                    new Vehicle { Vin = "JT153WKA000123457", EngineNumber = "2AR-123457", ChassisNumber = "WKA-123457", ProductId = pId1, WarehouseId = wId1, Status = "Reserved", CurrentLocationDetail = "Khu A - Lô 2" },
                    new Vehicle { Vin = "MLH53WKA000123458", EngineNumber = "15T-123458", ChassisNumber = "WKA-123458", ProductId = pId2, WarehouseId = wId2, Status = "In_stock", CurrentLocationDetail = "Khu B - Lô 1" },
                    new Vehicle { Vin = "MLH53WKA000123459", EngineNumber = "15T-123459", ChassisNumber = "WKA-123459", ProductId = pId2, WarehouseId = wId2, Status = "Sold", CurrentLocationDetail = "Khu Giao Xe" },
                    new Vehicle { Vin = "FMA53WKA000123460", EngineNumber = "BT-123460", ChassisNumber = "WKA-123460", ProductId = pId3, WarehouseId = wId1, Status = "In_stock", CurrentLocationDetail = "Khu C - Lô 1" }
                };
                context.Vehicles.AddRange(vehicles);
                context.SaveChanges();
            }
        }
    }
}
