using LaboratorioDeSoftware.Core.Data;
using LaboratorioDeSoftware.Core.DTOs.Filtros;
using LaboratorioDeSoftware.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace LaboratorioDeSoftware.Core.Repositories
{
    public class CalibracaoRepository(AppDbContext _context)
    {
        public async Task<List<Calibracao>> ProcurarTodos(EventoFiltroDTO filtro)
        {
            IQueryable<Calibracao> query = _context.Calibracoes
                .Include(c => c.Equipamento)
                    .ThenInclude(e => e.Laboratorio);
            
            if (!string.IsNullOrWhiteSpace(filtro.Nome))
            {
                query = query.Where(c => c.Equipamento != null && c.Equipamento.Nome.ToUpper().Contains(filtro.Nome.ToUpper()));
            }
            
            if (filtro.LaboratorioId.HasValue && filtro.LaboratorioId.Value != Guid.Empty)
            {
                query = query.Where(c => c.Equipamento != null && c.Equipamento.LaboratorioId == filtro.LaboratorioId.Value);
            }
            
            if (filtro.DataInicio.HasValue)
            {
                query = query.Where(c => c.DataCalibracao >= filtro.DataInicio.Value);
            }
            
            if (filtro.DataFim.HasValue)
            {
                query = query.Where(c => c.DataCalibracao <= filtro.DataFim.Value);
            }

            return await query.ToListAsync();
        }

        public async Task<Calibracao> ProcurarPorId(Guid id)
        {
            return await _context.Calibracoes
                .Include(e => e.Equipamento)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<Calibracao> Inserir(Calibracao calibracao)
        {
            await _context.Calibracoes.AddAsync(calibracao);

            return calibracao;
        }

        public async Task<Calibracao> Alterar(Calibracao calibracao)
        {
            var calibracaoExistente = await ProcurarPorId(calibracao.Id);

            if (calibracaoExistente == null)
                throw new ApplicationException("Calibração não encontrada!");

            _context.Entry(calibracaoExistente).CurrentValues.SetValues(calibracao);
            
            // Atualiza os relacionamentos
            calibracaoExistente.EquipamentoId = calibracao.EquipamentoId;

            await _context.SaveChangesAsync();
            return calibracaoExistente;
        }

        public void Remover(Calibracao calibracao)
        {
            _context.Calibracoes.Remove(calibracao);
        }
        
    }
}