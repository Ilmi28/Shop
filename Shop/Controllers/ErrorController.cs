using Microsoft.AspNetCore.Mvc;

namespace Shop.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
