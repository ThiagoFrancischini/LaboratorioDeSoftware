using LaboratorioDeSoftware.Core.Data;
using LaboratorioDeSoftware.Core.Entities;
using LaboratorioDeSoftware.Core.Repositories;
using ProdutoDeSoftware.Core.Repositories;

namespace LaboratorioDeSoftware.Core.Services
{
    public class EquipamentoService
    {
        private readonly AppDbContext _context;
        private readonly EquipamentoRepository _equipamentoRepository;
        private readonly ProdutoRepository _produtoRepository;
        private readonly LaboratorioRepository _laboratorioRepository;

        public EquipamentoService(AppDbContext context)
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
            
            // Validações básicas
            if (equipamento.ProdutoId == Guid.Empty)
                throw new ApplicationException("Informe um produto válido!");
            
            if (equipamento.LaboratorioId == Guid.Empty)
                throw new ApplicationException("Informe um laboratório válido!");

            // Carrega as entidades relacionadas
            equipamento.Produto = await _produtoRepository.ProcurarPorId(equipamento.ProdutoId);
            equipamento.Laboratorio = await _laboratorioRepository.ProcurarPorId(equipamento.LaboratorioId);

            await _equipamentoRepository.Inserir(equipamento);
            await _context.SaveChangesAsync();

            return equipamento;
        }

        public async Task<Equipamento> Alterar(Equipamento equipamento)
        {
            // Validações
            if (equipamento.ProdutoId == Guid.Empty)
                throw new ApplicationException("Informe um produto válido!");
            
            if (equipamento.LaboratorioId == Guid.Empty)
                throw new ApplicationException("Informe um laboratório válido!");

            await _equipamentoRepository.Alterar(equipamento);
            return equipamento;
        }

        public async Task Remover(Equipamento equipamento)
        {
            _equipamentoRepository.Remover(equipamento);
            await _context.SaveChangesAsync();
        }
    }
}