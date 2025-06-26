using LaboratorioDeSoftware.Core.Data;
using LaboratorioDeSoftware.Core.Entities;
using LaboratorioDeSoftware.Core.Repositories;
using Microsoft.EntityFrameworkCore;
using ProdutoDeSoftware.Core.Repositories;

namespace LaboratorioDeSoftware.Core.Services
{
    public class EquipamentoService
    {
        private readonly AppDbContext _context;
        private readonly EquipamentoRepository _equipamentoRepository;
        private readonly ProdutoRepository _produtoRepository;
        private readonly LaboratorioRepository _laboratorioRepository;

        public EquipamentoService(AppDbContext context )
        {
            _context = context;
            _equipamentoRepository = new EquipamentoRepository(context);
            _produtoRepository = new ProdutoRepository(context);
            _laboratorioRepository = new LaboratorioRepository(context);
        }

        public async Task<List<Equipamento>> ProcurarTodos()
        {
            return await _equipamentoRepository.ProcurarTodos();
        }

        public async Task<Equipamento> ProcurarPorId(Guid id)
        {
            return await _equipamentoRepository.ProcurarPorId(id);
        }

        public async Task<Equipamento> Inserir(Equipamento equipamento)
        {
            equipamento.Id = Guid.NewGuid();

            if (equipamento.ProdutoId == Guid.Empty)
                throw new ApplicationException("Informe um produto válido!");

            if (equipamento.LaboratorioId == Guid.Empty)
                throw new ApplicationException("Informe um laboratório válido!");

            foreach(var item in equipamento.Tags)
            {
                item.Id = Guid.NewGuid();
                item.EquipamentoId = equipamento.Id;
                item.Equipamento = equipamento;
            }

            await _equipamentoRepository.Inserir(equipamento);
            await _context.SaveChangesAsync();

            return equipamento;
        }

        public async Task<Equipamento> Alterar(Equipamento equipamentoDoForm)
        {
            var equipamentoNoBanco = await _context.Equipamentos
                .Include(e => e.Tags)
                .FirstOrDefaultAsync(e => e.Id == equipamentoDoForm.Id);

            if (equipamentoNoBanco == null)
            {
                throw new ApplicationException("Equipamento não encontrado!");
            }

            _context.Entry(equipamentoNoBanco).CurrentValues.SetValues(equipamentoDoForm);

            _context.TagsEquipamento.RemoveRange(_context.TagsEquipamento.Where(x => x.EquipamentoId == equipamentoDoForm.Id));

            foreach (var tag in equipamentoDoForm.Tags) 
            {
                tag.Id = Guid.NewGuid();
                tag.EquipamentoId = equipamentoDoForm.Id;
                _context.TagsEquipamento.Add(tag);
            }

            await _context.SaveChangesAsync();

            return equipamentoNoBanco;
        }

        public async Task Remover(Equipamento equipamento)
        {
            _equipamentoRepository.Remover(equipamento);
            await _context.SaveChangesAsync();
        }
    }
}