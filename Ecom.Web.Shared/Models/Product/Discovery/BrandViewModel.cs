using System;
using System.Collections.Generic;
using System.Text;

namespace Ecom.Web.Shared.Models.Product.Discovery
{
    public class BrandViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string NameAscii { get; set; } = null!;
        public string? LogoUrl { get; set; }
    }
}
