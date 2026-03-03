using System;
using System.Collections.Generic;
using System.Text;

namespace Ecom.Web.Shared.Models.Product
{
    public class HomeProductDisplayViewModel
    {
        public List<ProductCardViewModel> NewArrivals { get; set; } = new();

        // Danh sách sản phẩm đang giảm giá sâu (Flash Sale)
        public List<ProductCardViewModel> BestDeals { get; set; } = new();
    }
}
