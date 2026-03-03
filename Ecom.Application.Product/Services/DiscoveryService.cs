using Ecom.Application.Product.Interfaces;
using Ecom.Application.Product.Models;
using Ecom.Web.Shared.Models;
using Ecom.Web.Shared.Models.Product;
using Ecom.Web.Shared.Models.Product.Discovery;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Ecom.Application.Product.Services
{
    public class DiscoveryService : IDiscoveryService
    {
        private readonly ILogger<DiscoveryService> _logger;
        private readonly HttpClient _httpClient;
        private readonly IMemoryCache _memoryCache;
        public DiscoveryService(ILogger<DiscoveryService> logger, HttpClient httpClient, IMemoryCache memoryCache) {
        
            _logger = logger;
            _httpClient = httpClient;   
            _memoryCache = memoryCache;
        }

        public async Task<ProductFilterMenuViewModel> GetProductFilterMenuAsync()
        {
            const string cacheKey = "MenuFilters";

            // 1. Kiểm tra cache trước để tránh gọi API dư thừa
            if (_memoryCache.TryGetValue(cacheKey, out ProductFilterMenuViewModel? cachedData) && cachedData != null)
            {
                return cachedData;
            }

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

                // 3. Lưu vào RAM trong 30 phút cho lần gọi sau
                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(30))
                    .SetPriority(CacheItemPriority.Normal);

                _memoryCache.Set(cacheKey, result.Data, cacheOptions);

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
