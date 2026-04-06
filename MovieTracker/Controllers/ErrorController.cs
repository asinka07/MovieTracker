using Microsoft.AspNetCore.Mvc;

namespace MovieTracker.Controllers
{
    public class ErrorController : Controller
    {
        [Route("Error/{statusCode}")]
        public IActionResult Index(int statusCode)
        {
            if (statusCode == 404)
                return View("Error404");

            return View("Error500");
        }
    }
}
