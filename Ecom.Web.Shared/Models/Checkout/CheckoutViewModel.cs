using Ecom.Web.Shared.Models.Payment;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Ecom.Web.Shared.Models.Checkout
{
    public class CheckoutViewModel
    {
        public CheckoutViewModel() { }
        // Dữ liệu giỏ hàng lấy từ Product Service (thông qua gRPC)
        public List<CheckoutItemViewModel> Items { get; set; } = new();
        public decimal SubTotal { get; set; }
        public decimal ShippingFee { get; set; }
        public decimal TotalAmount { get; set; }

        // Thông tin giao hàng (Khách nhập)
        [Required(ErrorMessage = "Vui lòng nhập họ tên")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập số điện thoại")]
        [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập địa chỉ giao hàng")]
        public string ShippingAddress { get; set; }

        // Chỉ comment dòng quan trọng: Thuộc tính kiểm tra xem toàn bộ giỏ hàng có hợp lệ để thanh toán không
        public bool IsAllItemsAvailable => Items.All(x => x.IsAvailable);

        // Thêm danh sách để khách chọn trên giao diện
        public List<PaymentMethodViewModel> PaymentMethods { get; set; } = new();

        [Required(ErrorMessage = "Vui lòng chọn phương thức thanh toán")]
        public string SelectedPaymentMethodCode { get; set; }
    }
}
