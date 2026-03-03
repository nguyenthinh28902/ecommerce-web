using System;
using System.Collections.Generic;
using System.Text;

namespace Ecom.Web.Shared.Models.Product
{
    public class ProductListViewModel
    {
        // Danh sách sản phẩm dùng chung Card cũ của ông
        public List<ProductCardViewModel> Products { get; set; } = new List<ProductCardViewModel>();

        // Thông tin phân trang
        public int TotalItems { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }

        // Thông tin bổ trợ để hiển thị UI
        public string? CategoryName { get; set; }
        public string? BrandName { get; set; }
    }
}
