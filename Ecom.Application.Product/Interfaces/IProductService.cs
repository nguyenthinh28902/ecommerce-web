using Ecom.Web.Shared.Models;
using Ecom.Web.Shared.Models.Product;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ecom.Application.Product.Interfaces
{
    public interface IProductService
    {
      Task<Result<HomeProductDisplayViewModel>> GetHomeProductDisplayViewModelAsync();
      Task<Result<ProductListViewModel>> GetProductsAsync(string slug, int page, string? searchTerm);
        Task<Result<ProductDetailViewModel>> GetProductDetailAsync(string slug, string? version);

    }
}
