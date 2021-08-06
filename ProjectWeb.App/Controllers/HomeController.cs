using LazZiya.ExpressLocalization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProjectWeb.APIServices.IServiceBackendAPIs;
using ProjectWeb.App.Models;
using ProjectWeb.Models.Home;
using ProjectWeb.Models.Products;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectWeb.App.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ISharedCultureLocalizer _loc;
        private readonly IProductBackendAPI _productBackendAPI;

        public HomeController(ILogger<HomeController> logger, ISharedCultureLocalizer loc, IProductBackendAPI productBackendAP)
        {
            _logger = logger;
            _loc = loc;
            _productBackendAPI = productBackendAP;
        }

        public async Task<IActionResult> Index()
        {
            var request = new ProductPagingRequest()
            {
                Keyword = null,
                PageIndex = 1,
                PageSize = 8,
            };
            var data = await _productBackendAPI.GetProductPaging(request);
            var viewHome = new HomeViewModel();
            if(data.IsSuccessed)
            {
                viewHome.ItemProducts = data.Object.Items;
            }

            return View(viewHome);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult SetCultureCookie(string cltr, string returnUrl)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(cltr)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
                );

            return LocalRedirect(returnUrl);
        }
    }
}
