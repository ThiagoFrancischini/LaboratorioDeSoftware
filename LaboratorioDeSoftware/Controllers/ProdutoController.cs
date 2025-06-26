using LaboratorioDeSoftware.Core.Data;
using LaboratorioDeSoftware.Core.Entities;
using LaboratorioDeSoftware.Core.Services;
using LaboratorioDeSoftware.Tools;
using Microsoft.AspNetCore.Mvc;

namespace LaboratorioDeSoftware.Controllers
{
    public class ProdutoController : Controller
    {
        private ProdutoService _produtoService;

        public ProdutoController(AppDbContext _context)
        {

            _produtoService = new ProdutoService(_context);
        }

        public async Task<IActionResult> Index()
        {
            var produtos = await _produtoService.ProcurarTodos();
            return View(produtos);
        }

        public async Task<IActionResult> Cadastro()
        {
            return View(new Produto());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cadastro(Produto produto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _produtoService.Inserir(produto);
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (ApplicationException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }

            return View(produto);
        }

        [HttpGet]
    public async Task<IActionResult> Editar(Guid id)
    {
        var usuario = await _produtoService.ProcurarPorId(id);
        if (usuario == null) return NotFound();
        return View(usuario);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Editar(Guid id, Produto produto)
    {
        try
        {
            if (id != produto.Id)
            {
                return NotFound();
            }

            await _produtoService.Alterar(produto);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ViewBag.ErrorMessage = "Erro ao salvar: " + ex.Message;
            return View(produto);
        }
    }
    
    [HttpGet]
    public async Task<IActionResult> Deletar(Guid id)
    {
        var usuario = await _produtoService.ProcurarPorId(id);
        if (usuario == null) return NotFound();
        return View(usuario);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Produto produto)
    {
        try
        {
            await _produtoService.Remover(produto);
            TempData["SuccessMessage"] = "Produto excluído com sucesso!";
        }
        catch (ApplicationException ex)
        {
            TempData["ErrorMessage"] = ex.Message;
            return RedirectToAction(nameof(Deletar), new { produto.Id });
        }
        
        return RedirectToAction(nameof(Index));
    }
    }
}