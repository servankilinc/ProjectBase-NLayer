using Microsoft.AspNetCore.Mvc;

namespace WebUI.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult Forbidden()
        {
            return View();
        }

        public IActionResult InternalServer()
        {
            return View();
        }

        public IActionResult InvalidProcess()
        {
            return View();
        }

        public new IActionResult NotFound()
        {
            return View();
        }
    }
}
