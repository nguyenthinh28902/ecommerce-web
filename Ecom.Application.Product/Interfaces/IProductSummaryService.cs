using Ecom.Web.Shared.Models;
using Ecom.Web.Shared.Models.Dashboard;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ecom.Application.Product.Interfaces
{
    public interface IProductSummaryService
    {
        public Task<Result<DashboardViewModel>> GetProductSummaryDashboard();
    }
}
