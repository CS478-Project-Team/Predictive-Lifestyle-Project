using Microsoft.AspNetCore.Mvc;

namespace Predictive_Lifestyle_Project.Controllers
{
    public class AboutController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
