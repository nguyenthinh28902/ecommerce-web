using Ecom.Application.Product.Interfaces;
using Ecom.Application.Product.Models;
using Ecom.Web.Shared.Interfaces;
using Ecom.Web.Shared.Models;
using Ecom.Web.Shared.Models.Product.Discovery;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;

namespace Ecom.Application.Product.Services
{
    public class DiscoveryService : IDiscoveryService
    {
        private readonly ILogger<DiscoveryService> _logger;
        private readonly HttpClient _httpClient;
        private readonly ICacheService _cacheService;
        public DiscoveryService(ILogger<DiscoveryService> logger, HttpClient httpClient,
            ICacheService cacheService) {
        
            _logger = logger;
            _httpClient = httpClient;   
            _cacheService = cacheService;
        }

        public async Task<ProductFilterMenuViewModel> GetProductFilterMenuAsync()
        {
            const string cacheKey = "MenuFilters";

            // 1. Kiểm tra cache trước để tránh gọi API dư thừa
            var cachedMenu = _cacheService.Get<ProductFilterMenuViewModel>(cacheKey);
            if (cachedMenu != null) { return cachedMenu; }

            try
            {
                // 2. Gọi API lấy dữ liệu
                var response = await _httpClient.GetAsync(ConfigApiProductSupport.GetNavigation);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError($"{nameof(GetProductFilterMenuAsync)}: API thất bại với mã {response.StatusCode}");
                    return new ProductFilterMenuViewModel();
                }

                var result = await response.Content.ReadFromJsonAsync<Result<ProductFilterMenuViewModel>>();

                if (result?.Data == null || !result.IsSuccess)
                {
                    _logger.LogWarning($"{nameof(GetProductFilterMenuAsync)}: Dữ liệu API trả về rỗng hoặc không thành công");
                    return new ProductFilterMenuViewModel();
                }

                _cacheService.Set(cacheKey, result.Data);             

                return result.Data;
            }
            catch (Exception ex)
            {
                // Log lỗi exception (mạng, json, cache...)
                _logger.LogError(ex, $"{nameof(GetProductFilterMenuAsync)}: Có lỗi xảy ra khi xử lý dữ liệu");
                return new ProductFilterMenuViewModel();
            }
        }
    }
}
