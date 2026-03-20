using Ecom.Web.Shared.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace Ecom.Application.Product.Services
{
    public class CacheService : ICacheService
    {
        private readonly ILogger<CacheService> _logger;
        private readonly IMemoryCache _memoryCache;
        // Thời gian hết hạn mặc định cache là 30 phút
        private readonly TimeSpan _defaultExpiration = TimeSpan.FromMinutes(30);
        public CacheService(ILogger<CacheService> logger, IMemoryCache memoryCache) {
            _logger = logger;
            _memoryCache = memoryCache;
        }
        public T Get<T>(string key)
        {
            if (_memoryCache.TryGetValue(key, out T value))
            {
                return value;
            }

            _logger.LogWarning("Cache Miss cho Key: {Key}", key);
            return default;
        }
        public void Set<T>(string key, T value, TimeSpan? expiration = null)
        {
            // Cấu hình các tùy chọn cho Cache Item
            var cacheOptions = new MemoryCacheEntryOptions
            {
                // Thuật toán dọn dẹp mặc định của IMemoryCache là SlidingExpiration kết hợp Priority
                AbsoluteExpirationRelativeToNow = expiration ?? _defaultExpiration,
                SlidingExpiration = TimeSpan.FromMinutes(10), // Nếu item không được truy cập trong 10 phút, nó sẽ bị dọn dẹp
                Priority = CacheItemPriority.High, // Ưu tiên giữ lại khi RAM đầy
                Size = 1 // Kích thước của item, giúp IMemoryCache quản lý bộ nhớ tốt hơn
            };

            _memoryCache.Set(key, value, cacheOptions);
            _logger.LogInformation("Đã lưu Cache cho Key: {Key}", key);
        }
        public void Remove(string key)
        {
            _memoryCache.Remove(key);
            _logger.LogInformation("Đã xóa Cache cho Key: {Key}", key);
        }

        public bool TryGet<T>(string key, out T value)
        {
            return _memoryCache.TryGetValue(key, out value);
        }

    }
}
