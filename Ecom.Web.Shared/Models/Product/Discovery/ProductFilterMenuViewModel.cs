using System;
using System.Collections.Generic;
using System.Text;

namespace Ecom.Web.Shared.Models.Product.Discovery
{
    public class ProductFilterMenuViewModel
    {
        public List<BrandViewModel> Brands { get; set; } = new();
        public List<CategoryViewModel> Categories { get; set; } = new();
    }
}
