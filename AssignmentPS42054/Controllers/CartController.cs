using AssignmentPS42054.DAL;
using AssignmentPS42054.Helper;
using AssignmentPS42054.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AssignmentPS42054.Controllers
{
    [Authorize(Roles = "User")]
    public class CartController : Controller
    {
        private readonly CartService _cartService;
        private readonly ProductDAL _productDAL;

        public CartController(CartService cartService, ProductDAL productDAL)
        {
            _cartService = cartService;
            _productDAL = productDAL;
        }

        public IActionResult Index()
        {
            var cartItems = _cartService.GetCartItems();
            return View(cartItems);
        }

        [HttpPost]
        public IActionResult AddToCart(int productId)
        {
            // Lấy thông tin sản phẩm dựa vào productId
            var product = _productDAL.GetProductById(productId);

            if (product == null)
            {
                return NotFound();
            }

            // Lấy danh sách sản phẩm trong giỏ hàng hiện tại
            var cartItems = _cartService.GetCartItems();
            var existingItem = cartItems.FirstOrDefault(i => i.ProductId == productId);

            if (existingItem != null)
            {
                // Nếu sản phẩm đã có trong giỏ hàng, tăng số lượng lên 1
                existingItem.Quantity++;
            }
            else
            {
                // Nếu sản phẩm chưa có trong giỏ hàng, thêm mới với số lượng 1
                var cartItem = new CartItem
                {
                    DetailImage = product.ProductImage,
                    ProductId = product.Id,
                    ProductName = product.Name,
                    Price = (decimal)product.Price,
                    Quantity = 1
                };

                // Thêm sản phẩm mới vào giỏ hàng
                cartItems.Add(cartItem);
            }

            // Lưu lại giỏ hàng sau khi cập nhật
            _cartService.UpdateCart(cartItems);

            // Chuyển hướng người dùng về trang giỏ hàng
            return RedirectToAction("Index");
        }




        [HttpPost]
        public IActionResult RemoveFromCart(int productId)
        {
            _cartService.RemoveFromCart(productId);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult UpdateToCart(int productId, int quantity)
        {
            var cartItems = _cartService.GetCartItems();
            var existingItem = cartItems.FirstOrDefault(i => i.ProductId == productId);
            if (existingItem != null)
            {
                existingItem.Quantity = quantity;
                _cartService.UpdateCart(cartItems);
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        public IActionResult Checkout()
        {
            var cartItems = _cartService.GetCartItems();

            if (!cartItems.Any())
            {
                return RedirectToAction("Index");
            }

            var checkoutViewModel = new Checkout
            {
                CartItems = cartItems ?? new List<CartItem>(),  // Khởi tạo danh sách rỗng nếu là null
                CustomerName = "",
                Address = "",
                PhoneNumber = "",
                PaymentMethod = "COD"
            };

            return View(checkoutViewModel);
        }




        [HttpPost]
        public IActionResult SubmitCheckout(Checkout model)
        {
            _cartService.ClearCart();
            return RedirectToAction("OrderConfirmation");
        }
        public IActionResult OrderConfirmation()
        {
            return View();
        }



    }
}
