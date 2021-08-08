using Microsoft.AspNetCore.Mvc;
using ProjectWeb.Models.CommonModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectWeb.EcommerceApp.Controllers.Components
{
    public class PageViewComponent : ViewComponent
    {
        //Trả về view của phân trang.
        public Task<IViewComponentResult> InvokeAsync(PageResultViewBase result)
        {
            return Task.FromResult((IViewComponentResult)View("Default", result));
        }
    }
}
