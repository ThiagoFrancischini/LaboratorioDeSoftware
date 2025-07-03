

using LaboratorioDeSoftware.Core.Data;
using LaboratorioDeSoftware.Core.Entities;
using LaboratorioDeSoftware.Core.Services;
using Microsoft.EntityFrameworkCore;

namespace LaboratorioDeSoftware.Tests.Core.Services
{
    public class CategoriaItemServiceIntegrationTests
    {
        private AppDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var context = new AppDbContext(options);
            context.Database.EnsureCreated();
            return context;
        }

        [Fact]
        public async Task Inserir_CategoriaItemValido_DeveAdicionarCategoriaItemAoBanco()
        {
            using var context = GetInMemoryDbContext();
            var categoriaItemService = new CategoriaItemService(context);

            var categoriaItem = new CategoriaItem
            {
                Descricao = "Categoria de Eletrônicos",
                Ativo = true
            };

            var result = await categoriaItemService.Inserir(categoriaItem);

            Assert.NotNull(result);
            Assert.NotEqual(Guid.Empty, result.Id);

            var categoriaItemNoBanco = await context.Categorias.FindAsync(result.Id);
            Assert.NotNull(categoriaItemNoBanco);
            Assert.Equal(categoriaItem.Descricao, categoriaItemNoBanco.Descricao);
        }

        [Fact]
        public async Task ProcurarPorId_CategoriaItemExiste_DeveRetornarCategoriaItem()
        {
            using var context = GetInMemoryDbContext();
            var categoriaItemService = new CategoriaItemService(context);

            var categoriaItemExistente = new CategoriaItem
            {
                Id = Guid.NewGuid(),
                Descricao = "Categoria de Informática",
                Ativo = true
            };
            context.Categorias.Add(categoriaItemExistente);
            await context.SaveChangesAsync();

            var result = await categoriaItemService.ProcurarPorId(categoriaItemExistente.Id);

            Assert.NotNull(result);
            Assert.Equal(categoriaItemExistente.Id, result.Id);
            Assert.Equal(categoriaItemExistente.Descricao, result.Descricao);
        }

        [Fact]
        public async Task ProcurarPorId_CategoriaItemNaoExiste_DeveRetornarNull()
        {
            using var context = GetInMemoryDbContext();
            var categoriaItemService = new CategoriaItemService(context);
            var idNaoExistente = Guid.NewGuid();

            var result = await categoriaItemService.ProcurarPorId(idNaoExistente);

            Assert.Null(result);
        }

        [Fact]
        public async Task Remover_CategoriaItemExistente_DeveRemoverCategoriaItemDoBanco()
        {
            using var context = GetInMemoryDbContext();
            var categoriaItemService = new CategoriaItemService(context);

            var categoriaItemParaRemover = new CategoriaItem
            {
                Id = Guid.NewGuid(),
                Descricao = "Categoria de Jogos",
                Ativo = true
            };
            context.Categorias.Add(categoriaItemParaRemover);
            await context.SaveChangesAsync();
            Assert.NotNull(await context.Categorias.FindAsync(categoriaItemParaRemover.Id));

            await categoriaItemService.Remover(categoriaItemParaRemover);

            var categoriaItemRemovido = await context.Categorias.FindAsync(categoriaItemParaRemover.Id);
            Assert.Null(categoriaItemRemovido);
        }

        [Fact]
        public async Task Alterar_CategoriaItemExistente_DeveAtualizarCategoriaItemNoBanco()
        {
            using var context = GetInMemoryDbContext();
            var categoriaItemService = new CategoriaItemService(context);

            var categoriaItemOriginal = new CategoriaItem
            {
                Id = Guid.NewGuid(),
                Descricao = "Categoria de Roupas",
                Ativo = true
            };
            context.Categorias.Add(categoriaItemOriginal);
            await context.SaveChangesAsync();

            var categoriaItemAlterado = new CategoriaItem
            {
                Id = categoriaItemOriginal.Id,
                Descricao = "Categoria de Roupas e Acessórios",
                Ativo = true
            };

            var result = await categoriaItemService.Alterar(categoriaItemAlterado);

            Assert.NotNull(result);
            Assert.Equal(categoriaItemAlterado.Id, result.Id);
            Assert.Equal(categoriaItemAlterado.Descricao, result.Descricao);

            var categoriaItemNoBanco = await context.Categorias.AsNoTracking().FirstOrDefaultAsync(c => c.Id == categoriaItemAlterado.Id);
            Assert.NotNull(categoriaItemNoBanco);
            Assert.Equal("Categoria de Roupas e Acessórios", categoriaItemNoBanco.Descricao);
        }
    }
}
