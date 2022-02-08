using Microsoft.AspNetCore.Mvc;
using ProjectWeb.AdminApp.Models;
using System.Threading.Tasks;

namespace ProjectWeb.AdminApp.Controllers.Components
{
    public class SummernoteViewComponent : ViewComponent
    {
        public Task<IViewComponentResult> InvokeAsync(SummernoteModel result)
        {
            return Task.FromResult((IViewComponentResult)View("Default", result));
        }
    }
}
