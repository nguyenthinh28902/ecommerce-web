using Ecom.Application.Product.Interfaces;
using Ecom.Web.Models;
using Ecom.Web.Shared.Models.Product;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Ecom.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductService _productService;
      
        public HomeController(ILogger<HomeController> logger, IProductService productService)
        {
            _logger = logger;
            _productService = productService;
        }
        public async Task<IActionResult> Index()
        {
            var result = await _productService.GetHomeProductDisplayViewModelAsync();
            var viewModel = result.IsSuccess ? result.Data : new HomeProductDisplayViewModel();
            return View(viewModel);
        }

        /// <summary>
        /// Trang chi tiết sản phẩm: /dtdd/iphone-17-pro-max
        /// </summary>
        [HttpGet("/{categorySlug}/{productSlug}")]
        public async Task<IActionResult> ProductDetail(string categorySlug, string productSlug, [FromQuery(Name = "phienban")] string? version = null)
        {
            _logger.LogInformation($"{nameof(ProductDetail)} start: Category={categorySlug}, Product={productSlug}, Version={version}");

            // Gọi API từ Service (Sử dụng HttpClient hoặc gọi trực tiếp Service tùy kiến trúc của ông)
            var result = await _productService.GetProductDetailAsync(productSlug, version);

            if (!result.IsSuccess || result.Data == null)
            {
                return NotFound();
            }

            return View(result.Data);
        }

        /// <summary>
        /// Trang danh sách sản phẩm: /dtdd hoặc /dtdd-apple-iphone
        /// </summary>
        [HttpGet("/{slug}")]
        public async Task<IActionResult> ProductList(string slug, [FromQuery(Name = "trang")] int page = 1, [FromQuery(Name = "timkiem")] string? searchTerm = null)
        {
            // Đẩy toàn bộ slug và params vào service xử lý nội bộ
            var result = await _productService.GetProductsAsync(slug, page, searchTerm);

            if (!result.IsSuccess) return RedirectToAction("Index", "Home");

            // Đẩy dữ liệu ra ViewBag để bộ phân trang (Pager) biết đường tạo link
            ViewBag.CurrentSlug = slug;
            ViewBag.CurrentPage = page;
            ViewBag.SearchTerm = searchTerm;

            return View("ProductList", result.Data); 
}
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
