using LaboratorioDeSoftware.Core.Data;
using LaboratorioDeSoftware.Core.Entities;
using LaboratorioDeSoftware.Core.Services;
using Microsoft.AspNetCore.Mvc;
using LaboratorioDeSoftware.Filters;

namespace LaboratorioDeSoftware.Controllers;

[AuthorizationFilter]
public class UsuarioController : Controller
{
    private readonly UsuarioService _usuarioService;

    public UsuarioController(AppDbContext context)
    {
        _usuarioService = new UsuarioService(context);
    }

    public async Task<IActionResult> Index()
    {
        var usuarios = await _usuarioService.ProcurarTodos();
        return View(usuarios);
    }

    public IActionResult Cadastro()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Usuario usuario)
    {
        try
        {
            if (ModelState.IsValid)
            {
                await _usuarioService.Registrar(usuario);
                return RedirectToAction(nameof(Index));
            }
        }
        catch (ApplicationException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
        }
        
        return View(usuario);
    }    
}