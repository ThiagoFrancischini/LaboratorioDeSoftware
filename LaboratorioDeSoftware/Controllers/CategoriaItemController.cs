using LaboratorioDeSoftware.Core.Data;
using LaboratorioDeSoftware.Core.Entities;
using LaboratorioDeSoftware.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace LaboratorioDeSoftware.Controllers
{
    public class CategoriaItemController : Controller
    {
        private readonly CategoriaItemService _categoriaItemService;

        public CategoriaItemController(AppDbContext context)
        {
            _categoriaItemService = new CategoriaItemService(context);
        }

        public async Task<IActionResult> Index()
        {
            var categorias = await _categoriaItemService.ProcurarTodos();
            return View(categorias);
        }

        public IActionResult Cadastro()
        {
            return View(new CategoriaItem { Ativo = true }); // Por padrão já cria ativo
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cadastro(CategoriaItem categoriaItem)
        {
            try
            {
                await _categoriaItemService.Inserir(categoriaItem);
                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View(categoriaItem);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Editar(Guid id)
        {
            try
            {
                var categoria = await _categoriaItemService.ProcurarPorId(id);
                if (categoria == null)
                {
                    return NotFound();
                }

                return View(categoria);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Erro ao carregar categoria: " + ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(Guid id, CategoriaItem categoriaItem)
        {
            try
            {
                if (id != categoriaItem.Id)
                {
                    return NotFound();
                }

                await _categoriaItemService.Alterar(categoriaItem);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Erro ao salvar: " + ex.Message;
                return View(categoriaItem);
            }
        }    
    }
}