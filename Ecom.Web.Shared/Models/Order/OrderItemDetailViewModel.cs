using System;
using System.Collections.Generic;
using System.Text;

namespace Ecom.Web.Shared.Models.Order
{
    public class OrderItemDetailViewModel
    {
        public string ProductName { get; set; } = null!;
        public string ProductMainImage { get; set; } = null!;
        public string Sku { get; set; } = null!;
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public decimal TotalLineAmount => UnitPrice * Quantity;
    }
}
