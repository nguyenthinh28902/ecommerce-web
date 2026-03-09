using Ecom.Application.Order.Interfaces;
using Ecom.Application.Order.Models;
using Ecom.Web.Shared.Models;
using Ecom.Web.Shared.Models.Cart; // Giả định đặt ViewModel tại đây
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;

namespace Ecom.Application.Order.Services
{
    public class CartService : ICartService
    {
        private readonly ILogger<CartService> _logger;
        private readonly HttpClient _httpClient;

        public CartService(ILogger<CartService> logger, HttpClient httpClient)
        {
            _logger = logger;
            _httpClient = httpClient;
        }


        public async Task<Result<bool>> AddToCartAsync(int productId, int variantId , int quantity = 1)
        {
            try
            {
                // 1. Chuẩn bị URL từ Config đã có
              var url = $"{ConfigApiCartService.AddToCart}";

        // 2. Đóng gói dữ liệu (Request DTO khớp với Order API)
        var request = new { ProductId = productId, VariantId = variantId, Quantity = quantity };

                // Chỉ comment dòng quan trọng: Bắn request POST sang Order Service để lưu vào DB
                var response = await _httpClient.PostAsJsonAsync(url, request);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning("AddToCart: API trả về lỗi {StatusCode}", response.StatusCode);
                    return Result<bool>.Failure("Không thể thêm vào giỏ hàng.");
                }

                var result = await response.Content.ReadFromJsonAsync<Result<bool>>();
                return result ?? Result<bool>.Failure("Lỗi xử lý phản hồi.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi ngoại lệ khi AddToCart sản phẩm: {ProductId}", productId);
                return Result<bool>.Failure("Đã có lỗi xảy ra.");
            }
        }
        public async Task<Result<CartViewModel>> GetCartAsync()
        {
            try
            {
                // Chỉ comment dòng quan trọng: Gọi API lấy giỏ hàng từ Order Service
                var url = $"{ConfigApiCartService.GetCart}";
                var response = await _httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    return Result<CartViewModel>.Failure("Không thể lấy dữ liệu giỏ hàng.");
                }

                var result = await response.Content.ReadFromJsonAsync<Result<CartViewModel>>();

                if (result == null || !result.IsSuccess)
                    return Result<CartViewModel>.Failure(result?.Noti ?? "Lỗi dữ liệu.");

                // Ánh xạ từ DTO sang ViewModel để hiển thị
                var viewModel = new CartViewModel
                {
                    Items = result.Data.Items.Select(i => new CartItemViewModel
                    {
                        ProductId = i.ProductId,
                        ProductName = i.ProductName,
                        VariantName = i.VariantName,
                        UnitPrice = i.UnitPrice,
                        Quantity = i.Quantity,
                        MainImage = i.MainImage
                    }).ToList()
                };

                return Result<CartViewModel>.Success(viewModel, "Thành công.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi gọi GetCartAsync");
                return Result<CartViewModel>.Failure("Hệ thống giỏ hàng gặp sự cố.");
            }
        }

        public async Task<Result<bool>> CleanCartAsync()
        {
            // Chỉ comment dòng quan trọng: Gọi API xóa sạch giỏ hàng
            var url = $"{ConfigApiCartService.CleanCart}";
            var response = await _httpClient.DeleteAsync(url);

            if (!response.IsSuccessStatusCode) return Result<bool>.Failure("Xóa thất bại.");

            return await response.Content.ReadFromJsonAsync<Result<bool>>()
                   ?? Result<bool>.Failure("Lỗi kết nối.");
        }
    }
}