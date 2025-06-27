using LaboratorioDeSoftware.Core.Data;
using LaboratorioDeSoftware.Core.DTOs.Filtros;
using LaboratorioDeSoftware.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace LaboratorioDeSoftware.Core.Repositories
{
    public class ManutencaoCorretivaRepository(AppDbContext _context)
    {
        public async Task<List<ManutencaoCorretiva>> ProcurarTodos(EventoFiltroDTO filtro)
        {
            IQueryable<ManutencaoCorretiva> query = _context.ManutencoesCorretivas
                .Include(m => m.Equipamento)
                    .ThenInclude(e => e.Laboratorio);

            if (!string.IsNullOrWhiteSpace(filtro.Nome))
            {
                query = query.Where(m => m.Equipamento != null && m.Equipamento.Nome.ToUpper().Contains(filtro.Nome.ToUpper()));
            }

            if (filtro.LaboratorioId.HasValue && filtro.LaboratorioId.Value != Guid.Empty)
            {
                query = query.Where(m => m.Equipamento != null && m.Equipamento.LaboratorioId == filtro.LaboratorioId.Value);
            }
            
            if (filtro.DataInicio.HasValue)
            {
                query = query.Where(m => m.DataProblemaApresentado >= filtro.DataInicio.Value);
            }
           
            if (filtro.DataFim.HasValue)
            {
                query = query.Where(m => m.DataProblemaApresentado <= filtro.DataFim.Value);
            }            

            return await query.ToListAsync();
        }

        public async Task<ManutencaoCorretiva> ProcurarPorId(Guid id)
        {
            return await _context.ManutencoesCorretivas
                .Include(m => m.Equipamento)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<ManutencaoCorretiva> Inserir(ManutencaoCorretiva manutencao)
        {
            await _context.ManutencoesCorretivas.AddAsync(manutencao);
            return manutencao;
        }

        public async Task<ManutencaoCorretiva> Alterar(ManutencaoCorretiva manutencao)
        {
            var existente = await ProcurarPorId(manutencao.Id)
                ?? throw new ApplicationException("Manutenção não encontrada!");

            _context.Entry(existente).CurrentValues.SetValues(manutencao);
            existente.EquipamentoId = manutencao.EquipamentoId;

            await _context.SaveChangesAsync();
            return existente;
        }

        public void Remover(ManutencaoCorretiva manutencao)
        {
            _context.ManutencoesCorretivas.Remove(manutencao);
        }
    }
}