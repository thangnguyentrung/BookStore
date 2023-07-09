using BookStore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CartController : Controller
    {
        private DataContext _DataContext;
        public CartController(DataContext dataContext)
        {
            _DataContext = dataContext;
        }
        public IActionResult ListCart()
        {
            var lsCart = _DataContext.Carts.Include(x => x.User).Include(x => x.Book).ToList();
            return View(lsCart);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Cart model, IFormFile file)
        {
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
            model.Image = file.FileName;
            _DataContext.Carts.Add(model);
            _DataContext.SaveChanges();
            return RedirectToAction("ListCart");                                           
        }
        public IActionResult Delete(int id)
        {
            var cart = _DataContext.Carts.SingleOrDefault(x => x.CartId == id);
            if (cart == null)
            {
                return NotFound();
            }
            _DataContext.Carts.Remove(cart);
            _DataContext.SaveChanges();
            return Redirect("~/Admin/Cart/ListCart");
        }
    }
}
