using AssignmentPS42054.Models;
using Newtonsoft.Json;

namespace AssignmentPS42054.Helper
{
    public class CartService
    {
        private const string CartSessionKey = "CartItems"; // Khóa session dùng để lưu giỏ hàng
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CartService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        // Lấy danh sách các mặt hàng trong giỏ hàng
        public List<CartItem> GetCartItems()
        {
            var session = _httpContextAccessor.HttpContext.Session;
            string cartJson = session.GetString(CartSessionKey);
            if (string.IsNullOrEmpty(cartJson))
            {
                return new List<CartItem>(); // Trả về danh sách rỗng nếu không có giỏ hàng
            }
            return JsonConvert.DeserializeObject<List<CartItem>>(cartJson);
        }

        // Thêm sản phẩm vào giỏ hàng
        public void AddToCart(CartItem item)
        {
            var cartItems = GetCartItems(); // Lấy giỏ hàng hiện tại
            var existingItem = cartItems.FirstOrDefault(i => i.ProductId == item.ProductId); // Kiểm tra sản phẩm đã tồn tại chưa

            if (existingItem != null)
            {
                // Nếu sản phẩm đã tồn tại, tăng số lượng
                existingItem.Quantity += item.Quantity;
            }
            else
            {
                // Nếu sản phẩm chưa có trong giỏ hàng, thêm mới
                cartItems.Add(item);
            }

            SaveCartItems(cartItems); // Lưu lại giỏ hàng
        }

        // Xóa sản phẩm khỏi giỏ hàng
        public void RemoveFromCart(int productId)
        {
            var cartItems = GetCartItems(); // Lấy giỏ hàng hiện tại
            var itemToRemove = cartItems.FirstOrDefault(i => i.ProductId == productId); // Tìm sản phẩm cần xóa

            if (itemToRemove != null)
            {
                // Nếu tìm thấy, xóa sản phẩm khỏi giỏ hàng
                cartItems.Remove(itemToRemove);
                SaveCartItems(cartItems); // Lưu lại giỏ hàng
            }
        }

        // Cập nhật giỏ hàng
        public void UpdateCart(List<CartItem> cartItems)
        {
            SaveCartItems(cartItems); // Cập nhật giỏ hàng trong session
        }

        // Lưu giỏ hàng vào session
        private void SaveCartItems(List<CartItem> cartItems)
        {
            var session = _httpContextAccessor.HttpContext.Session;
            string cartJson = JsonConvert.SerializeObject(cartItems);
            session.SetString(CartSessionKey, cartJson); // Lưu giỏ hàng dưới dạng JSON
        }
        public void ClearCart()
        {
            var cartItems = new List<CartItem>(); // Tạo một danh sách rỗng
            SaveCartItems(cartItems); // Lưu giỏ hàng dưới dạng danh sách rỗng
        }

    }
}
