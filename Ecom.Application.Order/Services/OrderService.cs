using Ecom.Application.Customer.Interfaces;
using Ecom.Application.Customer.Model;
using Ecom.Application.Order.Interfaces;
using Ecom.Application.Order.Models;
using Ecom.Application.Payment.Interfaces;
using Ecom.Web.Shared.Models;
using Ecom.Web.Shared.Models.Checkout;
using Ecom.Web.Shared.Models.Order;
using Ecom.Web.Shared.Models.Payment;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Text;

namespace Ecom.Application.Order.Services
{
    public class OrderService : IOrderService
    {
        private readonly ILogger<OrderService> _logger;
        private readonly HttpClient _httpClient;
        private readonly ICustomerService _customerService;
        private readonly IPaymentService _paymentService;
        public OrderService(ILogger<OrderService> logger, HttpClient httpClient,
            ICustomerService customerService,
            IPaymentService paymentService)
        {
            _logger = logger;
            _httpClient = httpClient;
            _customerService = customerService;
            _paymentService = paymentService;
        }

        public async Task<Result<CheckoutViewModel>> GetCheckoutInforAsync()
        {
            try
            {
                _logger.LogInformation("Web MVC: Đang gọi API lấy thông tin Checkout...");

                // 1. Chỉ comment dòng quan trọng: Gọi API của Order Service (tương tự như cách CartService đang làm)
                var url = $"{ConfigApiOrderService.GetCheckoutInfor}";
                var response = await _httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning("GetCheckoutDetails: API trả về lỗi {StatusCode}", response.StatusCode);
                    return Result<CheckoutViewModel>.Failure("Không thể lấy dữ liệu thanh toán.");
                }

                // 2. Chỉ comment dòng quan trọng: Đọc Result<CheckoutDto> từ API trả về
                var apiResult = await response.Content.ReadFromJsonAsync<Result<CheckoutViewModel>>();

                if (apiResult == null || !apiResult.IsSuccess)
                    return Result<CheckoutViewModel>.Failure(apiResult?.Noti ?? "Lỗi dữ liệu.");

                // Get thong tin khách hàng để điền vào CheckoutViewModel (nếu cần)
              var customerResult = await _customerService.GetCustomerInforAsync();
                if (customerResult.IsSuccess && customerResult.Data != null)
                {
                    var customer = customerResult.Data;
                    // Nếu API Order Service chưa trả về thông tin khách hàng, ta có thể điền thêm vào CheckoutViewModel
                    if (apiResult.Data != null)
                    {
                        apiResult.Data.FullName = customer.DisplayName ?? string.Empty;
                        apiResult.Data.PhoneNumber = customer.PhoneNumber ?? string.Empty;
                        var paymentMethodsResult = await _paymentService.GetActivePaymentMethodsAsync();

                        apiResult.Data?.PaymentMethods = paymentMethodsResult.Data; //data tại đây đã check null ở trong func để trả về dữ liệu mẫu nên có thể dùng toán tử ?.
                    }
                }

                return Result<CheckoutViewModel>.Success(apiResult.Data ?? new CheckoutViewModel(), "Thành công");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi chuẩn bị dữ liệu Checkout");
                return Result<CheckoutViewModel>.Failure("Hệ thống thanh toán gặp sự cố.");
            }
        }

        public async Task<Result<PaymentResponse>> CheckoutAsync(CheckoutViewModel model)
        {
            try
            {
                _logger.LogInformation("Web MVC: Đang gửi yêu cầu đặt hàng cho {FullName}", model.FullName);

             
                var requestDto = new CheckoutRequestDto
                {
                    FullName = model.FullName,
                    PhoneNumber = model.PhoneNumber,
                    ShippingAddress = model.ShippingAddress,
                    PaymentMethodCode = model.SelectedPaymentMethodCode,
                    Note = "Đơn hàng đặt từ Web" 
                };

                
                var url = $"{ConfigApiOrderService.GetDefault}{ConfigApiOrderService.PlaceOrder}";
                var response = await _httpClient.PostAsJsonAsync(url, requestDto);

                if (!response.IsSuccessStatusCode)
                {
                    // Đọc nội dung lỗi từ API để trả về cho người dùng
                    var errorResult = await response.Content.ReadFromJsonAsync<Result<PaymentResponse>>();
                    _logger.LogWarning("Đặt hàng thất bại: {Message}", errorResult?.Noti);
                    return errorResult ?? Result<PaymentResponse>.Failure("Không thể kết nối với dịch vụ đặt hàng.");
                }

                var result = await response.Content.ReadFromJsonAsync<Result<PaymentResponse>>();

                if (result == null || result.Data == null)
                    return Result<PaymentResponse>.Failure("Lỗi xử lý phản hồi từ hệ thống.");

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi nghiêm trọng khi thực hiện đặt hàng");
                return Result<PaymentResponse>.Failure("Hệ thống gặp sự cố, vui lòng thử lại sau.");
            }
        }

        public async Task<Result<List<OrderHistoryViewModel>>> GetOrderHistoryAsync()
        {
            try
            {
                _logger.LogInformation("Web MVC: Đang lấy lịch sử đơn hàng...");

                // Chỉ comment dòng quan trọng: Gọi API lấy danh sách đơn hàng (sử dụng ConfigApi)
                var url = $"{ConfigApiOrderService.GetDefault}{ConfigApiOrderService.GetOrderHistory}";
                var response = await _httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                    return Result<List<OrderHistoryViewModel>>.Failure("Không thể lấy lịch sử đơn hàng.");

                var result = await response.Content.ReadFromJsonAsync<Result<List<OrderHistoryViewModel>>>();
                return result ?? Result<List<OrderHistoryViewModel>>.Failure("Dữ liệu trống.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi gọi API lịch sử đơn hàng");
                return Result<List<OrderHistoryViewModel>>.Failure("Hệ thống gặp sự cố khi tải dữ liệu.");
            }
        }

        public async Task<Result<OrderDetailViewModel>> GetOrderDetailAsync(string orderCode)
        {
            try
            {
                _logger.LogInformation("Web MVC: Đang lấy chi tiết đơn hàng {OrderCode}...", orderCode);

                // Chỉ comment dòng quan trọng: Tạo request body khớp với [FromBody] OrderDetailsRequest của API
                var requestBody = new { OrderCode = orderCode };
                var url = $"{ConfigApiOrderService.GetDefault}{ConfigApiOrderService.GetOrderDetail}";

                var response = await _httpClient.PostAsJsonAsync(url, requestBody);

                if (!response.IsSuccessStatusCode)
                    return Result<OrderDetailViewModel>.Failure("Không tìm thấy thông tin đơn hàng.");

                var result = await response.Content.ReadFromJsonAsync<Result<OrderDetailViewModel>>();
                return result ?? Result<OrderDetailViewModel>.Failure("Dữ liệu chi tiết trống.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi gọi API chi tiết đơn hàng");
                return Result<OrderDetailViewModel>.Failure("Lỗi kết nối dịch vụ đơn hàng.");
            }
        }
    }
}
