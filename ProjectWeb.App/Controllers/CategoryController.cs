using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ProjectWeb.APIServices.IServiceBackendAPIs;
using ProjectWeb.Models.Categories;
using ProjectWeb.Models.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static ProjectWeb.Common.Enums.EnumConstants;

namespace ProjectWeb.EcommerceApp.Controllers
{
    public class CategoryController : Controller
    {
        private readonly IProductBackendAPI _productBackendAPI;
        private readonly ICategoryBackendAPI _categoryBackendAPI;
        private readonly IConfiguration _config;
        public CategoryController(IProductBackendAPI productBackendAPI, IConfiguration config, ICategoryBackendAPI categoryBackendAPI)
        {
            _productBackendAPI = productBackendAPI;
            _config = config;
            _categoryBackendAPI = categoryBackendAPI;
        }
        public async Task<IActionResult> Detail(Guid ID, int pageIndex = 1, int pageSize = 4)
        {
            var BaseURLApi = _config[SystemsConstants.BaseURLApi];

            var request = new ProductPagingRequest()
            {
                Keyword = null, 
                PageIndex = pageIndex,
                PageSize = pageSize,
                CategoryId = ID
            };
            var category = await _categoryBackendAPI.GetCategoryByID(ID);
            var data = await _productBackendAPI.GetProductPaging(request);
            var result = new CategoryDetailViewModel();
            if (data.IsSuccessed)
            {
                result.Category = category;
                result.ItemProductsWithPaging = data;
                result.BaseURLApi = BaseURLApi;
            }

            return View(result);
        }
    }
}
