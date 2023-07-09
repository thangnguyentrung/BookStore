using BookStore.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Security.Claims;

namespace BookStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class UserController : Controller
    {
        private DataContext _DataContext;
        public UserController(DataContext dataContext)
        {
            _DataContext = dataContext;
        }
        public IActionResult ListUser()
        {
            var lsUser = _DataContext.Users.Include(x => x.Order).ToList();
            return View(lsUser);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(User user)
        {
            if(user == null)
            {
                ViewBag.Err = "User cần xác thực";
                return View(user);
            }
            var lsUser = _DataContext.Users.ToList();
            foreach (var  itemUser in lsUser)
            {
                if(itemUser.UserName == user.UserName)
                {
                    ViewBag.NameSake = "Username k được trùng nhau";
                }
            }
            _DataContext.Users.Add(user);
            _DataContext.SaveChanges();
            return Redirect("~/Admin/User/ListUser");
        }
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string username, string password)
        {
            var user = _DataContext.Users.FirstOrDefault(x => x.UserName.Equals(username) && x.Password.Equals(password));
            if (user != null)
            {
                // Danh sách claim cung cấp thông tin về người dùng và quyền hạn của họ,
                // giúp ứng dụng xác định và kiểm soát quyền truy cập cho từng người dùng.
                var claims = new List<Claim>
            {
                 new Claim(ClaimTypes.Name, user.UserName),
                 new Claim(ClaimTypes.Role, "User")
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
                HttpContext.Session.SetString("UserName", user.FullName);              
                return Redirect("~/Home/HomePage");
            }
            else
            {
                ViewBag.Err = "User cần xác thực ";
                return View(user);
            }
        }
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return Redirect("~/Home/HomePage");
        }
        [AllowAnonymous]
        public IActionResult RegisterUser()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public IActionResult RegisterUser(User user)
        {
            if(user == null)
            {
                ViewBag.Err = "User cần xác thực";
                return View(user);
            }
            var lsUser = _DataContext.Users.ToList();
            foreach (var itemUser in lsUser)
            {
                if (itemUser.UserName == user.UserName)
                {
                    ViewBag.NameSake = "Username k được trùng nhau";
                }
            }
            _DataContext.Users.Add(user);
            _DataContext.SaveChanges();
            return Redirect("~/Home/HomePage");
        }
        [Authorize(Roles= "Admin,User")]
        public IActionResult ChangePassword()
        {
            return View();
        }
        [HttpPost]
        [Authorize(Roles = "Admin,User")]
        public IActionResult ChangePassword(int id,string username, string password, string newPass, string againNewPass  )
        {
            var userId = _DataContext.Users.Find(id);
            if(againNewPass.Equals(newPass))
            {
                userId.Password = againNewPass;
                ViewBag.NewPass = "Thay đổi mật khẩu thành công";
            }           
            return View(userId);
        }
        public IActionResult Delete(int id)
        {
            var user = _DataContext.Users.SingleOrDefault(x => x.UsertId == id);
            if (user == null)
            {
                return NotFound();
            }
            _DataContext.Users.Remove(user);
            _DataContext.SaveChanges();
            return Redirect("~/Admin/User/ListUser");
        }


    }
}
