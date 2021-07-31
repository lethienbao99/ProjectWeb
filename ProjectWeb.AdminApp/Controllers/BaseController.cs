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
        //Lưu token vô session nên để cái này rồi từ từ xử lý sau.
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var TokenInSession = context.HttpContext.Session.GetString("Token");
            if (TokenInSession == null)
            {
                context.Result = new RedirectToActionResult("Login", "SystemUser", null);
            }
            base.OnActionExecuting(context);
        }

    }
}
