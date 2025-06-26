using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using LaboratorioDeSoftware.Filters;
using LaboratorioDeSoftware.Core.Data;
using LaboratorioDeSoftware.Core.Services;
using LaboratorioDeSoftware.Tools;

namespace LaboratorioDeSoftware.Controllers;
[AuthorizationFilter]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private EventoService _eventoService;

    public HomeController(ILogger<HomeController> logger, AppDbContext _context)
    {
        _logger = logger;
        _eventoService = new EventoService(_context);
    }

    public async Task<IActionResult> Index()
    {
        Guid userId = SessionTools.GetUserLogadoId(HttpContext);
        var avisosDeCalibracao = await _eventoService.GetAvisosDeCalibracaoAsync(userId);
        return View(avisosDeCalibracao);
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
