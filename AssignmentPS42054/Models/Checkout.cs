using System.ComponentModel.DataAnnotations;

namespace AssignmentPS42054.Models
{
    public class Checkout
    {
        public List<CartItem> CartItems { get; set; }
        [Required(ErrorMessage = "Tên khách hàng là bắt buộc.")]
        public string CustomerName { get; set; }
        [Required(ErrorMessage = "Địa chỉ là bắt buộc.")]
        public string Address { get; set; }
        [Required(ErrorMessage = "Số điện thoại là bắt buộc.")]
        [Phone(ErrorMessage = "Số điện thoại không hợp lệ.")]
        public string PhoneNumber { get; set; }
        public string PaymentMethod { get; set; }
    }

}