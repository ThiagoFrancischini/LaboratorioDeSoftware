using LaboratorioDeSoftware.Core.Data;
using LaboratorioDeSoftware.Core.Entities;
using LaboratorioDeSoftware.Core.Services;
using Microsoft.AspNetCore.Mvc;
using LaboratorioDeSoftware.Filters;
using Microsoft.AspNetCore.Mvc.Rendering;
using LaboratorioDeSoftware.Tools;

namespace LaboratorioDeSoftware.Controllers;

[AuthorizationFilter]
public class UsuarioController : Controller
{
    private readonly UsuarioService _usuarioService;
    private readonly LaboratorioService _laboratorioService;

    public UsuarioController(AppDbContext context)
    {
        _usuarioService = new UsuarioService(context);

        _laboratorioService = new LaboratorioService(context);
    }

    public async Task<IActionResult> Index()
    {
        var usuarios = await _usuarioService.ProcurarTodos();
        return View(usuarios);
    }

    public async Task<IActionResult> Cadastro()
    {
        Guid userId = SessionTools.GetUserLogadoId(HttpContext);

        var laboratorios = await _laboratorioService.ProcurarTodos(userId);

        ViewBag.Laboratorios = new SelectList(laboratorios, "Id", "Nome");

        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Cadastro(Usuario usuario)
    {
        try
        {
            await _usuarioService.Registrar(usuario);
            return RedirectToAction(nameof(Index));
        }
        catch (ApplicationException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
        }

        return View(usuario);
    }

    [HttpGet]
    public async Task<IActionResult> Editar(Guid id)
    {
        var usuario = await _usuarioService.ProcurarPorId(id);

        Guid userId = SessionTools.GetUserLogadoId(HttpContext);

        var laboratorios = await _laboratorioService.ProcurarTodos(userId);

        ViewBag.Laboratorios = new SelectList(laboratorios, "Id", "Nome");

        if (usuario == null) return NotFound();

        return View(usuario);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Editar(Guid id, Usuario usuario)
    {
        if (id != usuario.Id) return NotFound();

        try
        {
            await _usuarioService.AtualizarUsuario(usuario);
            TempData["SuccessMessage"] = "Usuário atualizado com sucesso!";
            return RedirectToAction(nameof(Index));
        }
        catch (ApplicationException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
        }
        
        return View(usuario);
    }
    
    [HttpGet]
    public async Task<IActionResult> Deletar(Guid id)
    {
        var usuario = await _usuarioService.ProcurarPorId(id);
        if (usuario == null) return NotFound();
        return View(usuario);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        try
        {
            await _usuarioService.RemoverUsuario(id);
            TempData["SuccessMessage"] = "Usuário excluído com sucesso!";
        }
        catch (ApplicationException ex)
        {
            TempData["ErrorMessage"] = ex.Message;
            return RedirectToAction(nameof(Deletar), new { id });
        }
        
        return RedirectToAction(nameof(Index));
    }
}