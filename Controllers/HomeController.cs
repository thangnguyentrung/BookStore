using BookStore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace BookStore.Controllers
{
    public class HomeController : Controller
    {
        private DataContext _DataContext;

        public HomeController(DataContext dataContext) { 
            
            _DataContext = dataContext;
        }
        public IActionResult HomePage()
        {
            var list = _DataContext.Books.Include(x=>x.Category).Include(x=>x.Author).ToList();
            var carousel = _DataContext.News.Where(x => x.NewsTitle.StartsWith("C")).ToList();
      
            ViewBag.LsNewsCarousel = carousel;
            var outStanding = _DataContext.News.Where(x => x.NewsTitle.StartsWith("O")).ToList();
            ViewBag.LsNewsOutStanding = outStanding;
            var randomBooks = RandomBooks();
            ViewBag.RamdomBooks = randomBooks;
            var Username = HttpContext.Session.GetString("UserName");
            ViewBag.UserName = Username;
            return View(list);
        }
        public IEnumerable<Book> RandomBooks()
        {   
            // Khởi tạo đối tượng Random
            Random random = new Random();

            // Lấy danh sách tất cả sách
            var allBooks = _DataContext.Books.Include(x => x.Author).Include(x => x.Category).ToList();

            // Lấy 6 sách ngẫu nhiên từ danh sách sách
            var randomBooks = allBooks.OrderBy(p => random.Next()).Take(6).ToList();
            return randomBooks;
        }

    }
}