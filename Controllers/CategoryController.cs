using BookStore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Controllers
{
    public class CategoryController : Controller
    {
        private DataContext _DataContext;

        public CategoryController(DataContext dataContext)
        {

            _DataContext = dataContext;
        }
        public IActionResult ListCategory(string typeCate, string name)
        {
            var list = _DataContext.Books.Include(x => x.Author).Include(x => x.Category).ToList();           
            // category dashboard
            var cate = _DataContext.Categories.ToList();
            ViewBag.lsCate = cate;
            foreach (var itemCate in cate)
            {
                // lặp cate nếu cate.name nào được chọn  ==> trang với ds có cate đó
                if (typeCate == itemCate.CategoryName)
                {
                    list = _DataContext.Books.Include(x=>x.Author).Include(x => x.Category).Where(x=>x.Category.CategoryName == typeCate).ToList();                  
                }
            }
            if (!string.IsNullOrEmpty(name))
                list = list.Where(x=>x.BookName.ToLower().Contains(name.ToLower())).ToList();
            ViewBag.Search = name;
            _DataContext.SaveChanges();
            return View(list);
        }
        public IActionResult Details(int id) {
            var book = _DataContext.Books.Include(x => x.Author).Include(x => x.Category).SingleOrDefault(x => x.BookId == id);
            if (book == null)
            {
                return NotFound();
            }
            return View(book);
        }
        
    }
}
