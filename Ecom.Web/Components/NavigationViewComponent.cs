using Ecom.Application.Product.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Ecom.Web.Components
{
    public class NavigationViewComponent : ViewComponent
    {
        private readonly IDiscoveryService _discoveryService; // Service gọi sang Product API

        public NavigationViewComponent(IDiscoveryService discoveryService)
        {
            _discoveryService = discoveryService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            // Lấy dữ liệu (đã có cache bên trong Service như mình thảo luận)
            var data = await _discoveryService.GetProductFilterMenuAsync();
            return View(data);
        }
    }
}
