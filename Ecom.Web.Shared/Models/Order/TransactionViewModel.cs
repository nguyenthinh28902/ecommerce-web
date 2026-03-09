using System;
using System.Collections.Generic;
using System.Text;

namespace Ecom.Web.Shared.Models.Order
{
    public class TransactionViewModel
    {
        public string PaymentMethodName { get; set; } = null!;
        public string ExternalTransactionId { get; set; } = null!; // Mã GD ngoại sàn
        public string StatusName { get; set; } = null!;
        public decimal Amount { get; set; }
        public string Currency { get; set; } = null!;
    }

}
