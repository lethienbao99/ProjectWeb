using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ProjectWeb.APIServices.IServiceBackendAPIs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static ProjectWeb.Common.Enums.EnumConstants;

namespace ProjectWeb.EcommerceApp.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductBackendAPI _productBackendAPI;
        private readonly IConfiguration _config;
        public ProductController(IProductBackendAPI productBackendAP, IConfiguration config)
        {
            _config = config;
            _productBackendAPI = productBackendAP;
        }

        [HttpGet]
        public async Task<IActionResult> Detail(Guid id)
        {
            var BaseURLApi = _config[SystemsConstants.BaseURLApi];
            ViewBag.BaseURLApi = BaseURLApi;
            var result = await _productBackendAPI.GetProductByIDCustome(id);
            return View(result.Object);
        }

    }
}
