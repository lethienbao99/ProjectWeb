using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectWeb.AdminApp.Controllers.Components
{
    public class NavigationViewComponent: ViewComponent
    {
        //Tạm thòi chia cái top nav ra để mốt làm đa ngôn ngữ.
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View("Default");
        }
    }
}
