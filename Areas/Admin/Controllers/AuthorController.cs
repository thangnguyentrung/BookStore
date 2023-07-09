using Microsoft.AspNetCore.Mvc;
using BookStore.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace BookStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class AuthorController : Controller
    {
        private DataContext _DataContext;
        public AuthorController(DataContext dataContext)
        {
            _DataContext = dataContext;
        }
        public IActionResult ListAuthor(string? search, string? sortBy, string sortType = "ASC")
        {
            var list = _DataContext.Authors.ToList();
            // tìm kiếm
            if (!string.IsNullOrEmpty(search))
                list = list.Where(b => b.AuthorName.ToLower().Contains(search.ToLower())).ToList();
            // sắp xếp
            if (sortBy == "AuthorName")
            {
                if (sortType == "ASC")
                {
                    list = list.OrderBy(b => b.AuthorName).ToList();
                }
                else
                {
                    list = list.OrderByDescending(b => b.AuthorName).ToList();
                }
            }
            else
            {
                // Mặc định sắp xếp theo tên sách theo thứ tự tăng dần
                list = list.OrderBy(b => b.AuthorName).ToList();
            }
            // phân trang

            ViewBag.SortBy = sortBy;
            ViewBag.SortType = sortType;
            return View(list);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Author model)
        {           
            if(model == null)
            {
                ViewBag.Err = "Author cần xác thực";
                return View(model);
            } else
            {
                _DataContext.Authors.Add(model);
                _DataContext.SaveChanges();
                return Redirect("~/Admin/Author/ListAuthor");
            }
           
        }
        public IActionResult Edit(int id)
        {
            var author = _DataContext.Authors.SingleOrDefault(x=>x.AuthorId == id);
            return View(author);
        }
        [HttpPost]
        public IActionResult Edit(int id,Author model)
        {
            var author = _DataContext.Authors.SingleOrDefault(x => x.AuthorId == id);
            if (model == null)
            {
                ViewBag.Err = "Author cần xác thực";
                return View(model);
            }
            author.AuthorName = model.AuthorName;
            author.BirthDate = model.BirthDate;
            author.Nationality =  model.Nationality;
            author.Story = model.Story;
            _DataContext.SaveChanges();
            return Redirect("~/Admin/Author/ListAuthor");
        }
        public IActionResult Delete(int id)
        {
            var author = _DataContext.Authors.SingleOrDefault(x => x.AuthorId == id);
            if (author == null)
            {
                return NotFound();
            }
            _DataContext.Authors.Remove(author);
            _DataContext.SaveChanges();
            return Redirect("~/Admin/Author/ListAuthor");
        }
    }
}
