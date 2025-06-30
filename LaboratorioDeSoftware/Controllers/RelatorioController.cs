using LaboratorioDeSoftware.Core.Data;
using LaboratorioDeSoftware.Core.DTOs.Filtros;
using LaboratorioDeSoftware.Core.Services;
using LaboratorioDeSoftware.Tools;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LaboratorioDeSoftware.Controllers
{
    public class RelatorioController : Controller
    {

        private readonly EquipamentoService _equipamentoService;
        private readonly LaboratorioService _laboratorioService;
        private readonly RelatorioService _relatorioService;

        public RelatorioController(AppDbContext context)
        {
            _equipamentoService = new EquipamentoService(context);
            _laboratorioService = new LaboratorioService(context);
            _relatorioService = new RelatorioService(context);
        }

        public async Task<IActionResult> Index([FromQuery] RelatorioFiltroDTO filtro)
        {
            Guid userId = SessionTools.GetUserLogadoId(HttpContext);

            var laboratorios = await _laboratorioService.ProcurarTodos(userId);
            ViewBag.Laboratorios = new SelectList(laboratorios, "Id", "Nome", filtro.LaboratorioId);

            var filtroEquipamento = new EquipamentoFiltroDTO
            {
                UserId = userId
            };
            var equipamentos = await _equipamentoService.ProcurarTodos(filtroEquipamento);
            ViewBag.Equipamentos = new SelectList(equipamentos, "Id", "Nome", filtro.EquipamentoId);

            ViewBag.FiltroAtual = filtro;

            filtro.UserId = userId;

            var relatorio = await _relatorioService.BuscarRelatorio(filtro);

            return View(relatorio);
        }
    }
}
