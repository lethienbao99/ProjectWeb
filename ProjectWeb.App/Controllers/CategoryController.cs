using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectWeb.EcommerceApp.Controllers
{
    public class CategoryController : Controller
    {
        public IActionResult Detail()
        {
            return View();
        }
    }
}
