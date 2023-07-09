using BookStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class OrderController : Controller
    {
        private DataContext _DataContext;
        public OrderController(DataContext dataContext)
        {
            _DataContext = dataContext;
        }
        public IActionResult ListOrder()
        {
            var lsOrder = _DataContext.Orders.Include(x => x.Cart).ToList();
            return View(lsOrder);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Order model)
        {
            if (model == null)
            {
                ViewBag.Err = "Order cần xác thực";
                return View(model);
            }
            else
            {
                _DataContext.Orders.Add(model);
                _DataContext.SaveChanges();
                return RedirectToAction("ListCart");
            }
        }
        public IActionResult Delete(int id)
        {
            var order = _DataContext.Orders.SingleOrDefault(x => x.OrderId == id);
            if (order == null)
            {
                return NotFound();
            }
            _DataContext.Orders.Remove(order);
            _DataContext.SaveChanges();
            return Redirect("~/Admin/Order/ListOrder");
        }
    }
}
