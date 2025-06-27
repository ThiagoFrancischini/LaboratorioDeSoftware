using LaboratorioDeSoftware.Core.Data;
using LaboratorioDeSoftware.Core.DTOs.Filtros;
using LaboratorioDeSoftware.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace ProdutoDeSoftware.Core.Repositories
{
    public class ProdutoRepository(AppDbContext _context)
    {
        public async Task<List<Produto>> ProcurarTodos(ProdutoFiltroDTO filtro)
        {
            IQueryable<Produto> query = _context.Produtos
                .Include(p => p.Categoria);
            
            if (!string.IsNullOrWhiteSpace(filtro.Nome))
            {
                query = query.Where(p => p.Nome.ToUpper().Contains(filtro.Nome.ToUpper()));
            }

            if (!string.IsNullOrWhiteSpace(filtro.MarcaFabricante))
            {
                query = query.Where(p => p.MarcaFabricante.ToUpper().Contains(filtro.MarcaFabricante.ToUpper()));
            }

            if (!string.IsNullOrWhiteSpace(filtro.Modelo))
            {
                query = query.Where(p => p.Modelo.ToUpper().Contains(filtro.Modelo.ToUpper()));
            }

            if (filtro.CategoriaId.HasValue && filtro.CategoriaId.Value != Guid.Empty)
            {
                query = query.Where(p => p.CategoriaId == filtro.CategoriaId.Value);
            }

            return await query.OrderBy(x => x.Modelo).ToListAsync();
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

