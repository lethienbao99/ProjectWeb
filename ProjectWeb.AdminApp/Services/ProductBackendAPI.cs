using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using ProjectWeb.AdminApp.IServiceBackendAPIs;
using ProjectWeb.Models.CommonModels;
using ProjectWeb.Models.Products;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ProjectWeb.AdminApp.Services
{
    public class ProductBackendAPI : BaseBackendAPI, IProductBackendAPI
    {

        public ProductBackendAPI(
            IHttpClientFactory httpClientFactory,
            IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor) : base(httpClientFactory, configuration, httpContextAccessor)
        {

        }

        public async Task<bool> Create(ProductCreateRequest request)
        {
            var Token = _httpContextAccessor.HttpContext.Session.GetString("Token");
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseURLApi"]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);

            var requestContent = new MultipartFormDataContent();
            if(request.ThumbnailImage != null)
            {
                byte[] data;
                using (var br = new BinaryReader(request.ThumbnailImage.OpenReadStream()))
                {
                    data = br.ReadBytes((int)request.ThumbnailImage.OpenReadStream().Length);
                }
                ByteArrayContent byteArray = new ByteArrayContent(data);
                requestContent.Add(byteArray, "ThumbnailImage", request.ThumbnailImage.FileName);

            }

            requestContent.Add(new StringContent(request.Code.ToString()), "Code");
            requestContent.Add(new StringContent(request.ProductName.ToString()), "ProductName");
            requestContent.Add(new StringContent(request.Description.ToString()), "Description");
            requestContent.Add(new StringContent(request.Type.ToString()), "Type");
            requestContent.Add(new StringContent(request.Status.ToString()), "Status");
            requestContent.Add(new StringContent(request.Price.ToString()), "Price");
            requestContent.Add(new StringContent(request.Stock.ToString()), "Stock");
            requestContent.Add(new StringContent(request.Alias.ToString()), "Alias");

            var response = await client.PostAsync("/api/products/", requestContent);
            return response.IsSuccessStatusCode;

        }

        public async Task<ResultMessage<PageResultModel<ProductModel>>> GetProductPaging(ProductPagingRequest request)
        {
            return await GetAndReturnAsync<PageResultModel<ProductModel>>($"/api/Products/Paging?pageIndex=" +
                $"{request.PageIndex}&pageSize={request.PageSize}&keyword={request.Keyword}");
        }

    }
}
