using System;
using System.Collections.Generic;
using System.Text;

namespace Ecom.Web.Shared.Models.Payment
{
    public class PaymentMethodViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Code { get; set; } = null!;
    }
}
