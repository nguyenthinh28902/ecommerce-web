using Ecom.Application.Authentication.Services;
using Ecom.Application.Customer.Interfaces;
using Ecom.Application.Customer.Model;
using Ecom.Application.Customer.Services;
using Ecom.Application.Order.Interfaces;
using Ecom.Application.Order.Models;
using Ecom.Application.Order.Services;
using Ecom.Application.Payment.Interfaces;
using Ecom.Application.Payment.Models;
using Ecom.Application.Payment.Service;
using Ecom.Application.Product.Interfaces;
using Ecom.Application.Product.Models;
using Ecom.Application.Product.Services;
using Ecom.Application.User.Models;
using Ecom.Application.User.Services;
using Ecom.Web.Shared.Interfaces.Auth;
using Ecom.Web.Shared.Interfaces.User;
using Ecom.Web.Shared.Models.Settings;

namespace Ecom.Web.Common.HeaderHandler
{
    public static class ApplicationHeadeHandler
    {
        public static IServiceCollection AddApplicationHeadeHandler(this IServiceCollection services, IConfiguration configuration)
        {
            var systemConfig = configuration.GetSection(nameof(ConfigClientIdentity)).Get<ConfigClientIdentity>();
            var configServiceUrl = configuration.GetSection(nameof(ConfigServiceUrl)).Get<ConfigServiceUrl>();
            if (systemConfig == null || configServiceUrl == null)
            {
                throw new Exception("Thiếu cấu hình SystemConfig hoặc GatewayUrl trong appsettings.json");
            }
            services.AddHttpClient<IAuthAppService, AuthAppService>(client =>
            {
                client.BaseAddress = new Uri($"{configServiceUrl.IdentityUrl}");
                // Để hẳn 10 phút cho thoải mái Debug
                client.Timeout = TimeSpan.FromMinutes(10);
            }); // Cần thêm dòng này
            services.AddHttpClient<IUserInformation, UserInformation>(client =>
            {
                client.BaseAddress = new Uri($"{configServiceUrl.GatewayUrl}{ConfigApiUser.GetDefault}");
                // Để hẳn 10 phút cho thoải mái Debug
                client.Timeout = TimeSpan.FromMinutes(10);
            }).AddHttpMessageHandler<AuthenticationHeaderHandler>(); // Cần thêm dòng này

            services.AddHttpClient<ICustomerService, CustomerService>(client =>
            {
                client.BaseAddress = new Uri($"{configServiceUrl.GatewayUrl}{ConfigApiCustomerService.GetDefault}");
                // Để hẳn 10 phút cho thoải mái Debug
                client.Timeout = TimeSpan.FromMinutes(10);
            }).AddHttpMessageHandler<AuthenticationHeaderHandler>(); // Cần thêm dòng này


            services.AddHttpClient<IPaymentService, PaymentService>(client =>
            {
                client.BaseAddress = new Uri($"{configServiceUrl.GatewayUrl}{ConfigApiPaymentService.GetDefault}");
                // Để hẳn 10 phút cho thoải mái Debug
                client.Timeout = TimeSpan.FromMinutes(10);
            }).AddHttpMessageHandler<AuthenticationHeaderHandler>();
            services.AddHttpClient<IProductService, ProductService>(client =>
            {
                client.BaseAddress = new Uri($"{configServiceUrl.GatewayUrl}{ConfigApiProductService.GetDefault}");
                // Để hẳn 10 phút cho thoải mái Debug
                client.Timeout = TimeSpan.FromMinutes(10);
            }).AddHttpMessageHandler<AuthenticationHeaderHandler>();
            services.AddHttpClient<IDiscoveryService, DiscoveryService>(client =>
            {
                client.BaseAddress = new Uri($"{configServiceUrl.GatewayUrl}{ConfigApiProductService.GetDefault}");
                // Để hẳn 10 phút cho thoải mái Debug
                client.Timeout = TimeSpan.FromMinutes(10);
            }).AddHttpMessageHandler<AuthenticationHeaderHandler>();

            services.AddHttpClient<ICartService, CartService>(client =>
            {
                client.BaseAddress = new Uri($"{configServiceUrl.GatewayUrl}{ConfigApiCartService.GetDefault}");
                // Để hẳn 10 phút cho thoải mái Debug
                client.Timeout = TimeSpan.FromMinutes(10);
            }).AddHttpMessageHandler<AuthenticationHeaderHandler>();

            services.AddHttpClient<IOrderService, OrderService>(client =>
            {
                client.BaseAddress = new Uri($"{configServiceUrl.GatewayUrl}{ConfigApiOrderService.GetDefault}");
                // Để hẳn 10 phút cho thoải mái Debug
                client.Timeout = TimeSpan.FromMinutes(10);
            }).AddHttpMessageHandler<AuthenticationHeaderHandler>();

            return services;
        }
    }
}
