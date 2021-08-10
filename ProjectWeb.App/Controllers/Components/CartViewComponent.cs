using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectWeb.EcommerceApp.Controllers.Components
{
    public class CartViewComponent : ViewComponent
    {
        public CartViewComponent()
        {

        }


        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View("Default");
        }
    }
}
