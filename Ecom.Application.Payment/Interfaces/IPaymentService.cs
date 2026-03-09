using Ecom.Web.Shared.Models;
using Ecom.Web.Shared.Models.Payment;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ecom.Application.Payment.Interfaces
{
    public interface IPaymentService
    {
        Task<Result<List<PaymentMethodViewModel>>> GetActivePaymentMethodsAsync();
    }
}
