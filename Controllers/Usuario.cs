using Microsoft.AspNetCore.Mvc;

namespace Intranet_NEW.Controllers
{
    public class Usuario : Controller
    {
        public IActionResult Index()
        {
            return View();
        }


        private readonly ILogger<HomeController> _logger;
    }
}
