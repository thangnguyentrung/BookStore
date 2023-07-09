using BookStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace BookStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class BookController : Controller
    {
        private DataContext _DataContext;
        public BookController(DataContext dataContext)
        {
            _DataContext = dataContext;
        }
        public IActionResult ListBook(string? search, string? sortBy, string sortType = "ASC")
        {
            var list = _DataContext.Books.Include(a => a.Author).Include(c=>c.Category).ToList();
            // tìm kiếm
            if (!string.IsNullOrEmpty(search))
                list = list.Where(b => b.BookName.ToLower().Contains(search.ToLower())).ToList();
            // sắp xếp
            if (sortBy == "BookName")
            {
                if (sortType == "ASC")
                {
                    list = list.OrderBy(b => b.BookName).ToList();
                }
                else
                {
                    list = list.OrderByDescending(b => b.BookName).ToList();
                }
            }
            else
            {
                // Mặc định sắp xếp theo tên sách theo thứ tự tăng dần
                list = list.OrderBy(b => b.BookName).ToList();
            }
            // phân trang

            ViewBag.SortBy = sortBy;
            ViewBag.SortType = sortType;
            return View(list);
        }
        public IActionResult Create()
        {
            var author = _DataContext.Authors.ToList();
            ViewBag.Author = author;
            var cate = _DataContext.Categories.ToList();
            ViewBag.Cate = cate;
            return View();
        }
        [HttpPost]
        public IActionResult Create(Book model, IFormFile Image)
        {
            if (model == null)
            {
                ViewBag.Err = "Book cần xác thực";
                return View();
            }
            if (Image != null && Image.Length > 0)
            {
                string fileName = Path.GetFileName(Image.FileName);
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
                if (Image.Length > fileSizeLimit)
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
                    Image.CopyTo(stream);
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Vui lòng chọn một file ảnh.");
                return View(model);
            }
            model.Image = Image.FileName;
            _DataContext.Books.Add(model);
            _DataContext.SaveChanges();
            return Redirect("~/Admin/Book/ListBook");


        }
        public IActionResult Edit(int id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var book = _DataContext.Books.Include(a => a.Author).SingleOrDefault(b => b.BookId == id);
            if (book == null)
            {
                return NotFound();
            }
            var author = _DataContext.Authors.ToList();
            ViewBag.Author = author;
            return View(book);
        }
        [HttpPost]
        public IActionResult Edit(Book model, int id, IFormFile Image)
        {
            var book = _DataContext.Books.Include(a => a.Author).SingleOrDefault(b => b.BookId == id);
            if (model == null)
            {
                ViewBag.Err = "Book cần xác thực";
                return View();
            }
            if (book == null)
            {
                return NotFound(model);
            }
            if (Image != null && Image.Length > 0)
            {
                string fileName = Path.GetFileName(Image.FileName);
                string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "img", "folderName");
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
                if (Image.Length > fileSizeLimit)
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
                    Image.CopyTo(stream);
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Vui lòng chọn một file ảnh.");
                return View(model);
            }
            book.BookName = model.BookName;
            book.Description = model.Description;
            book.DateTime = model.DateTime;
            book.Price = model.Price;
            book.Quantity = model.Quantity;
            book.Image = Image.FileName;
            book.CategoryId = model.CategoryId;
            book.AuthorId = model.AuthorId;
            _DataContext.SaveChanges();
            return Redirect("~/Admin/Book/ListBook");

        }

        public IActionResult Delete(int id)
        {
            var book = _DataContext.Books.Include(a => a.Author).SingleOrDefault(b => b.BookId == id);
            _DataContext.Books.Remove(book);
            _DataContext.SaveChanges();
            return Redirect("~/Admin/Book/ListBook");
        }
        public IActionResult Detail(int id)
        {
            var book = _DataContext.Books.Include(a => a.Author).SingleOrDefault(b => b.BookId == id);
            if (book == null)
            {
                return NotFound();
            }
            return View(book);
        }
        
    }
}
