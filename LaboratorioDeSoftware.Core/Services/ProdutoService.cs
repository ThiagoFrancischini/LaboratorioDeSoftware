using LaboratorioDeSoftware.Core.Data;
using LaboratorioDeSoftware.Core.Entities;
using ProdutoDeSoftware.Core.Repositories;

namespace LaboratorioDeSoftware.Core.Services
{
    public class ProdutoService
    {
        private AppDbContext _context;

        private ProdutoRepository produtoRepository;

        public ProdutoService(AppDbContext context)
        {
            _context = context;
            produtoRepository = new ProdutoRepository(context);
        }

        public async Task<List<Produto>> ProcurarTodos()
        {
            return await produtoRepository.ProcurarTodos();
        }

        public async Task<Produto> ProcurarPorId(Guid id)
        {
            return await produtoRepository.ProcurarPorId(id);
        }

        public async Task<Produto> Inserir(Produto produto)
        {
            produto.Id = Guid.NewGuid();

            produto.Validar();

            await produtoRepository.Inserir(produto);

            await _context.SaveChangesAsync();

            return produto;
        }

        public async Task<Produto> Alterar(Produto produto)
        {
            await produtoRepository.Alterar(produto);

            return produto;
        }

        public async Task Remover(Produto produto)
        {
            _context.Produtos.Remove(produto);

            await _context.SaveChangesAsync();
        }
    }
}
