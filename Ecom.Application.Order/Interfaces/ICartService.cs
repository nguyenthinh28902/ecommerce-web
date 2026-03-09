using Ecom.Web.Shared.Models;
using Ecom.Web.Shared.Models.Cart;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ecom.Application.Order.Interfaces
{
    public interface ICartService
    {
        public Task<Result<bool>> CleanCartAsync();
        public Task<Result<CartViewModel>> GetCartAsync();
        public Task<Result<bool>> AddToCartAsync(int productId, int variantId, int quantity = 1);
    }
}
