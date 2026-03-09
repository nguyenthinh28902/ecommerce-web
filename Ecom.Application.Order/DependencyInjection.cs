using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ecom.Application.Product
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationOrderDependencyInjection(this IServiceCollection services, IConfiguration configuration)
        {
            return services;
        }

    }
}
