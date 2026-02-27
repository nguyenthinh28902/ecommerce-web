using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Ecom.Application.Authentication
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationAuthenticationDependencyInjection(this IServiceCollection services, IConfiguration configuration)
        {

            return services;
        }
    }
}

