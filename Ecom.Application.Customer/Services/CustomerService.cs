using Ecom.Application.Customer.Interfaces;
using Ecom.Application.Customer.Model;
using Ecom.Web.Shared.Models;
using Ecom.Web.Shared.Models.Custom;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Text;

namespace Ecom.Application.Customer.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ILogger<CustomerService> _logger;
        private readonly HttpClient _httpClient;

        public CustomerService(ILogger<CustomerService> logger, HttpClient httpClient)
        {
            _logger = logger;
            _httpClient = httpClient;
        }

        public async Task<Result<CustomerViewModel>> GetCustomerInforAsync()
        {
            var customerInforUrl = ConfigApiCustomerService.GetCurrentCustomerInfo;

            try
            {
                _logger.LogInformation("truy xuất thông tin khách hàng mới nhất ");
                var response = await _httpClient.GetAsync(customerInforUrl);
                if (!response.IsSuccessStatusCode)
                { 
                    _logger.LogWarning("Lỗi khi truy xuất thông tin khách hàng: {StatusCode}", response.StatusCode);
                    return Result<CustomerViewModel>.Failure($"Lỗi khi truy xuất thông tin khách hàng: {response.StatusCode}");
                }
                var result = await response.Content.ReadFromJsonAsync<Result<CustomerViewModel>>();
                if (result == null || !result.IsSuccess || result.Data == null)
                {
                    _logger.LogWarning("Không nhận được dữ liệu từ API khách hàng.");
                    return Result<CustomerViewModel>.Failure("Không nhận được dữ liệu từ API khách hàng.");
                }
                _logger.LogInformation("Truy xuất thông tin khách hàng thành công.");
                return Result<CustomerViewModel>.Success(result.Data, result.Noti ?? "Truy xuất thông tin khách hàng thành công.");
            }
            catch (Exception ex)
            {

                throw;
            }

        }
    }
}
