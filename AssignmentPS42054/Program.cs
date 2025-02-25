using AssignmentPS42054.DAL;
using AssignmentPS42054.Helper;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Cấu hình các dịch vụ (ConfigureServices)
builder.Services.AddScoped<CategoryDAL>();
builder.Services.AddScoped<ProductDAL>();
builder.Services.AddScoped<AccountDAL>();
builder.Services.AddScoped<CartService>();

builder.Services.AddHttpContextAccessor(); // Để có thể truy cập HttpContext trong ứng dụng

builder.Services.AddSession(options => {
    options.IdleTimeout = TimeSpan.FromMinutes(10); // Thời gian session hết hạn
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Cấu hình xác thực bằng Cookie
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login"; // Đường dẫn đến trang login nếu chưa đăng nhập
        options.AccessDeniedPath = "/Account/AccessDenied"; // Trang từ chối truy cập nếu không đủ quyền
    });

// Cấu hình quyền truy cập (Authorization)
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin")); // Chính sách yêu cầu role Admin
}); ;

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Cấu hình các middleware (Configure)
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Sử dụng session trước khi dùng authentication và authorization
app.UseSession();

app.UseAuthentication(); // Xác thực người dùng
app.UseAuthorization();

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=AdminHome}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
