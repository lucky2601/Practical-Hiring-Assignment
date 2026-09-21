using Microsoft.AspNetCore.Mvc;

namespace PracticalHiring.Controllers
{
    public class Control : Controller
    {
        public IActionResult Index()
        {
            ViewBag.Message = "Hello from the Controller to control";
            return View();
        }
    }
}
