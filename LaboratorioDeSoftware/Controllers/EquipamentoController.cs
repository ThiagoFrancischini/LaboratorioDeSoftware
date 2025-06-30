using LaboratorioDeSoftware.Core.Data;
using LaboratorioDeSoftware.Core.DTOs.Filtros;
using LaboratorioDeSoftware.Core.Entities;
using LaboratorioDeSoftware.Core.Services;
using LaboratorioDeSoftware.Tools;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LaboratorioDeSoftware.Controllers
{
    public class EquipamentoController : Controller
    {
        private readonly EquipamentoService _equipamentoService;
        private readonly ProdutoService _produtoService;
        private readonly LaboratorioService _laboratorioService;
        private readonly CategoriaItemService _categoriaItemService;

        public EquipamentoController(AppDbContext context)
        {
            _equipamentoService = new EquipamentoService(context);
            _produtoService = new ProdutoService(context);
            _laboratorioService = new LaboratorioService(context);
            _categoriaItemService = new CategoriaItemService(context);
        }

        public async Task<IActionResult> Index([FromQuery] EquipamentoFiltroDTO filtro)
        {
            Guid userId = SessionTools.GetUserLogadoId(HttpContext);

            var laboratorios = await _laboratorioService.ProcurarTodos(userId);
            ViewBag.Laboratorios = new SelectList(laboratorios, "Id", "Nome", filtro.LaboratorioId);

            var categorias = await _categoriaItemService.ProcurarTodos();
            ViewBag.Categorias = new SelectList(categorias, "Id", "Descricao", filtro.CategoriaId);

            filtro.UserId = userId;

            var equipamentos = await _equipamentoService.ProcurarTodos(filtro);

            ViewBag.FiltroAtual = filtro;

            return View(equipamentos);
        }

        public async Task<IActionResult> Detalhes(Guid id)
        {
            try
            {
                var equipamento = await _equipamentoService.ProcurarPorId(id);
                if (equipamento == null)
                {
                    return NotFound();
                }

                await CarregarViewBags();
                return View(equipamento);
            }

            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Erro ao carregar equipamento: " + ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }


        public async Task<IActionResult> Cadastro()
        {
            await CarregarViewBags();
            return View(new Equipamento
            {
                DataColocacaoUso = DateTime.Now
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cadastro(Equipamento equipamento)
        {
            try
            {
                await _equipamentoService.Inserir(equipamento);
                
                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                await CarregarViewBags();
                return View(equipamento);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Editar(Guid id)
        {
            try
            {
                var equipamento = await _equipamentoService.ProcurarPorId(id);
                if (equipamento == null)
                {
                    return NotFound();
                }

                await CarregarViewBags();
                return View(equipamento);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Erro ao carregar equipamento: " + ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(Guid id, Equipamento equipamento)
        {
            try
            {
                if (id != equipamento.Id)
                {
                    return NotFound();
                }

                await _equipamentoService.Alterar(equipamento);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Erro ao salvar: " + ex.Message;
                await CarregarViewBags();
                return View(equipamento);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Deletar(Guid id)
        {
            try
            {
                var equipamento = await _equipamentoService.ProcurarPorId(id);
                if (equipamento == null)
                {
                    return NotFound();
                }
                return View(equipamento);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Erro ao carregar equipamento: " + ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletarConfirmado(Guid id)
        {
            try
            {
                var equipamento = await _equipamentoService.ProcurarPorId(id);
                if (equipamento != null)
                {
                    await _equipamentoService.Remover(equipamento);
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Erro ao excluir: " + ex.Message;
                var equipamento = await _equipamentoService.ProcurarPorId(id);
                return View("Deletar", equipamento);
            }
        }

        private async Task CarregarViewBags()
        {
            var produtos = await _produtoService.ProcurarTodos(new ProdutoFiltroDTO());
            Guid userId = SessionTools.GetUserLogadoId(HttpContext);

            var laboratorios = await _laboratorioService.ProcurarTodos(userId);

            ViewBag.Produtos = new SelectList(produtos, "Id", "Nome");
            ViewBag.Laboratorios = new SelectList(laboratorios, "Id", "Nome");
        }
    }
}