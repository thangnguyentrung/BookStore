using BookStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class CategoryController : Controller
    {
        private DataContext _DataContext;
        public CategoryController(DataContext dataContext)
        {
            _DataContext = dataContext;
        }
        public IActionResult ListCategory(string? search, string? sortBy, string sortType = "ASC")
        {
            var list = _DataContext.Categories.ToList();
            // tìm kiếm
            if (!string.IsNullOrEmpty(search))
                list = list.Where(b => b.CategoryName.ToLower().Contains(search.ToLower())).ToList();
            // sắp xếp
            if (sortBy == "CategoryName")
            {
                if (sortType == "ASC")
                {
                    list = list.OrderBy(b => b.CategoryName).ToList();
                }
                else
                {
                    list = list.OrderByDescending(b => b.CategoryName).ToList();
                }
            }
            else
            {
                // Mặc định sắp xếp theo tên sách theo thứ tự tăng dần
                list = list.OrderBy(b => b.CategoryName).ToList();
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
        public IActionResult Create(Category model)
        {
            if (model == null)
            {
                ViewBag.Err = "Cate cần xác thực";
                return View(model);
            }
            _DataContext.Categories.Add(model);
            _DataContext.SaveChanges();
            return Redirect("~/Admin/Category/ListCategory");
        }
        public IActionResult Edit(int id)
        {
            var cate = _DataContext.Categories.SingleOrDefault(x => x.CategoryId == id);
            return View(cate);
        }
        [HttpPost]
        public IActionResult Edit(int id, Category model)
        {
            var cate = _DataContext.Categories.SingleOrDefault(x => x.CategoryId == id);
            if (model == null)
            {
                ViewBag.Err = "Cate cần xác thực";
                return View(model);
            }
            cate.CategoryName = model.CategoryName;         
            _DataContext.SaveChanges();
            return Redirect("~/Admin/Category/ListCategory");
        }
        public IActionResult Delete(int id)
        {
            var cate = _DataContext.Categories.SingleOrDefault(x => x.CategoryId == id);
            if (cate == null)
            {
                return NotFound();
            }
            _DataContext.Categories.Remove(cate);
            _DataContext.SaveChanges();
            return Redirect("~/Admin/Category/ListCategory");
        }
    }
}
