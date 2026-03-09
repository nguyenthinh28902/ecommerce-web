using Ecom.Application.Order.Interfaces;
using Ecom.Web.Shared.Models.Checkout;
using Ecom.Web.Shared.Models.Order;
using Ecom.Web.Shared.Models.Payment;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecom.Web.Controllers
{
    [Route("don-hang")]
    [Authorize]
    public class OrderController : Controller
    {
        private readonly ILogger<OrderController> _logger;
        private readonly IOrderService _orderService;
        public OrderController(ILogger<OrderController> logger, IOrderService orderService)
        {
            _logger = logger;
            _orderService = orderService;
        }

        [HttpGet("xac-nhan-thong-tin-don-hang")]
        public async Task<ActionResult> Index()
        {
            // Chỉ comment dòng quan trọng: Gọi Service call API sang Order Service để lấy data checkout
            var result = await _orderService.GetCheckoutInforAsync();

            if (!result.IsSuccess)
            {
                // ĐÚNG: Sử dụng RedirectToAction để về trang chủ hoặc trang giỏ hàng
                return RedirectToAction("Index", "Home");

                // Hoặc nếu muốn về trang Giỏ hàng:
                // return RedirectToAction("GetCart", "CartWeb");
            }

            // Trả về View kèm theo CheckoutViewModel (result.Data)
            return View(result.Data);
        }

        [HttpPost("dat-hang")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PlaceOrder([FromForm]CheckoutViewModel model)
        {
            // 1. Kiểm tra tính hợp lệ của dữ liệu (Họ tên, SĐT, Địa chỉ, PTTT)
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                _logger.LogWarning("Dữ liệu Checkout không hợp lệ: {Errors}", string.Join(", ", errors));
                var checkoutData = await _orderService.GetCheckoutInforAsync();
                model.PaymentMethods = checkoutData.Data?.PaymentMethods ?? new();

                // 2. Chỉ comment dòng quan trọng: Trả về View "Index" cùng với model hiện tại để hiện lỗi Validation
                return View("Index", model);
            }

            _logger.LogInformation("Bắt đầu xử lý đặt hàng cho khách: {Name}", model.FullName);

            // 2. Chỉ comment dòng quan trọng: Gọi Service để thực hiện lưu đơn hàng và lấy link thanh toán (nếu có)
            var result = await _orderService.CheckoutAsync(model);

            if (result != null && result.Data != null && result.IsSuccess)
            {
                // 3. Chỉ comment dòng quan trọng: Nếu có link thanh toán (PayPal/VNPAY), redirect khách đi ngay
                if (!string.IsNullOrEmpty(result.Data.ApprovalUrl))
                {
                    return Redirect(result.Data.ApprovalUrl);
                }

               if(result.Data.IsSuccess && string.IsNullOrEmpty(result.Data.ApprovalUrl))
                    return RedirectToAction("Details", new { code = result.Data.OrderCode });
            }

            // Nếu xử lý thất bại từ phía API, hiển thị thông báo lỗi
            ModelState.AddModelError("", result?.Noti ?? "Đặt hàng thất bại, vui lòng thử lại.");
            return View("Index", model);
        }

        /// <summary>
        /// Trang lịch sử đơn hàng - Link: /don-hang/lich-su
        /// </summary>
        [HttpGet("lich-su-dat-hang")]
        public async Task<IActionResult> OrderHistory()
        {
            _logger.LogInformation("Khách hàng xem lịch sử đơn hàng");

            // Chỉ comment dòng quan trọng: Gọi service lấy danh sách đơn hàng đã rút gọn (Summary)
            var result = await _orderService.GetOrderHistoryAsync();

            if (!result.IsSuccess)
            {
                // Nếu lỗi hoặc chưa có đơn, vẫn trả về View nhưng với list rỗng
                return View(new List<OrderHistoryViewModel>());
            }

            return View(result.Data);
        }

        /// <summary>
        /// Trang chi tiết đơn hàng - Link: /don-hang/chi-tiet/{code}
        /// </summary>
        [HttpGet("chi-tiet-don-hang/{code}")]
        public async Task<IActionResult> Details(string code)
        {
            if (string.IsNullOrEmpty(code)) return RedirectToAction("OrderHistory");

            _logger.LogInformation("Khách hàng xem chi tiết đơn hàng: {OrderCode}", code);

            // Chỉ comment dòng quan trọng: Lấy chi tiết đơn hàng kèm thông tin thanh toán qua gRPC từ API
            var result = await _orderService.GetOrderDetailAsync(code);

            if (!result.IsSuccess)
            {
                return NotFound();
            }

            return View(result.Data);
        }
    }
}
