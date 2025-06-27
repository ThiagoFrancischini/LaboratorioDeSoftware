using LaboratorioDeSoftware.Core.Data;
using LaboratorioDeSoftware.Core.DTOs.Filtros;
using LaboratorioDeSoftware.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace LaboratorioDeSoftware.Core.Repositories
{
    public class EquipamentoRepository(AppDbContext _context)
    {
        public async Task<List<Equipamento>> ProcurarTodos(EquipamentoFiltroDTO filtro)
        {
            IQueryable<Equipamento> query = _context.Equipamentos
                .Include(e => e.Produto)
                .Include(e => e.Laboratorio);
            
            if(filtro.LaboratorioId != null && filtro.LaboratorioId.HasValue)
            {
                query = query.Where(x => x.LaboratorioId == filtro.LaboratorioId);
            }

            if(filtro.Disponivel != null)
            {
                query = query.Where(x => x.Disponivel == filtro.Disponivel);
            }

            return await query.ToListAsync();
        }

        public async Task<Equipamento> ProcurarPorId(Guid id)
        {
            return await _context.Equipamentos
                .Include(e => e.Produto)
                .Include(e => e.Laboratorio)
                .Include(e => e.Tags)
                .Include(e => e.Calibracoes)
                .Include(e => e.Manutencoes)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<Equipamento> Inserir(Equipamento equipamento)
        {
            await _context.Equipamentos.AddAsync(equipamento);

            if(equipamento.Tags != null)
            {
                foreach(var tag in equipamento.Tags)
                {
                    await _context.TagsEquipamento.AddAsync(tag);
                }
            }

            return equipamento;
        }

        public async Task<Equipamento> Alterar(Equipamento equipamento)
        {
            var equipamentoExistente = await ProcurarPorId(equipamento.Id);

            if (equipamentoExistente == null)
                throw new ApplicationException("Equipamento não encontrado!");

            _context.Entry(equipamentoExistente).CurrentValues.SetValues(equipamento);
            
            // Atualiza os relacionamentos
            equipamentoExistente.ProdutoId = equipamento.ProdutoId;
            equipamentoExistente.LaboratorioId = equipamento.LaboratorioId;

            await _context.SaveChangesAsync();
            return equipamentoExistente;
        }

        public void Remover(Equipamento equipamento)
        {
            _context.Equipamentos.Remove(equipamento);
        }
    }
}