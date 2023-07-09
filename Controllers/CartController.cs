using BookStore.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Configuration;

namespace BookStore.Controllers
{
    public class CartController : Controller
    {
        private readonly DataContext _dataContext;

        public CartController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public IActionResult ListCart()
        {
            var listCart = _dataContext.Carts.Include(o => o.Book).ToList();
            // Tính tổng tiền 
            decimal totalPrice = GetTotalPrice();
            ViewBag.TotalPrice = totalPrice;
            int totalQuantity = GetTotalQuantity();
            ViewBag.TotalQuantity = totalQuantity;
            // gửi kết quả sang order      
            HttpContext.Session.SetString("totalPrice", totalPrice.ToString());
            HttpContext.Session.SetString("totalQuantity",totalQuantity.ToString());
            return View(listCart);
        }
        public IActionResult AddToCart(int id)
        {
            // Sản phẩm mình muốn thêm vào giỏ
            var book = _dataContext.Books.Include(b => b.Author).Include(b => b.Category).SingleOrDefault(b => b.BookId == id);

            // Kiểm tra giỏ hàng có hàng chưa 
            var checkCart = _dataContext.Carts.FirstOrDefault(c => c.BookId == id);

            if (checkCart == null)
            {
                // Kiểm tra người dùng có trùng trong giỏ hàng không
                var user = _dataContext.Users.SingleOrDefault(u => u.UsertId == 1);

                Cart cart = new Cart()
                {
                    UserId = user.UsertId,
                    BookId = book.BookId,
                    BookName = book.BookName,
                    Image = book.Image,
                    Quantity = 1,
                    TotalPrice = book.Price,
                };
                // tạo được cart mới thì số lượng trong data phải giảm đi 1              
                book.Quantity--;                
                _dataContext.Carts.Add(cart);               

            }
            else
            {
                // nếu có sẵn hàng thì số lượng + tiền tăng
                book.Quantity--;
                checkCart.Quantity++;
                checkCart.TotalPrice = checkCart.Quantity * book.Price;
            }
            
            _dataContext.SaveChanges();
            return RedirectToAction("ListCart");
        }

        public decimal GetTotalPrice()
        {
            decimal totalPrice = 0;
            var cart = _dataContext.Carts.ToList();
            foreach (var item in cart)
            {
                totalPrice += item.TotalPrice;
            }
            return totalPrice;
        }
        public int GetTotalQuantity()
        {
            int totalQuantity = 0;
            var cart = _dataContext.Carts.ToList();
            foreach (var item in cart)
            {
                totalQuantity += item.Quantity;
            }
            return totalQuantity;
        }
        public IActionResult Delete(int id)
        {
            var deleteBook = _dataContext.Carts.SingleOrDefault(x=>x.Book.BookId == id);
            var cart = _dataContext.Carts.FirstOrDefault(c => c.BookId == id);
            var book = _dataContext.Books.Include(b => b.Author).Include(b => b.Category).SingleOrDefault(b => b.BookId == id);

            // data phải tăng theo số lượng trong giỏ hàng
            book.Quantity = book.Quantity  + cart.Quantity;
            _dataContext.Remove(deleteBook);
            _dataContext.SaveChanges();
            return RedirectToAction("ListCart");
        }             

    }
}