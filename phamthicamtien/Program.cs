using phamthicamtien.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// =====================================================
// DATABASE CONFIGURATION
// Tự động dùng PostgreSQL khi deploy lên Render (có biến DATABASE_URL)
// Dùng SQL Server khi chạy local
// =====================================================
var databaseUrl = Environment.GetEnvironmentVariable("DATABASE_URL");

if (!string.IsNullOrEmpty(databaseUrl))
{
    // Render cung cấp DATABASE_URL dạng: postgres://user:pass@host:port/dbname
    // Chuyển sang dạng Npgsql connection string
    var uri = new Uri(databaseUrl);
    var userInfo = uri.UserInfo.Split(':', 2); // split tối đa 2 phần (password có thể chứa ':')
    var host = uri.Host;
    var dbPort = uri.Port == -1 ? 5432 : uri.Port; // dùng 5432 nếu không có port trong URL
    var database = uri.AbsolutePath.TrimStart('/');
    var username = Uri.UnescapeDataString(userInfo[0]);
    var password = Uri.UnescapeDataString(userInfo[1]); // decode ký tự đặc biệt trong password

    var npgsqlConnectionString =
        $"Host={host};Port={dbPort};Database={database};Username={username};Password={password};SSL Mode=Require;Trust Server Certificate=true";

    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseNpgsql(npgsqlConnectionString));
}
else
{
    // Local development: dùng SQL Server
    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
}

// =====================================================
// CORS – cho phép frontend gọi API
// =====================================================
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// =====================================================
// SWAGGER – bật cả trên production (để test trên Render)
// =====================================================
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Car Inventory API v1");
    c.RoutePrefix = string.Empty; // Swagger tại root URL "/"
});

// Tự động tạo tables khi khởi động (dùng EnsureCreated để tránh lỗi provider)
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated(); // Tạo DB + tables nếu chưa có
}

app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

// Lấy PORT từ biến môi trường của Render
var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
app.Run($"http://0.0.0.0:{port}");
