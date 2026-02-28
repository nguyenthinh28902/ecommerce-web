using Ecom.Application.Product.Interfaces;
using Ecom.Web.Models;
using Ecom.Web.Services;
using Ecom.Web.Shared.Models.AuthWeb;
using Ecom.Web.Shared.Models.Dashboard;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Diagnostics;
using System.Security.Claims;

namespace Ecom.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductSummaryService _productSummaryService;
        public HomeController(ILogger<HomeController> logger, IProductSummaryService productSummaryService)
        {
            _logger = logger;
            _productSummaryService = productSummaryService;
        }
        public async Task<IActionResult> Index()
        {
            

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
