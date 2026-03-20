using Ecom.Application.Product.Interfaces;
using Ecom.Application.Product.Models;
using Ecom.Web.Shared.Interfaces;
using Ecom.Web.Shared.Models;
using Ecom.Web.Shared.Models.Product;
using Ecom.Web.Shared.Models.Product.Discovery;
using Ecom.Web.Shared.Models.User;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Text;

namespace Ecom.Application.Product.Services
{
    public class ProductService : IProductService
    {
        private readonly ILogger<ProductService> _logger;
        private readonly HttpClient _httpClient;
        private readonly ICacheService _cacheService;
        public ProductService(ILogger<ProductService> logger, HttpClient httpClient, ICacheService cacheService) { 
            _logger = logger;
            _httpClient = httpClient;
            _cacheService = cacheService;
        }

        public async Task<Result<HomeProductDisplayViewModel>> GetHomeProductDisplayViewModelAsync()
        {
            const string cacheKey = "GetHomeProductDisplayViewModelAsync";

            // 1. Kiểm tra cache trước để tránh gọi API dư thừa
            var cachedProduct = _cacheService.Get<HomeProductDisplayViewModel>(cacheKey);
            if(cachedProduct != default && cachedProduct != null) { return Result<HomeProductDisplayViewModel>.Success(cachedProduct,"cache");  }

            var response = await _httpClient.GetAsync(ConfigApiProductService.GetGetProductHome);
            if(!response.IsSuccessStatusCode)
            {
                _logger.LogInformation($"{nameof(GetHomeProductDisplayViewModelAsync)}: lấy danh sách sản phẩm cho trang chủ thất bại{response.StatusCode}");
                return Result<HomeProductDisplayViewModel>.Failure("");
            }

            var jsonString = await response.Content.ReadAsStringAsync();
            var result = await response.Content.ReadFromJsonAsync<Result<HomeProductDisplayViewModel>>();
            
            if(result == null || !result.IsSuccess)
            {
                _logger.LogInformation($"{nameof(GetHomeProductDisplayViewModelAsync)}: lấy danh sách sản phẩm cho trang chủ thất bại");
                return Result<HomeProductDisplayViewModel>.Failure("");
            }

            _cacheService.Set(cacheKey, result.Data, TimeSpan.FromMinutes(5)); // 5 phút là quá đủ dùng cho trang lượng truy cập lớn.
            return result;
        }

        public async Task<Result<ProductListViewModel>> GetProductsAsync(string slug, int trang, string? timkiem)
        {
            // 1. Tách slug bằng logic đã chuẩn hóa (Sử dụng _ cho danh mục)
            var (cat, brand) = ParseSlug(slug);

            // 2. Đóng gói tham số vào Object theo đúng ProductQueryParameters
            var query = new ProductQueryParameters
            {
                CategorySlug = cat,
                BrandSlug = brand,
                Page = trang > 0 ? trang : 1,
                PageSize = 3,
                SearchTerm = timkiem
            };

            try
            {
                // 3. GỌI API BẰNG POST (Khớp với [HttpPost("danh-sach-san-pham")])
                // Truyền query vào body dưới dạng JSON
               
                var response = await _httpClient.PostAsJsonAsync(ConfigApiProductService.GetGetProducts, query);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError($"API lỗi: {response.StatusCode}");
                    return Result<ProductListViewModel>.Failure("Lỗi kết nối dịch vụ sản phẩm");
                }

                // 4. Giải mã kết quả trả về
                var result = await response.Content.ReadFromJsonAsync<Result<ProductListViewModel>>();
                return result ?? Result<ProductListViewModel>.Failure("Không có dữ liệu trả về");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi hệ thống khi gọi GetProducts API");
                return Result<ProductListViewModel>.Failure("Hệ thống đang bận, vui lòng thử lại sau");
            }
        }

        public async Task<Result<ProductDetailViewModel>> GetProductDetailAsync(string slug, string? version)
        {
            // 1. Build URL: Thay thế {NameAscii} và thêm query string cho phiên bản
            var url = ConfigApiProductService.GetGetProductDetail.Replace("{NameAscii}", slug);

            if (!string.IsNullOrEmpty(version))
            {
                url += $"?phienban={Uri.EscapeDataString(version)}";
            }

            try
            {
                // 2. Thực hiện Call API
                var response = await _httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError($"{nameof(GetProductDetailAsync)}: Lấy chi tiết sản phẩm thất bại. Status: {response.StatusCode}, Slug: {slug}");
                    return Result<ProductDetailViewModel>.Failure("Không thể kết nối đến máy chủ.");
                }

                // 3. Giải mã dữ liệu
                var result = await response.Content.ReadFromJsonAsync<Result<ProductDetailViewModel>>();

                if (result == null || !result.IsSuccess)
                {
                    _logger.LogWarning($"{nameof(GetProductDetailAsync)}: API trả về lỗi hoặc null cho Slug: {slug}");
                    return Result<ProductDetailViewModel>.Failure(result?.Noti ?? "Không tìm thấy thông tin sản phẩm.");
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{nameof(GetProductDetailAsync)}: Lỗi ngoại lệ khi gọi API cho Slug: {slug}");
                return Result<ProductDetailViewModel>.Failure("Đã có lỗi xảy ra trong quá trình xử lý.");
            }
        } 


        // Hàm tách slug riêng biệt để mã nguồn gọn gàng
        private (string? category, string? brand) ParseSlug(string slug)
        {
            if (string.IsNullOrEmpty(slug)) return (null, null);

            // Xử lý trường hợp có tiền tố "thuong-hieu-"
            if (slug.StartsWith("thuong-hieu-"))
            {
                // Đổi tên biến tạm thành brandName hoặc gán thẳng để tránh trùng với 'brand' ở trên
                string brandName = slug.Replace("thuong-hieu-", "");
                return (null, brandName);
            }

            // Xử lý trường hợp tách theo dấu gạch ngang (ví dụ: dtdd-apple)
            var parts = slug.Split('-', 2);

            string categoryResult = parts[0];
            string? brandResult = parts.Length > 1 ? parts[1] : null;

            return (categoryResult, brandResult);
        }
    }
}
