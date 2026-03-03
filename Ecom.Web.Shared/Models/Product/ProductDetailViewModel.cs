using System;
using System.Collections.Generic;
using System.Text;

namespace Ecom.Web.Shared.Models.Product
{
    public class ProductDetailViewModel
    {
        // --- Thông tin cơ bản ---
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string BrandName { get; set; } = null!;
        public string NameAscii { get; set; } = null!;
        public string Sku { get; set; } = null!;
        public string? VersionName { get; set; } // Ví dụ: "128GB"
        public string? ShortDescription { get; set; }
        public string? FullDescription { get; set; } // Map từ ProductGroup.Description
        public bool IsActive { get; set; }
        public string StatusString => IsActive ? "Đang kinh doanh" : "Tạm ngưng kinh doanh";

        // --- Giá & Kho (Dùng cho bản mặc định) ---
        public decimal Price { get; set; }
        public decimal? OriginalPrice { get; set; }
        public int? DiscountPercentage { get; set; }
        public bool IsStockAvailable { get; set; } // <--- Đã sửa ở đây
        public string CurrencyUnit { get; set; } = "₫";

        // --- Phân nhánh (Breadcrumbs) ---
        // Laptop > Gaming > ASUS
        public List<BreadcrumbViewModel> Breadcrumbs { get; set; } = new();

        // --- Media (Ảnh Slide) ---
        public List<ProductImageViewModel> Images { get; set; } = new();

        // --- Biến thể & Nhóm (Chọn Màu/Chọn Model) ---
        public List<ProductVariantViewModel> Variants { get; set; } = new(); // Chọn Màu
        public List<RelatedProductViewModel> GroupProducts { get; set; } = new(); // Các model khác trong cùng ProductGroup

        // --- Thông số kỹ thuật (Specs) ---
        public List<AttributeGroupViewModel> Specifications { get; set; } = new();
    }

    // --- Các Class hỗ trợ ---

    public class BreadcrumbViewModel
    {
        public string Name { get; set; } = null!;
        public string NameAscii { get; set; } = null!;
    }

    public class ProductImageViewModel
    {
        public string ImagePath { get; set; } = null!;
        public string? AltText { get; set; }
        public int? VariantId { get; set; } // Dùng để đổi ảnh khi chọn màu
    }

    public class ProductVariantViewModel
    {
        public int Id { get; set; }
        public string ColorName { get; set; } = null!;
        public string NameAscii { get; set; } = null!;
        public string? ColorCode { get; set; } // Mã HEX: #000000
        public decimal Price { get; set; }
        public string Sku { get; set; } = null!;
        public bool IsDefault { get; set; }
    }

    public class RelatedProductViewModel
    {
        public string Name { get; set; } = null!;
        public string NameAscii { get; set; } = null!;
        public string? VersionName { get; set; }
        public bool IsCurrent { get; set; } // Đánh dấu model đang xem
    }

    public class AttributeGroupViewModel
    {
        public string GroupName { get; set; } = null!; // Ví dụ: "Màn hình"
        public List<AttributeValueViewModel> Attributes { get; set; } = new();
    }

    public class AttributeValueViewModel
    {
        public string Key { get; set; } = null!;   // Ví dụ: "Kích thước"
        public string Value { get; set; } = null!; // Ví dụ: "6.7 inch"
    }
}
