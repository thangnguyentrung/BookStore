using Microsoft.AspNetCore.Mvc;

namespace BookStore.Controllers
{
    public class ContactController : Controller
    {
        public IActionResult ContactPage()
        {          
            return View();
        }
    }
}
