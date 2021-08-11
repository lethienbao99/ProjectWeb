using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using ProjectWeb.APIServices.IServiceBackendAPIs;
using ProjectWeb.Models.CommonModels;
using ProjectWeb.Models.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ProjectWeb.APIServices.Services
{
    public class OrderBackendAPI : BaseBackendAPI, IOrderBackendAPI
    {

        public OrderBackendAPI(
          IHttpClientFactory httpClientFactory,
          IConfiguration configuration,
          IHttpContextAccessor httpContextAccessor) : base(httpClientFactory, configuration, httpContextAccessor)
        {

        }

        public async Task<ResultMessage<bool>> CreateOrder(OrderViewModel request)
        {
            return await PostAndReturnAsync<bool, OrderViewModel>("/api/Orders", request, false);
        }
    }
}
