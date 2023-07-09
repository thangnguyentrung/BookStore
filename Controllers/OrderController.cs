using BookStore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;

namespace BookStore.Controllers
{
    public class OrderController : Controller
    {
        private readonly DataContext _dataContext;

        public OrderController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public IActionResult OrderInfo()
        {
            var cart = _dataContext.Carts.Include(o => o.Order).FirstOrDefault();
            if (cart != null)
            {
                var cartId = cart.CartId;
                if (cartId != null)
                {
                    HttpContext.Session.SetString("cartId", cartId.ToString());
                }
            } else
            {
                return NotFound();
            }

            var lsCart = _dataContext.Carts.Include(x => x.Book).ToList();
            ViewBag.Cart = lsCart;

            string totalPrice = HttpContext.Session.GetString("totalPrice");
            ViewBag.TotalPrice = totalPrice;
            string totalQuantity = HttpContext.Session.GetString("totalQuantity");
            ViewBag.TotalQuantity = totalQuantity;

            return View();
        }

        [HttpPost]
        public IActionResult OrderInfo(Order model)
        {
            string cartId = HttpContext.Session.GetString("cartId");
            if (int.TryParse(cartId, out int cId))
            {
                if (model != null)
                {
                    var order = new Order()
                    {
                        CartId = cId,
                        OrderId = model.OrderId,
                        OrderName = model.OrderName,
                        Email = model.Email,
                        PhoneNumbers = model.PhoneNumbers,
                        DeliveryAddress = model.DeliveryAddress,
                    };

                    _dataContext.Orders.Add(order);                   
                    _dataContext.SaveChanges();
                    return RedirectToAction("OrderConfirmation");
                }
            }
            return View(model);
        }

        public IActionResult OrderConfirmation()
        {
            return View();
        }
    }
}