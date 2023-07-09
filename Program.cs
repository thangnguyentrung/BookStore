using BookStore.Models;
using Microsoft.CodeAnalysis.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
var configuration = new ConfigurationBuilder()
    .SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json")
    .Build();
builder.Services.AddDbContext<DataContext>(options =>
{
    object value = options.UseSqlServer(configuration.GetConnectionString("DataContextConnection"));
});
// Add services Cookies login
builder.Services.AddAuthentication("Cookies")
    .AddCookie("Cookies", options =>
    {
        // Cấu hình các thuộc tính của cookie
        options.Cookie.Name = "YourCookieName"; // Tên của cookie
        options.Cookie.HttpOnly = true; // Chỉ truy cập cookie qua HTTP
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30); // Thời gian tồn tại của cookie
        options.LoginPath = "/Admin/Admin/Login"; // Đường dẫn đến trang đăng nhập
        options.AccessDeniedPath = "/Admin/Admin/Login"; // Đường dẫn đến trang bị từ chối truy cập
        options.SlidingExpiration = true; // Có cho phép cơ chế đăng nhập tự động kết lại không
    });
// Add services Session
builder.Services.AddSession();
builder.Services.AddControllersWithViews();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.UseStaticFiles();
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=HomePage}/{id?}");

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
      name: "areas",
      pattern: "{area:exists}/{controller=Admin}/{action=Login}/{id?}"
    );
});
app.Run();
