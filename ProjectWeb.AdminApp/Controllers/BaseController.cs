using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectWeb.AdminApp.Controllers
{
    [Authorize]
    public class BaseController : Controller
    {
        //Lưu token vô session nên để cái này rồi từ từ xử lý sau. Vì nó bị miss cái Remember me.
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var Token = context.HttpContext.Request.Cookies["access_token"];

            if (Token == null)
            {
                context.Result = new RedirectToActionResult("Login", "SystemUser", null);
            }
            base.OnActionExecuting(context);
        }

    }
}
