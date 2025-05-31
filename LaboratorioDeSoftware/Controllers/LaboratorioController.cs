using LaboratorioDeSoftware.Core.Data;
using LaboratorioDeSoftware.Core.Entities;
using LaboratorioDeSoftware.Core.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace LaboratorioDeSoftware.Controllers
{
    public class LaboratorioController : Controller
    {
        private LaboratorioService _laboratorioService;
        private UsuarioService _usuarioService;

        public LaboratorioController(AppDbContext _context)
        {
            _laboratorioService = new LaboratorioService(_context);
            _usuarioService = new UsuarioService(_context);
        }

        public async Task<IActionResult> Index()
        {
            var laboratorios = await _laboratorioService.ProcurarTodos();
            return View(laboratorios);
        }

        public async Task<IActionResult> Cadastro()
        {
            // Você precisará injetar o UsuarioService no controller
            var responsaveis = await _usuarioService.ProcurarTodos();
            ViewBag.Responsaveis = new SelectList(responsaveis, "Id", "Nome");

            return View(new Laboratorio());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cadastro(Laboratorio laboratorio)
        {
            try
            {
                await _laboratorioService.Inserir(laboratorio);
                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                var responsaveis = await _usuarioService.ProcurarTodos();
                ViewBag.Responsaveis = new SelectList(responsaveis, "Id", "Nome");
                return View(laboratorio);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Editar(Guid id)
        {
            try
            {
                var laboratorio = await _laboratorioService.ProcurarPorId(id);
                if (laboratorio == null)
                {
                    return NotFound();
                }

                var responsaveis = await _usuarioService.ProcurarTodos();
                ViewBag.Responsaveis = new SelectList(responsaveis, "Id", "Nome");
                return View(laboratorio);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Erro ao carregar laboratório: " + ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(Guid id, Laboratorio laboratorio)
        {
            try
            {
                if (id != laboratorio.Id)
                {
                    return NotFound();
                }

                await _laboratorioService.Alterar(laboratorio);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Erro ao salvar: " + ex.Message;
                var responsaveis = await _usuarioService.ProcurarTodos();
                ViewBag.Responsaveis = new SelectList(responsaveis, "Id", "Nome");
                return View(laboratorio);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Deletar(Guid id)
        {
            try
            {
                var laboratorio = await _laboratorioService.ProcurarPorId(id);
                if (laboratorio == null)
                {
                    return NotFound();
                }
                return View(laboratorio);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Erro ao carregar laboratório: " + ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost, ActionName("Deletar")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletarConfirmado(Guid id)
        {
            try
            {
                var laboratorio = await _laboratorioService.ProcurarPorId(id);
                if (laboratorio != null)
                {
                    await _laboratorioService.Remover(laboratorio);
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Erro ao excluir: " + ex.Message;
                var laboratorio = await _laboratorioService.ProcurarPorId(id);
                return View("Deletar", laboratorio);
            }
        }
    }
}
