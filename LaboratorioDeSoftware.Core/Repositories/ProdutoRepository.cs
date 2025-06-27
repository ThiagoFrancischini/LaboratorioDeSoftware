using LaboratorioDeSoftware.Core.Data;
using LaboratorioDeSoftware.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace ProdutoDeSoftware.Core.Repositories
{
    public class ProdutoRepository(AppDbContext _context)
    {
        public async Task<List<Produto>> ProcurarTodos()
        {
            return await _context.Produtos.OrderBy(x => x.Modelo).ToListAsync();
        }

        public async Task<Produto> ProcurarPorId(Guid id)
        {
            return await _context.Produtos.FindAsync(id);
        }

        public async Task<Produto> Inserir(Produto Produto)
        {
            await _context.Produtos.AddAsync(Produto);

            return Produto;
        }

        public async Task<Produto> Alterar(Produto produto)
        {
            var produtoExistente = await ProcurarPorId(produto.Id);

            if (produtoExistente == null)
                throw new ApplicationException("Produto não encontrado!");

            produtoExistente.Nome = produto.Nome;
            produtoExistente.MarcaFabricante = produto.MarcaFabricante;
            produtoExistente.Modelo = produto.Modelo;
            produtoExistente.TipoProduto = produto.TipoProduto;
            produtoExistente.Observacoes = produto.Observacoes;
            produtoExistente.CategoriaId = produto.CategoriaId;

            await _context.SaveChangesAsync();
            return produtoExistente;
        }

        public void Remover(Produto produto)
        {
            _context.Produtos.Remove(produto);
        }
    }
}

