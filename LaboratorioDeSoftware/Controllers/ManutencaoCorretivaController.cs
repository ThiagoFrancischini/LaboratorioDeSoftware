using LaboratorioDeSoftware.Core.Data;
using LaboratorioDeSoftware.Core.DTOs.Filtros;
using LaboratorioDeSoftware.Core.Entities;
using LaboratorioDeSoftware.Core.Services;
using LaboratorioDeSoftware.Tools;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using static LaboratorioDeSoftware.Core.Entities.Enums.Enums;

namespace LaboratorioDeSoftware.Controllers
{
    public class ManutencaoCorretivaController : Controller
    {
        private readonly ManutencaoCorretivaService _manutencaoService;
        private readonly EquipamentoService _equipamentoService;
        private readonly LaboratorioService _laboratorioService;

        public ManutencaoCorretivaController(AppDbContext context)
        {
            _manutencaoService = new ManutencaoCorretivaService(context);
            _equipamentoService = new EquipamentoService(context);
            _laboratorioService = new LaboratorioService(context);
        }

        public async Task<IActionResult> Index([FromQuery] EventoFiltroDTO filtro)
        {
            Guid userId = SessionTools.GetUserLogadoId(HttpContext);

            var laboratorios = await _laboratorioService.ProcurarTodos(userId);
            ViewBag.Laboratorios = new SelectList(laboratorios, "Id", "Nome", filtro.LaboratorioId);            

            var manutencoes = await _manutencaoService.ProcurarTodos(filtro);

            filtro.UsuarioId = userId;
            ViewBag.FiltroAtual = filtro;

            return View(manutencoes);
        }

        public async Task<IActionResult> Cadastro()
        {
            await CarregarViewBags();
            var model = new ManutencaoCorretiva
            {
                DataProblemaApresentado = DateTime.Now,
                DataRetorno = DateTime.Now
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cadastro(ManutencaoCorretiva manutencao)
        {
            try 
            { 
                await _manutencaoService.Inserir(manutencao);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                await CarregarViewBags();
                return View(manutencao);
            }
        }

        public async Task<IActionResult> Editar(Guid id)
        {
            var manutencao = await _manutencaoService.ProcurarPorId(id);
            if (manutencao == null)
            {
                return NotFound();
            }
            await CarregarViewBags();
            return View(manutencao);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(Guid id, ManutencaoCorretiva manutencao)
        {
            if (id != manutencao.Id)
            {
                return NotFound();
            }

            try
            {
                await _manutencaoService.Alterar(manutencao);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Erro ao salvar: " + ex.Message;
                await CarregarViewBags();
                return View(manutencao);
            }
        }

        public async Task<IActionResult> Deletar(Guid id)
        {
            var manutencao = await _manutencaoService.ProcurarPorId(id);
            if (manutencao == null)
            {
                return NotFound();
            }
            return View(manutencao);
        }

        [HttpPost, ActionName("Deletar")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletarConfirmado(Guid id)
        {
            try
            {
                var manutencao = await _manutencaoService.ProcurarPorId(id);
                if (manutencao != null)
                {
                    await _manutencaoService.Remover(manutencao);
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Erro ao excluir: " + ex.Message;
                var manutencao = await _manutencaoService.ProcurarPorId(id);
                return View("Deletar", manutencao);
            }
        }

        private async Task CarregarViewBags()
        {
            var userId = SessionTools.GetUserLogadoId(HttpContext);

            var equipamentos = await _equipamentoService.ProcurarTodos(new Core.DTOs.Filtros.EquipamentoFiltroDTO
            {
                UserId = userId
            });

            var listaEquipamentos = equipamentos.Select(e => new SelectListItem
            {
                Value = e.Id.ToString(),
                Text = $"{e.Nome} - Série: {e.Numero_Serie}"
            });

            ViewBag.Equipamentos = new SelectList(listaEquipamentos, "Value", "Text");
            ViewBag.StatusLista = new SelectList(Enum.GetValues(typeof(enStatusManutencoes)));
        }
    }
}