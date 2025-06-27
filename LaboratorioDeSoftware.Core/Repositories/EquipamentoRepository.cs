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
                    .ThenInclude(p => p.Categoria)
                .Include(e => e.Laboratorio);
            
            if (filtro.LaboratorioId.HasValue && filtro.LaboratorioId.Value != Guid.Empty)
            {
                query = query.Where(x => x.LaboratorioId == filtro.LaboratorioId.Value);
            }
            
            if (filtro.Disponivel != DisponibilidadeFiltro.Todos)
            {
                query = query.Where(x => x.Disponivel == (filtro.Disponivel == DisponibilidadeFiltro.Sim));                
            }
            
            if (!string.IsNullOrWhiteSpace(filtro.NomeEquipamento))
            {
                query = query.Where(x => x.Nome.ToUpper().Contains(filtro.NomeEquipamento.ToUpper()));
            }
            
            if (filtro.CategoriaId.HasValue && filtro.CategoriaId.Value != Guid.Empty)
            {
                query = query.Where(x => x.Produto != null && x.Produto.CategoriaId == filtro.CategoriaId.Value);
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