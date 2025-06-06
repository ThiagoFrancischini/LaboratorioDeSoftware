using LaboratorioDeSoftware.Core.Data;
using LaboratorioDeSoftware.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace LaboratorioDeSoftware.Core.Repositories
{
    public class CategoriaItemRepository(AppDbContext _context)
    {
        public async Task<List<CategoriaItem>> ProcurarTodos()
        {
            return await _context.Categorias.ToListAsync();
        }

        public async Task<CategoriaItem> ProcurarPorId(Guid id)
        {
            return await _context.Categorias.FindAsync(id);
        }

        public async Task<CategoriaItem> Inserir(CategoriaItem categoria)
        {
            await _context.Categorias.AddAsync(categoria);

            return categoria;
        }

        public async Task<CategoriaItem> Alterar(CategoriaItem categoria)
        {
            var categoriaExistente = await ProcurarPorId(categoria.Id);

            if (categoriaExistente == null)
                throw new ApplicationException("Categoria não encontrado!");

            categoriaExistente.Descricao = categoria.Descricao;
            categoriaExistente.Ativo = categoria.Ativo;

            await _context.SaveChangesAsync();
            return categoriaExistente;
        }

        public void Remover(CategoriaItem cat)
        {
            _context.Categorias.Remove(cat);
        }
    }
}
