using System;
using System.Collections.Generic;
using System.Text;

namespace Ecom.Web.Shared.Models.Product
{
    public class ProductCardViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string NameAscii { get; set; } = null!;
        public string? MainImage { get; set; }

        // Giá gốc để hiển thị gạch ngang khi có giảm giá
        public decimal OriginalPrice { get; set; }
        public decimal Price { get; set; }
        public string? CurrencyUnit { get; set; }

        // Tên thương hiệu để hiển thị trên thẻ sản phẩm
        public string? BrandName { get; set; }
        public string? CategoryNameAscii { get; set; }

        // Thuộc tính bổ sung để gắn nhãn (New, Sale, v.v.)
        public bool IsNewArrival { get; set; }
        public DateTime? PublishDate { get; set; }

        // Tính toán % giảm giá trực tiếp để hiển thị Badge
        public int DiscountPercentage { get; set; }
    }
}
