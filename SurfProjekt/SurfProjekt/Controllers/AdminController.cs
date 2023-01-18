using Microsoft.AspNetCore.Mvc;

namespace SurfProjekt.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
