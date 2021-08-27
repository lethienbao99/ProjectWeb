using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var categories = await _categoryBackendAPI.GetAllByCreateOrUpdate(true);
            if (categories.Object != null)
            {
                ViewBag.Categories = categories.Object.Select(x =>
                    new SelectListItem()
                    {
                        Text = x.CategoryName,
                        Value = x.ID.ToString(),
                    });
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CategoryCreateOrUpdateRequest request)
        {
            var ErrorString = "";
            if (!ModelState.IsValid)
            {
                foreach (var item in ModelState.Root.Errors)
                {
                    ErrorString += item.ErrorMessage;
                }
                if (ErrorString == "")
                {
                    foreach (var item in ModelState.Root.Children)
                    {
                        if (item.Errors.Count > 0)
                        {
                            ErrorString += item.Errors[0].ErrorMessage;
                        }
                    }
                }
                TempData["ErrorMessage"] = ErrorString;
                if (TempData["ErrorMessage"] != null)
                {
                    ViewBag.ErrorMessage = TempData["ErrorMessage"];
                }
                return View();
            }

            var result = await _categoryBackendAPI.Create(request);

            if (result.IsSuccessed)
            {
                TempData["SuccessMessage"] = "Thêm mới thành công";
                return RedirectToAction("Index", "SystemUser");
            }

            ModelState.AddModelError("", result.Message);
            TempData["ErrorMessage"] = result.Message;
            if (TempData["ErrorMessage"] != null)
            {
                ViewBag.ErrorMessage = TempData["ErrorMessage"];
            }
            return View(request);
        }
    }
}
