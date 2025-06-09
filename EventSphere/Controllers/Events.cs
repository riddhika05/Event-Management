using Microsoft.AspNetCore.Mvc;

namespace EventSphere.Controllers
{
    public class Events : Controller
    {
        public IActionResult Explore()
        {
            return View();
        }

        public IActionResult Host()
        {
            return View();
        }
    }
}