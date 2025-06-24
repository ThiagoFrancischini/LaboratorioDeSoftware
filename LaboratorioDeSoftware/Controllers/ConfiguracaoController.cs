using LaboratorioDeSoftware.Core.Data;
using LaboratorioDeSoftware.Core.Entities;
using LaboratorioDeSoftware.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace LaboratorioDeSoftware.Controllers
{
    public class ConfiguracaoController : Controller
    {
        private readonly ConfiguracaoService _configuracaoService;

        public ConfiguracaoController(AppDbContext context)
        {
            _configuracaoService = new ConfiguracaoService(context);
        }

        /// <summary>
        /// Ação GET: Chamada quando a página de configurações é aberta.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var model = await _configuracaoService.GetConfiguracoesAsync();
            return View(model);
        }

        /// <summary>
        /// Ação POST: Chamada quando o usuário clica no botão "Salvar".
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(ConfiguracaoSistema model)
        {
            if (ModelState.IsValid)
            {
                await _configuracaoService.SalvarConfiguracoesAsync(model);

                TempData["SuccessMessage"] = "Configurações salvas com sucesso!";

                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }
    }
}