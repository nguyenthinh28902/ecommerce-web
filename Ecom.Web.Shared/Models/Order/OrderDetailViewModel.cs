using System;
using System.Collections.Generic;
using System.Text;

namespace Ecom.Web.Shared.Models.Order
{
    public class OrderDetailViewModel
    {
        public int Id { get; set; }
        public string OrderCode { get; set; } = null!;
        public decimal TotalAmount { get; set; }
        public string? Currency { get; set; }
        public string FullName { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string ShippingAddress { get; set; } = null!;
        public string? StatusName { get; set; }
        public DateTime? CreatedAt { get; set; }

        // Chỉ comment dòng quan trọng: Danh sách sản phẩm chi tiết (có thêm SKU, đơn giá...)
        public List<OrderItemDetailViewModel> OrderItems { get; set; } = new();

        // Chỉ comment dòng quan trọng: Thông tin giao dịch (PayPal/VNPAY) từ DTO TransactionInfo
        public TransactionViewModel? TransactionInfo { get; set; }
    }
}
