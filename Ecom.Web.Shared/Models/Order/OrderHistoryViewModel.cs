using System;
using System.Collections.Generic;
using System.Text;

namespace Ecom.Web.Shared.Models.Order
{
    public class OrderHistoryViewModel
    {
        public int Id { get; set; }
        public string OrderCode { get; set; } = null!;
        public decimal TotalAmount { get; set; }
        public string? Currency { get; set; }
        public string? StatusName { get; set; }
        public DateTime? CreatedAt { get; set; }

        // Chỉ comment dòng quan trọng: Danh sách item rút gọn để loop hiển thị ngoài danh sách
        public List<OrderItemSummaryViewModel> OrderItems { get; set; } = new();

        public int TotalItems => OrderItems.Sum(x => x.Quantity);
    }
}
