using System;
using System.Collections.Generic;
using System.Text;

namespace Ecom.Application.Product.Models
{
    public class ConfigApiProductService
    {
        public const string GetDefault = "/api/nganh-hang/";
        public const string GetGetProductHome = "san-pham/san-pham-trang-chu";
        public const string GetGetProducts = "san-pham/danh-sach-san-pham";
        public const string GetGetProductDetail = "san-pham/{NameAscii}";
    }
}
