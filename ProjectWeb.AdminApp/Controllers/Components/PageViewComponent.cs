using Microsoft.AspNetCore.Mvc;
using ProjectWeb.Models.CommonModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectWeb.AdminApp.Controllers.Components
{
    public class PageViewComponent : ViewComponent
    { 
        public Task<IViewComponentResult> InvokeAsync(PageResultViewBase result)
        {
            return Task.FromResult((IViewComponentResult)View("Default", result));
        }
    }
}
