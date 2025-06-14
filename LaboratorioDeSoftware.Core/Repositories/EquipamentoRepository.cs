using LaboratorioDeSoftware.Core.Data;
using LaboratorioDeSoftware.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace LaboratorioDeSoftware.Core.Repositories
{
    public class EquipamentoRepository(AppDbContext _context)
    {
        public async Task<List<Equipamento>> ProcurarTodos()
        {
            return await _context.Equipamentos
                .Include(e => e.Produto)
                .Include(e => e.Laboratorio)
                .ToListAsync();
        }

        public async Task<Equipamento> ProcurarPorId(Guid id)
        {
            return await _context.Equipamentos
                .Include(e => e.Produto)
                .Include(e => e.Laboratorio)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<Equipamento> Inserir(Equipamento equipamento)
        {
            await _context.Equipamentos.AddAsync(equipamento);
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