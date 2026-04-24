using phamthicamtien.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


var databaseUrl = Environment.GetEnvironmentVariable("DATABASE_URL");

if (!string.IsNullOrEmpty(databaseUrl))
{
    // Render cung cấp DATABASE_URL dạng: postgres://user:pass@host:port/dbname
    // Chuyển sang dạng Npgsql connection string
    var uri = new Uri(databaseUrl);
    var userInfo = uri.UserInfo.Split(':', 2);

    var host = uri.Host;
    var dbPort = uri.Port == -1 ? 5432 : uri.Port;
    var database = uri.AbsolutePath.TrimStart('/');
    var username = Uri.UnescapeDataString(userInfo[0]);
    // Thêm check độ dài để tránh lỗi IndexOutOfRange nếu mật khẩu bị khuyết
    var password = userInfo.Length > 1 ? Uri.UnescapeDataString(userInfo[1]) : "";

    var npgsqlConnectionString =
        $"Host={host};Port={dbPort};Database={database};Username={username};Password={password};SSL Mode=Require;Trust Server Certificate=true";

    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseNpgsql(npgsqlConnectionString));
}
else
{
    // Local development: dùng Npgsql PostgreSQL
    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
}

// =====================================================
// 2. CORS – Cho phép frontend gọi API
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

// Cấu hình Controller và Json để tránh lỗi lặp vòng
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler =
            System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.DefaultIgnoreCondition =
            System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// =====================================================
// 3. SWAGGER – Bật trên mọi môi trường (kể cả Render)
// =====================================================
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Car Inventory API v1");
    // Trên Render: Swagger ở trang chủ. Local: ở /swagger (khớp launchSettings)
    if (!app.Environment.IsDevelopment())
    {
        c.RoutePrefix = string.Empty;
    }
});

// =====================================================
// 4. KHỞI TẠO DATABASE
// =====================================================
// Tự động apply migrations khi khởi động
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    // Áp dụng migration để tạo Db thay vì EnsureCreated
    db.Database.Migrate(); 

    // Bổ sung seed data
    DbSeeder.Seed(db);
}

// =====================================================
// 5. MIDDLEWARE PIPELINE
// =====================================================
app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

// =====================================================
// 6. RENDER PORT BINDING
// =====================================================
var renderPort = Environment.GetEnvironmentVariable("PORT");
if (!string.IsNullOrEmpty(renderPort))
{
    // Trên Render: bind vào port mà Render cung cấp
    app.Run($"http://0.0.0.0:{renderPort}");
}
else
{
    // Local development: dùng port từ launchSettings.json (5065)
    app.Run();
}