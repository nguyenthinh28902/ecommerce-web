using Ecom.Web.Shared.Models;
using Ecom.Web.Shared.Models.Checkout;
using Ecom.Web.Shared.Models.Order;
using Ecom.Web.Shared.Models.Payment;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ecom.Application.Order.Interfaces
{
    public interface IOrderService
    {
       Task<Result<CheckoutViewModel>> GetCheckoutInforAsync();
       Task<Result<PaymentResponse>> CheckoutAsync(CheckoutViewModel model);
        Task<Result<List<OrderHistoryViewModel>>> GetOrderHistoryAsync();
        Task<Result<OrderDetailViewModel>> GetOrderDetailAsync(string orderCode);
    }
}
