using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace StocksApp.Controllers
{
    public class HomeController : Controller
    {
        [Route("Error")]
        public IActionResult Error()
        {
            IExceptionHandlerPathFeature? exceptionHandlerPath =
                HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            if (exceptionHandlerPath != null && exceptionHandlerPath.Error != null)
            {
                ViewBag.ErrorMessage = exceptionHandlerPath.Error.Message;
            }
            return View();
        }
    }
}
