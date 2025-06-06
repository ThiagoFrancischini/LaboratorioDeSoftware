using LaboratorioDeSoftware.Core.Data;
using LaboratorioDeSoftware.Core.Entities;
using LaboratorioDeSoftware.Core.Repositories;

namespace LaboratorioDeSoftware.Core.Services
{
    public class CategoriaItemService
    {
        private readonly AppDbContext _context;
        private readonly CategoriaItemRepository _categoriaItemRepository;

        public CategoriaItemService(AppDbContext context)
        {
            _context = context;
            _categoriaItemRepository = new CategoriaItemRepository(context);
        }

        public async Task<List<CategoriaItem>> ProcurarTodos()
        {
            return await _categoriaItemRepository.ProcurarTodos();
        }

        public async Task<List<CategoriaItem>> ProcurarAtivos()
        {
            return await _categoriaItemRepository.ProcurarTodos();
        }

        public async Task<CategoriaItem> ProcurarPorId(Guid id)
        {
            return await _categoriaItemRepository.ProcurarPorId(id);
        }

        public async Task<CategoriaItem> Inserir(CategoriaItem categoriaItem)
        {
            categoriaItem.Id = Guid.NewGuid();
                        
            if (string.IsNullOrWhiteSpace(categoriaItem.Descricao))
                throw new ArgumentException("Descrição não pode ser vazia");

            await _categoriaItemRepository.Inserir(categoriaItem);
            await _context.SaveChangesAsync();

            return categoriaItem;
        }

        public async Task<CategoriaItem> Alterar(CategoriaItem categoriaItem)
        {            
            if (string.IsNullOrWhiteSpace(categoriaItem.Descricao))
                throw new ArgumentException("Descrição não pode ser vazia");

            await _categoriaItemRepository.Alterar(categoriaItem);
            await _context.SaveChangesAsync();

            return categoriaItem;
        }

        public async Task Remover(CategoriaItem categoriaItem)
        {
            _context.Categorias.Remove(categoriaItem);
            await _context.SaveChangesAsync();
        }

        public async Task Desativar(Guid id)
        {
            var categoria = await _categoriaItemRepository.ProcurarPorId(id);
            if (categoria != null)
            {
                categoria.Ativo = false;
                await _categoriaItemRepository.Alterar(categoria);
                await _context.SaveChangesAsync();
            }
        }

        public async Task Ativar(Guid id)
        {
            var categoria = await _categoriaItemRepository.ProcurarPorId(id);
            if (categoria != null)
            {
                categoria.Ativo = true;
                await _categoriaItemRepository.Alterar(categoria);
                await _context.SaveChangesAsync();
            }
        }
    }
}