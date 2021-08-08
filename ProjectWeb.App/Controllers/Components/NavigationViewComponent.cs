using Microsoft.AspNetCore.Mvc;
using ProjectWeb.APIServices.IServiceBackendAPIs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectWeb.EcommerceApp.Controllers.Components
{
    public class NavigationViewComponent: ViewComponent
    {
        private readonly ICategoryBackendAPI _categoryBackendAPI;
        public NavigationViewComponent(ICategoryBackendAPI categoryBackendAPI)
        {
            _categoryBackendAPI = categoryBackendAPI;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var categories = await _categoryBackendAPI.GetAll();
            if(categories.Object != null)
                return View(categories.Object);
            return View();
        }
    }
}
