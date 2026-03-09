using Ecom.Application.Payment.Interfaces;
using Ecom.Application.Payment.Models;
using Ecom.Web.Shared.Models;
using Ecom.Web.Shared.Models.Payment;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Text;

namespace Ecom.Application.Payment.Service
{
    public class PaymentService : IPaymentService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<PaymentService> _logger;

        public PaymentService(HttpClient httpClient, ILogger<PaymentService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<Result<List<PaymentMethodViewModel>>> GetActivePaymentMethodsAsync()
        {
            // Tạo sẵn dữ liệu mặc định để dùng khi API lỗi
            var defaultMethods = new List<PaymentMethodViewModel>
            {
                new PaymentMethodViewModel
                {
                    Id = 1,
                    Name = "Tiền mặt",
                    Code = "CASH"
                }
            };
            var defaultResult = Result<List<PaymentMethodViewModel>>.Success(defaultMethods, "Phương thức thanh toán.");
            try
            {
                // Chỉ comment dòng quan trọng: Gọi API lấy danh sách phương thức thanh toán từ Payment Service
                var response = await _httpClient.GetAsync(ConfigApiPaymentService.GetActiveMethods);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning("Payment Service lỗi {StatusCode}, sử dụng PTTT mặc định.", response.StatusCode);
                    return defaultResult;
                }

                var result = await response.Content.ReadFromJsonAsync<Result<List<PaymentMethodViewModel>>>();

                // Chỉ comment dòng quan trọng: Nếu dữ liệu null hoặc IsSuccess = false, trả về "Tiền mặt"
                if (result == null || !result.IsSuccess || result.Data == null || result.Data.Count == 0)
                {
                    return defaultResult;
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Không thể kết nối tới Payment Service, sử dụng PTTT mặc định.");

                // Trả về Success kèm dữ liệu mặc định để luồng Checkout không bị ngắt quãng
                return defaultResult;
            }
        }
    }
}
