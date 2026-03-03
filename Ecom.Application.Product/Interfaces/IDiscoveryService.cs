using Ecom.Web.Shared.Models.Product.Discovery;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ecom.Application.Product.Interfaces
{
    public interface IDiscoveryService
    {
       Task<ProductFilterMenuViewModel> GetProductFilterMenuAsync();
    }
}
