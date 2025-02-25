using AssignmentPS42054.DAL;
using AssignmentPS42054.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Authorization;

namespace AssignmentPS42054.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class CategoryController : Controller
    {
        private readonly CategoryDAL _categoryDAL;

        
        public CategoryController(CategoryDAL categoryDAL)
        {
            _categoryDAL = categoryDAL;
        }

        
        public IActionResult Index()
        {
            var categories = _categoryDAL.GetAllCats();
            return View(categories);
        }

        
        public IActionResult Create()
        {
            var category = new Category();
            return View(category);
        }

      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category category, IFormFile? FileImage)
        {
            if (ModelState.IsValid)
            {
                if (FileImage == null || FileImage.Length == 0)
                {
                    ViewBag.InfoImage = "Vui lòng chọn hình ảnh";
                    return View(category);
                }

                // Xử lý file ảnh
                string ImageName = DateTime.Now.ToString("yyyy_MM_dd_HHmmss_") + Path.GetFileNameWithoutExtension(FileImage.FileName) + Path.GetExtension(FileImage.FileName);
                string SavePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/image/sanpham", ImageName);

                // Tạo thư mục nếu chưa có
                if (!Directory.Exists(Path.GetDirectoryName(SavePath)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(SavePath));
                }

                // Lưu ảnh vào thư mục
                using (var stream = new FileStream(SavePath, FileMode.Create))
                {
                    FileImage.CopyTo(stream);
                }

                category.Cat_Img = ImageName;
                _categoryDAL.AddCategory(category); 

                return RedirectToAction("Index"); 
            }
            return View(category);
        }

       
        public IActionResult Edit(int id)
        {
            var category = _categoryDAL.GetCatById(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category category, IFormFile? FileImage)
        {
            if (ModelState.IsValid)
            {
                if (FileImage != null && FileImage.Length > 0)
                {
                    // Xử lý file ảnh mới
                    string ImageName = DateTime.Now.ToString("yyyy_MM_dd_HHmmss_") + Path.GetFileNameWithoutExtension(FileImage.FileName) + Path.GetExtension(FileImage.FileName);
                    string SavePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/image/sanpham", ImageName);

                    // Tạo thư mục nếu chưa có
                    if (!Directory.Exists(Path.GetDirectoryName(SavePath)))
                    {
                        Directory.CreateDirectory(Path.GetDirectoryName(SavePath));
                    }

                    // Lưu ảnh mới vào thư mục
                    using (var stream = new FileStream(SavePath, FileMode.Create))
                    {
                        FileImage.CopyTo(stream);
                    }

                    category.Cat_Img = ImageName;
                }

                _categoryDAL.UpdateCategory(category); 
                return RedirectToAction("Index"); 
            }

            return View(category);
        }

        // Action: Hiển thị form xóa Category
        public IActionResult Delete(int id)
        {
            var category = _categoryDAL.GetCatById(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // POST: Xác nhận xóa Category
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var category = _categoryDAL.GetCatById(id);
            if (category == null)
            {
                return NotFound();
            }

            _categoryDAL.DeleteCategory(id);
            return RedirectToAction("Index");
        }
    }
}
