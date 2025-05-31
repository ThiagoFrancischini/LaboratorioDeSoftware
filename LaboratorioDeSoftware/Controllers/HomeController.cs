using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using LaboratorioDeSoftware.Filters;

namespace LaboratorioDeSoftware.Controllers;
[AuthorizationFilter]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {        
        return View();
    }
}
