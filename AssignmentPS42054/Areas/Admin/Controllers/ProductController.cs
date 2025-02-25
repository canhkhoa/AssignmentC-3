using AssignmentPS42054.DAL;
using AssignmentPS42054.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AssignmentPS42054.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ProductController : Controller
    {
        private readonly ProductDAL _productDAL;

        public ProductController(ProductDAL productDAL)
        {
            _productDAL = productDAL;
        }

        public IActionResult Index()
        {
            var products = _productDAL.GetAllProducts();
            return View(products);
        }

        public IActionResult Create()
        {
            var product = new Product(); // Tạo một đối tượng Product mới
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Product product, IFormFile? FileImage, IFormFile? Image1, IFormFile? Image2, IFormFile? Image3)
        {
            if (ModelState.IsValid)
            {
                // Xử lý ảnh chính (FileImage)
                if (FileImage == null || FileImage.Length == 0)
                {
                    ViewBag.InfoImage = "Vui lòng chọn hình ảnh";
                    return View(product);
                }
                else
                {
                    string ImageName = DateTime.Now.ToString("yyyy_MM_dd_HHmmss_") + Path.GetFileNameWithoutExtension(FileImage.FileName) + Path.GetExtension(FileImage.FileName);
                    string SavePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/image/sanpham", ImageName);
                    using (var stream = new FileStream(SavePath, FileMode.Create))
                    {
                        FileImage.CopyTo(stream);
                    }
                    product.ProductImage = ImageName; // Gán tên ảnh cho thuộc tính ProductImage của Product
                }

                // Xử lý ảnh phụ (Image1)
                if (Image1 != null && Image1.Length > 0)
                {
                    string Image1Name = DateTime.Now.ToString("yyyy_MM_dd_HHmmss_") + Path.GetFileNameWithoutExtension(Image1.FileName) + Path.GetExtension(Image1.FileName);
                    string SavePath1 = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/image/sanpham", Image1Name);
                    using (var stream = new FileStream(SavePath1, FileMode.Create))
                    {
                        Image1.CopyTo(stream);
                    }
                    product.Image1 = Image1Name;
                }

                // Xử lý ảnh phụ (Image2)
                if (Image2 != null && Image2.Length > 0)
                {
                    string Image2Name = DateTime.Now.ToString("yyyy_MM_dd_HHmmss_") + Path.GetFileNameWithoutExtension(Image2.FileName) + Path.GetExtension(Image2.FileName);
                    string SavePath2 = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/image/sanpham", Image2Name);
                    using (var stream = new FileStream(SavePath2, FileMode.Create))
                    {
                        Image2.CopyTo(stream);
                    }
                    product.Image2 = Image2Name;
                }

                // Xử lý ảnh phụ (Image3)
                if (Image3 != null && Image3.Length > 0)
                {
                    string Image3Name = DateTime.Now.ToString("yyyy_MM_dd_HHmmss_") + Path.GetFileNameWithoutExtension(Image3.FileName) + Path.GetExtension(Image3.FileName);
                    string SavePath3 = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/image/sanpham", Image3Name);
                    using (var stream = new FileStream(SavePath3, FileMode.Create))
                    {
                        Image3.CopyTo(stream);
                    }
                    product.Image3 = Image3Name;
                }

                // Thêm Product mới vào cơ sở dữ liệu
                _productDAL.AddProduct(product);

                return RedirectToAction("Index");
            }
            return View(product);
        }

        public IActionResult Edit(int id)
        {
            var product = _productDAL.GetProductById(id);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Product product, IFormFile? FileImage, IFormFile? Image1, IFormFile? Image2, IFormFile? Image3)
        {
            if (ModelState.IsValid)
            {
                // Xử lý ảnh chính (FileImage)
                if (FileImage != null && FileImage.Length > 0)
                {
                    string ImageName = DateTime.Now.ToString("yyyy_MM_dd_HHmmss_") + Path.GetFileNameWithoutExtension(FileImage.FileName) + Path.GetExtension(FileImage.FileName);
                    string SavePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/image/sanpham", ImageName);
                    using (var stream = new FileStream(SavePath, FileMode.Create))
                    {
                        FileImage.CopyTo(stream);
                    }
                    product.ProductImage = ImageName; // Gán tên ảnh mới cho sản phẩm
                }

                // Xử lý ảnh phụ (Image1)
                if (Image1 != null && Image1.Length > 0)
                {
                    string Image1Name = DateTime.Now.ToString("yyyy_MM_dd_HHmmss_") + Path.GetFileNameWithoutExtension(Image1.FileName) + Path.GetExtension(Image1.FileName);
                    string SavePath1 = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/image/sanpham", Image1Name);
                    using (var stream = new FileStream(SavePath1, FileMode.Create))
                    {
                        Image1.CopyTo(stream);
                    }
                    product.Image1 = Image1Name;
                }

                // Xử lý ảnh phụ (Image2)
                if (Image2 != null && Image2.Length > 0)
                {
                    string Image2Name = DateTime.Now.ToString("yyyy_MM_dd_HHmmss_") + Path.GetFileNameWithoutExtension(Image2.FileName) + Path.GetExtension(Image2.FileName);
                    string SavePath2 = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/image/sanpham", Image2Name);
                    using (var stream = new FileStream(SavePath2, FileMode.Create))
                    {
                        Image2.CopyTo(stream);
                    }
                    product.Image2 = Image2Name;
                }

                // Xử lý ảnh phụ (Image3)
                if (Image3 != null && Image3.Length > 0)
                {
                    string Image3Name = DateTime.Now.ToString("yyyy_MM_dd_HHmmss_") + Path.GetFileNameWithoutExtension(Image3.FileName) + Path.GetExtension(Image3.FileName);
                    string SavePath3 = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/image/sanpham", Image3Name);
                    using (var stream = new FileStream(SavePath3, FileMode.Create))
                    {
                        Image3.CopyTo(stream);
                    }
                    product.Image3 = Image3Name;
                }

                _productDAL.UpdateProduct(product);

                return RedirectToAction("Index");
            }
            return View(product);
        }

        public IActionResult Delete(int id)
        {
            var product = _productDAL.GetProductById(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var product = _productDAL.GetProductById(id);
            if (product != null)
            {
                _productDAL.DeleteProduct(product);  // Xóa sản phẩm
            }
            return RedirectToAction("Index");
        }

        public IActionResult Details(int id)
        {
            var product = _productDAL.GetProductById(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }
    }
}
