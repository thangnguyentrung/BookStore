using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class HomeController : Controller
    {
        public IActionResult AdminPage()
        {
            var adminName = TempData["AdminName"] as string; // Lấy tên admin từ TempData

            ViewBag.AdminName = adminName;
            return View();
        }
    }
}
