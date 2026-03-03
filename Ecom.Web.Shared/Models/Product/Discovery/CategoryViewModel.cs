using System;
using System.Collections.Generic;
using System.Text;

namespace Ecom.Web.Shared.Models.Product.Discovery
{
    public class CategoryViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string NameAscii { get; set; } = null!;
        public string? IconPath { get; set; }
        public List<CategoryViewModel> SubCategories { get; set; } = new(); // Cho menu đa cấp
    }
}
