using System;
using System.Collections.Generic;
using System.Text;

namespace Ecom.Web.Shared.Models.Cart
{
    public class CartViewModel
    {
        public List<CartItemViewModel> Items { get; set; } = new();
        public decimal GrandTotal => Items.Sum(x => x.TotalPrice);
        public int TotalItems => Items.Sum(x => x.Quantity);

        // Phí vận chuyển (Giả định miễn phí theo style Apple)
        public string ShippingText => "Miễn phí";
        public decimal TaxEstimate => GrandTotal * 0.1m; // Tạm tính 10% VAT
    }
}
