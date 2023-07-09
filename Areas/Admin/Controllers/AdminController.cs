using BookStore.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using System.Security.Claims;

namespace BookStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class AdminController : Controller
    {
        private DataContext _DataContext;
        public AdminController(DataContext dataContext)
        {
            _DataContext = dataContext;
        }      
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            var admin = _DataContext.Admins.FirstOrDefault(x => x.UserName.Equals(username) && x.Password.Equals(password));       
            if (admin != null)
            {
                // Danh sách claim cung cấp thông tin về người dùng và quyền hạn của họ,
                // giúp ứng dụng xác định và kiểm soát quyền truy cập cho từng người dùng.
             var claims = new List<Claim>
            {
                 new Claim(ClaimTypes.Name, admin.UserName),
                 new Claim(ClaimTypes.Role, "Admin")
            // Thêm các thông tin xác thực khác vào danh sách claim nếu cần
            };

                var claimsIdentity = new ClaimsIdentity(
                    claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var authProperties = new AuthenticationProperties
                {
                    // Cấu hình thuộc tính xác thực nếu cần
                };

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                    authProperties);
                TempData["AdminName"] = admin.FullName;
                return Redirect("~/Admin/Home/AdminPage");
            }
            else
            {
                ViewBag.Err = "lỗi không đăng nhập được ";
                return View(admin);
            }          
        }
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return Redirect("~/Home/HomePage");
        }
    }
}
