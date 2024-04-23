using Microsoft.AspNetCore.Mvc;

namespace WebApplication3.Controllers
{
    public class DefaultController : Controller
    {
        public IActionResult Index()
        {
            return Content("123");
        }
    }
}
