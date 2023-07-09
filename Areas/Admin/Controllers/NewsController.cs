using BookStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class NewsController : Controller
    {
        private DataContext _DataContext;
        public NewsController(DataContext dataContext)
        {
            _DataContext = dataContext;
        }
        public IActionResult ListNews()
        {
            var lsNews = _DataContext.News.ToList();
            _DataContext.SaveChanges();
            return View(lsNews);
        }
        public IActionResult AddNews()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddNews(News model, IFormFile file)
        {
            if (model == null)
            {
                ViewBag.Err = "News cần xác thực";
                return View(model);
            }
            if (file != null && file.Length > 0)
            {
                string fileName = Path.GetFileName(file.FileName);
                string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "img");
                string filePath = Path.Combine(folderPath, fileName);

                // Kiểm tra loại file
                string[] allowedExtensions = { ".jpg", ".jpeg", ".png" }; // Thêm các loại file hợp lệ khác nếu cần
                string fileExtension = Path.GetExtension(fileName).ToLowerInvariant();
                if (!allowedExtensions.Contains(fileExtension))
                {
                    ModelState.AddModelError(string.Empty, "Chỉ chấp nhận file ảnh có định dạng JPG, JPEG, PNG.");
                    return View(model);
                }

                // Kiểm tra kích thước file (giới hạn là 5MB)
                long fileSizeLimit = 5 * 1024 * 1024; // 5MB (đơn vị tính là byte)
                if (file.Length > fileSizeLimit)
                {
                    ModelState.AddModelError(string.Empty, "File ảnh vượt quá kích thước cho phép (5MB).");
                    return View(model);
                }

                // Tạo thư mục lưu trữ nếu chưa tồn tại
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                // Tạo FileStream và sao chép nội dung file
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Vui lòng chọn một file ảnh.");
                return View(model);
            }
            model.NewsImageUrl = file.FileName;
            _DataContext.News.Add(model);
            _DataContext.SaveChanges();
            return Redirect("~/Admin/News/ListNews");
        }
        public IActionResult Delete(int id)
        {
            var news = _DataContext.News.SingleOrDefault(n => n.NewsId == id);
            _DataContext.News.Remove(news);
            _DataContext.SaveChanges();
            return Redirect("~/Admin/News/ListNews");
        }
        public IActionResult Edit(int id)
        {
            var news = _DataContext.News.SingleOrDefault(x=>x.NewsId == id);
            return View(news);
        }
        [HttpPost]
        public IActionResult Edit(News model ,int id)
        {
            var news = _DataContext.News.SingleOrDefault(x => x.NewsId == id);
            if(model == null)
            {
                ViewBag.Err = "News cần xác thực";
            }
            news.NewsTitle = model.NewsTitle;
            news.NewsContent = model.NewsContent;
            news.NewsImageUrl = model.NewsImageUrl;
            _DataContext.SaveChanges();
            return Redirect("~/Admin/News/ListNews");
        }

    }
}
