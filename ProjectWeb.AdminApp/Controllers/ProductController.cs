using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using ProjectWeb.APIServices.IServiceBackendAPIs;
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
        private readonly ICategoryBackendAPI _categoryBackendAPI;
        private readonly IConfiguration _config;
        public ProductController(IProductBackendAPI productBackendAPI, IConfiguration config, ICategoryBackendAPI categoryBackendAPI)
        {
            _productBackendAPI = productBackendAPI;
            _categoryBackendAPI = categoryBackendAPI;
            _config = config;
        }
        public async Task<IActionResult> Index(string keyword, Guid? categoryId, int pageIndex = 1, int pageSize = 5)
        {
            var request = new ProductPagingRequest()
            {
                Keyword = keyword,
                PageIndex = pageIndex,
                PageSize = pageSize,
                CategoryId = categoryId
            };
            ViewBag.Keyword = keyword;
            var data = await _productBackendAPI.GetProductPaging(request);

            //Lấy líst category vào viewbag để làm dropdown tìm kiếm.
            var categories = await _categoryBackendAPI.GetAll();
            if(categories.Object != null)
            {
                ViewBag.Categories = categories.Object.Select(x =>
                    new SelectListItem()
                    {
                        Text = x.CategoryName,
                        Value = x.ID.ToString(),
                        Selected = categoryId.HasValue && categoryId.Value == x.ID ? true : false
                    });
            }
            

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
            var categories = await _categoryBackendAPI.GetAllByCreateOrUpdate();
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
        public async Task<IActionResult> Edit(Guid ID)
        {
            //Lấy líst category vào viewbag để làm dropdown tìm kiếm.
            var categories = await _categoryBackendAPI.GetAllByCreateOrUpdate();
            if (categories.Object != null)
            {
                ViewBag.Categories = categories.Object.Select(x =>
                    new SelectListItem()
                    {
                        Text = x.CategoryName,
                        Value = x.ID.ToString(),
                    });
            }

            var result = await _productBackendAPI.GetProductByIDCustome(ID);
            if (result.IsSuccessed)
            {
                var product = result.Object;
                var data = new ProductUpdateRequest()
                {
                    ID = product.ID,
                    ProductName = product.ProductName,
                    Code = product.Code,
                    CategoryId = product.CategoryId,
                    Stock = product.Stock,
                    Status = product.Status,
                    Price = product.Price,
                    PriceDollar = product.PriceDollar,
                    Alias = product.Alias,
                    Description = product.Description
                };
                return View(data);
            }
            return RedirectToAction("Error", "Home");
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Edit([FromForm] ProductUpdateRequest request)
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

            var result = await _productBackendAPI.Update(request);

            if (result.IsSuccessed)
            {
                TempData["SuccessMessage"] = "Cập nhật thành công";
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
