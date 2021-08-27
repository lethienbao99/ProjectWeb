using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using ProjectWeb.APIServices.IServiceBackendAPIs;
using ProjectWeb.Models;
using ProjectWeb.Models.Categories;
using ProjectWeb.Models.CommonModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ProjectWeb.APIServices.Services
{
    public class CategoryBackendAPI : BaseBackendAPI, ICategoryBackendAPI
    {
        public CategoryBackendAPI(
            IHttpClientFactory httpClientFactory,
            IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor) : base(httpClientFactory, configuration, httpContextAccessor)
        {

        }

        public async Task<ResultMessage<int>> Create(CategoryCreateOrUpdateRequest request)
        {
            return await PostAndReturnAsync<int, CategoryCreateOrUpdateRequest>("/api/Categories/", request, true);
        }

        public async Task<ResultMessage<List<CategoryViewModel>>> GetAll()
        {
            return await GetAndReturnAsync<List<CategoryViewModel>>("/api/Categories/", false);
        }

        public async Task<ResultMessage<List<CategoryViewModel>>> GetAllByCreateOrUpdate(bool isParent)
        {
            return await GetAndReturnAsync<List<CategoryViewModel>>($"/api/Categories/forCreateOrUpdate?isParent={isParent}", true);
        }

        public async Task<ResultMessage<PageResultModel<CategoryViewModel>>> GetAllPaging(CategoryPagingRequest request)
        {
            return await GetAndReturnAsync<PageResultModel<CategoryViewModel>>($"/api/Categories/Paging?pageIndex=" +
                 $"{request.PageIndex}&pageSize={request.PageSize}&keyword={request.Keyword}", true);
        }

        public async Task<ResultMessage<CategoryViewModel>> GetCategoryByID(Guid ID)
        {
            return await GetAndReturnAsync<CategoryViewModel>($"/api/Categories/{ID}");
        }

        public async Task<ResultMessage<int>> Update(Guid ID, CategoryCreateOrUpdateRequest request)
        {
            return await PutAndReturnAsync<int, CategoryCreateOrUpdateRequest>($"/api/Categories/{ID}", request);
        }
    }
}
