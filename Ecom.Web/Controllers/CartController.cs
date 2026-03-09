using Ecom.Application.Order.Interfaces;
using Ecom.Web.Shared.Models.Cart;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ecom.Web.Controllers
{
    // Đường dẫn thuần Việt cho toàn bộ Controller
    [Route("gio-hang")]
    [Authorize]
    public class CartController : Controller
    {
        private readonly ICartService _cartService;
        private readonly ILogger<CartController> _logger;

        public CartController(ICartService cartService, ILogger<CartController> logger)
        {
            _cartService = cartService;
            _logger = logger;
        }

        // Trang danh sách giỏ hàng: /gio-hang
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var result = await _cartService.GetCartAsync();

            // Nếu lấy dữ liệu lỗi hoặc không có, vẫn trả về View với model mới để hiện giao diện "Túi trống"
            return View(result.Data ?? new CartViewModel());
        }

        // API thêm sản phẩm: /gio-hang/them-moi
        [HttpPost("them-moi")]
        public async Task<IActionResult> AddToCart([FromBody] AddToCartRequest request)
        {
            if (request.ProductId <= 0) return Json(new { isSuccess = false, noti = "Sản phẩm không hợp lệ" });

            // Chỉ comment dòng quan trọng: Gọi xuống Application Service để bắn request sang Order Service
            var result = await _cartService.AddToCartAsync(request.ProductId,request.VariantId ,request.Quantity);

            return Json(result);
        }

        // API làm sạch giỏ hàng: /gio-hang/lam-sach
        [HttpPost("lam-sach")]
        public async Task<IActionResult> ClearCart()
        {
            var result = await _cartService.CleanCartAsync();

            // Nếu gọi từ Form thuần thì Redirect, nếu gọi từ Ajax thì trả về Json
            return RedirectToAction(nameof(Index));
        }
    }

    // Request DTO cho việc nhận dữ liệu từ View
    public record AddToCartRequest(int ProductId, int VariantId, int Quantity);
}