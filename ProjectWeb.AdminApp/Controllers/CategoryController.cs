using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ProjectWeb.APIServices.IServiceBackendAPIs;
using ProjectWeb.Models.Categories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectWeb.AdminApp.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryBackendAPI _categoryBackendAPI;
        private readonly IConfiguration _config;
        public CategoryController(IConfiguration config, ICategoryBackendAPI categoryBackendAPI)
        {
            _categoryBackendAPI = categoryBackendAPI;
            _config = config;
        }
        public async Task<IActionResult> Index(string keyword, int pageIndex = 1, int pageSize = 5)
        {
            var request = new CategoryPagingRequest()
            {
                Keyword = keyword,
                PageIndex = pageIndex,
                PageSize = pageSize
            };
            ViewBag.Keyword = keyword;
            var data = await _categoryBackendAPI.GetAllPaging(request);

            //Phần này để lấy thông báo ở index khi các view khác trả message.
            if (TempData["SuccessMessage"] != null)
            {
                ViewBag.SuccessMessage = TempData["SuccessMessage"];
            }
            if (TempData["ErrorMessage"] != null)
            {
                ViewBag.ErrorMessage = TempData["ErrorMessage"];
            }

            return View(data.Object);
        }
    }
}
