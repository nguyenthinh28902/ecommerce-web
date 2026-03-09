using Ecom.Web.Shared.Models;
using Ecom.Web.Shared.Models.Custom;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ecom.Application.Customer.Interfaces
{
    public interface ICustomerService
    {
        Task<Result<CustomerViewModel>> GetCustomerInforAsync();
    }
}
