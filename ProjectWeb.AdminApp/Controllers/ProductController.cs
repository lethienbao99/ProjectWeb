﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ProjectWeb.AdminApp.IServiceBackendAPIs;
using ProjectWeb.Models.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectWeb.AdminApp.Controllers
{
    
    public class ProductController : Controller
    {
        private readonly IProductBackendAPI _productBackendAPI;
        private readonly IConfiguration _config;
        public ProductController(IProductBackendAPI productBackendAPI, IConfiguration config)
        {
            _productBackendAPI = productBackendAPI;
            _config = config;
        }
        public async Task<IActionResult> Index(string keyword, int pageIndex = 1, int pageSize = 5)
        {
            var request = new ProductPagingRequest()
            {
                Keyword = keyword,
                PageIndex = pageIndex,
                PageSize = pageSize,
            };
            ViewBag.Keyword = keyword;
            if (TempData["SuccessMessage"] != null)
            {
                ViewBag.SuccessMessage = TempData["SuccessMessage"];
            }
            if (TempData["ErrorMessage"] != null)
            {
                ViewBag.ErrorMessage = TempData["ErrorMessage"];
            }
            var data = await _productBackendAPI.GetProductPaging(request);
            return View(data.Object);

        }


        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Create([FromForm] ProductCreateRequest request)
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

            var result = await _productBackendAPI.Create(request);

            if (result.IsSuccessed)
            {
                TempData["SuccessMessage"] = "Thêm mới thành công";
                return RedirectToAction("Index", "Product");
            }

            ModelState.AddModelError("", result.Message);
            TempData["ErrorMessage"] = result.Message;
            if (TempData["ErrorMessage"] != null)
            {
                ViewBag.ErrorMessage = TempData["ErrorMessage"];
            }
            return View(request);
        }


        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _productBackendAPI.GetProductByID(id);
            return PartialView("_DeteleModalPartial", result.Object);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(ProductModel model)
        {
            var result = await _productBackendAPI.Delete(model.ID);
            if (result.IsSuccessed)
            {
                TempData["SuccessMessage"] = "Xóa thành công";
                return RedirectToAction("Index", "SystemUser");
            }

            TempData["ErrorMessage"] = result.Message;
            return RedirectToAction("Index", "SystemUser");
        }

    }
}
