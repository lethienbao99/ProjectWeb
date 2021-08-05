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
using static ProjectWeb.Common.Enums.EnumConstants;

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

        public async Task<ResultMessage<bool>> Create(ProductCreateRequest request)
        {
            var sessions = _httpContextAccessor
                .HttpContext
                .Session
                .GetString(SystemsConstants.Token);


            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration[SystemsConstants.BaseURLApi]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var requestContent = new MultipartFormDataContent();

            if (request.ThumbnailImage != null)
            {
                byte[] data;
                using (var br = new BinaryReader(request.ThumbnailImage.OpenReadStream()))
                {
                    data = br.ReadBytes((int)request.ThumbnailImage.OpenReadStream().Length);
                }
                ByteArrayContent bytes = new ByteArrayContent(data);
                requestContent.Add(bytes, "thumbnailImage", request.ThumbnailImage.FileName);
            }


            requestContent.Add(new StringContent(request.Code.ToString()), "code");
            requestContent.Add(new StringContent(request.ProductName.ToString()), "productName");
            requestContent.Add(new StringContent(request.Description.ToString()), "description");
            requestContent.Add(new StringContent(request.Type.ToString()), "type");
            requestContent.Add(new StringContent(request.Status.ToString()), "status");
            requestContent.Add(new StringContent(request.Price.ToString()), "price");
            requestContent.Add(new StringContent(request.Stock.ToString()), "stock");
            requestContent.Add(new StringContent(request.Alias.ToString()), "alias");
            requestContent.Add(new StringContent(request.CategoryId.ToString()), "categoryId");

            var response = await client.PostAsync($"/api/products/", requestContent);
            var dataRaw = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<ResultObjectSuccess<bool>>(dataRaw);

            return JsonConvert.DeserializeObject<ResultObjectError<bool>>(dataRaw);

        }

        public async Task<ResultMessage<bool>> Delete(Guid ID)
        {
            return await DeteleAndReturnAsync<bool>($"/api/Products/{ID}");
        }

        public async Task<ResultMessage<ProductModel>> GetProductByID(Guid ID)
        {
            return await GetAndReturnAsync<ProductModel>($"/api/Products/{ID}"); 
        }

        public async Task<ResultMessage<PageResultModel<ProductModel>>> GetProductPaging(ProductPagingRequest request)
        {
            return await GetAndReturnAsync<PageResultModel<ProductModel>>($"/api/Products/Paging?pageIndex=" +
                $"{request.PageIndex}&pageSize={request.PageSize}&keyword={request.Keyword}&categoryId={request.CategoryId}");
        }

        public Task<ResultMessage<bool>> Update(Guid ID, ProductCreateRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
