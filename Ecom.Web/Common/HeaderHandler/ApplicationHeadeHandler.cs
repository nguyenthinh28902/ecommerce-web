using Ecom.Application.Authentication.Services;
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


            services.AddHttpClient<IProductSummaryService, ProductSummaryService>(client =>
            {
                client.BaseAddress = new Uri($"{configServiceUrl.GatewayUrl}{ConfigApiProductService.GetDefault}");
                // Để hẳn 10 phút cho thoải mái Debug
                client.Timeout = TimeSpan.FromMinutes(10);
            }).AddHttpMessageHandler<AuthenticationHeaderHandler>(); // Cần thêm dòng này
            return services;
        }
    }
}
