using System;
using System.Collections.Generic;
using System.Text;

namespace Ecom.Web.Shared.Models.Payment
{
    public class PaymentResponse
    {
        public int OrderId { get; set; }
        public string OrderCode { get; set; } = null!;
        public string ApprovalUrl { get; set; } = null!; // Link để khách click thanh toán
        public bool IsSuccess { get; set; }
    }
}
