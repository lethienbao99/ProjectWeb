﻿using Microsoft.AspNetCore.Mvc;
using ProjectWeb.APIServices.IServiceBackendAPIs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectWeb.EcommerceApp.Controllers.Components
{
    public class SlideProductsViewComponent : ViewComponent
    {
        private readonly IProductBackendAPI _productBackendAPI;
        public SlideProductsViewComponent(IProductBackendAPI productBackendAPI)
        {
            _productBackendAPI = productBackendAPI;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            /*var categories = await _categoryBackendAPI.GetAll();
            if (categories.Object != null)
                return View(categories.Object);*/
            return View("Default");
        }
    }

}
