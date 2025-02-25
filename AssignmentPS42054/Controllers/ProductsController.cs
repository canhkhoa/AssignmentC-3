using AssignmentPS42054.DAL;
using AssignmentPS42054.Models;
using Microsoft.AspNetCore.Mvc;

namespace AssignmentPS42054.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ProductDAL _productDAL;
        private readonly CategoryDAL _categoryDAL;
        public ProductsController(ProductDAL productDAL, CategoryDAL categoryDAL)
        {
            _productDAL = productDAL;
            _categoryDAL = categoryDAL;
        }
        public IActionResult Index()
        {
            // Lấy tất cả sản phẩm và danh mục
            var products = _productDAL.GetAllProducts();
            var categories = _categoryDAL.GetAllCats();

            // Tạo ViewModel để chứa dữ liệu
            var viewModel = new ProCatView
            {
                Products = products,
                Categories = categories
            };

            // Truyền ViewModel vào view
            return View(viewModel);
        }
        public IActionResult Category(int id)
        {
            // Lấy các sản phẩm theo danh mục
            var products = _productDAL.GetProductsByCategoryId(id);
            var categories = _categoryDAL.GetAllCats();

            // Tạo ViewModel để chứa dữ liệu
            var viewModel = new ProCatView
            {
                Products = products,
                Categories = categories
            };

            // Truyền ViewModel vào view
            return View("Index", viewModel); // Sử dụng lại view Index để hiển thị
        }

        public IActionResult Detail(int id)
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
