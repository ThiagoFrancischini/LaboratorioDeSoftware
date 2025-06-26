using LaboratorioDeSoftware.Core.Data;
using LaboratorioDeSoftware.Core.Entities;
using LaboratorioDeSoftware.Core.Services;
using LaboratorioDeSoftware.Tools;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LaboratorioDeSoftware.Controllers
{
    public class CalibracaoController : Controller
    {
        private readonly CalibracaoService _calibracaoService;
        private readonly EquipamentoService _equipamentoService;

        public CalibracaoController(AppDbContext context)
        {
            _calibracaoService = new CalibracaoService(context);
            _equipamentoService = new EquipamentoService(context);
        }

        public async Task<IActionResult> Index()
        {
            var calibracoes = await _calibracaoService.ProcurarTodos();
            return View(calibracoes);
        }

        public async Task<IActionResult> Cadastro()
        {
            await CarregarViewBags();
            return View(new Calibracao
            {
                DataCalibracao = DateTime.Now,
                DataAcompanhamento = DateTime.Now,
                DataSolicitacao = DateTime.Now,
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cadastro(Calibracao calibracao)
        {
            try
            {
                await _calibracaoService.Inserir(calibracao);
                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                await CarregarViewBags();
                return View(calibracao);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Editar(Guid id)
        {
            try
            {
                var calibracao = await _calibracaoService.ProcurarPorId(id);
                if (calibracao == null)
                {
                    return NotFound();
                }

                await CarregarViewBags();
                return View(calibracao);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Erro ao carregar a Calibração: " + ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(Guid id, Calibracao calibracao)
        {
            try
            {
                if (id != calibracao.Id)
                {
                    return NotFound();
                }

                await _calibracaoService.Alterar(calibracao);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Erro ao salvar: " + ex.Message;
                await CarregarViewBags();
                return View(calibracao);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Deletar(Guid id)
        {
            try
            {
                var calibracao = await _calibracaoService.ProcurarPorId(id);
                if (calibracao == null)
                {
                    return NotFound();
                }
                return View(calibracao);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Erro ao carregar a Calibração: " + ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletarConfirmado(Guid id)
        {
            try
            {
                var calibracao = await _calibracaoService.ProcurarPorId(id);
                if (calibracao != null)
                {
                    await _calibracaoService.Remover(calibracao);
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Erro ao excluir: " + ex.Message;
                var calibracao = await _calibracaoService.ProcurarPorId(id);
                return View("Deletar", calibracao);
            }
        }

        private async Task CarregarViewBags()
        {
            var equipamentos = await _equipamentoService.ProcurarTodos(new Core.DTOs.Filtros.EquipamentoFiltroDTO());

            if (equipamentos == null || !equipamentos.Any())
            {
                ViewBag.Equipamentos = new List<SelectListItem>(); 
                ViewBag.ErrorMessage = "Nenhum equipamento cadastrado.";
            }
            else
            {
                ViewBag.Equipamentos = new SelectList(equipamentos, "Id", "Nome");
            }
        }
    }
}