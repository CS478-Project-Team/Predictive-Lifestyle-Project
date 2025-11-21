using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Predictive_Lifestyle_Project.Models;
using Predictive_Lifestyle_Project.Services;

namespace Predictive_Lifestyle_Project.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IEmailSender _emailSender;

    public HomeController(IEmailSender emailSender, ILogger<HomeController> logger)
    {
        this._emailSender = emailSender;
        _logger = logger;
    }


    //public HomeController(ILogger<HomeController> logger)
    //{
    //    _logger = logger;
    //}

    public async Task<IActionResult> Index()
    {
        var reciever = "xrvdfcwazigineethw@xfavaj.com"; //temperary email, using 10minutemail.com, replace with new mail for testing
        var subject = "Remember to log how you're feeling!";
        var message = "Please";

        await _emailSender.SendEmailAsync(reciever, subject, message);

        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
