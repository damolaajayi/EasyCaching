using Microsoft.AspNetCore.Mvc;

namespace EasyCaching.Controllers
{
    public class ValuesWithTwoProvidersController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
