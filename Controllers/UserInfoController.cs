using Microsoft.AspNetCore.Mvc;
using Predictive_Lifestyle_Project.Models;

namespace Predictive_Lifestyle_Project.Controllers
{
    public class UserInfoController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(UserInfo model)
        {
            if (ModelState.IsValid)
            {
                ViewBag.Message = $"Age: {model.Age}, Weight: {model.Weight}, Height: {model.Height}";
            }

            return View(model);
        }
    }
}
