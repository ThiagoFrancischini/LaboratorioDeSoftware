using LaboratorioDeSoftware.Core.Data;
using LaboratorioDeSoftware.Core.Entities;
using LaboratorioDeSoftware.Core.Repositories;
using Microsoft.EntityFrameworkCore;
using ProdutoDeSoftware.Core.Repositories;

namespace LaboratorioDeSoftware.Core.Services
{
    public class CalibracaoService
    {
        private readonly AppDbContext _context;
        private readonly CalibracaoRepository _calibracaoRepository;
        private readonly Guid userId;

        public CalibracaoService(AppDbContext context)
        {
            this._context = context;
            _calibracaoRepository = new CalibracaoRepository(context);
        }

        public async Task<List<Calibracao>> ProcurarTodos()
        {
            return await _calibracaoRepository.ProcurarTodos();
        }

        public async Task<Calibracao> ProcurarPorId(Guid id)
        {
            return await _calibracaoRepository.ProcurarPorId(id);
        }

        public async Task<Calibracao> Inserir(Calibracao calibracao)
        {
            calibracao.Id = Guid.NewGuid();

            if (calibracao.EquipamentoId == Guid.Empty)
                throw new ApplicationException("Informe um equipamento válido!");

            try
            {
                await _calibracaoRepository.Inserir(calibracao);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                // Aqui você captura a inner exception real
                throw new ApplicationException($"Erro ao salvar a calibração: {ex.InnerException?.Message}", ex);
            }

            return calibracao;
        }


        public async Task<Calibracao> Alterar(Calibracao calibracaoDoForm)
        {
            var calibracaoNoBanco = await _context.Calibracoes
                .FirstOrDefaultAsync(e => e.Id == calibracaoDoForm.Id);

            if (calibracaoNoBanco == null)
            {
                throw new ApplicationException("Calibração não encontrada!");
            }

            _context.Entry(calibracaoNoBanco).CurrentValues.SetValues(calibracaoDoForm);

            await _context.SaveChangesAsync();

            return calibracaoNoBanco;
        }

        public async Task Remover(Calibracao calibracao)
        {
            _calibracaoRepository.Remover(calibracao);
            await _context.SaveChangesAsync();
        }
    }
}