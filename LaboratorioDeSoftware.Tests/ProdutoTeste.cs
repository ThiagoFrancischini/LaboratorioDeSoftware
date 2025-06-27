using Xunit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore; 
using LaboratorioDeSoftware.Core.Data;
using LaboratorioDeSoftware.Core.Entities;
using LaboratorioDeSoftware.Core.Services;
using LaboratorioDeSoftware.Core.DTOs.Filtros;
using static LaboratorioDeSoftware.Core.Entities.Enums.Enums;

namespace LaboratorioDeSoftware.Tests.Core.Services
{
    public class ProdutoServiceIntegrationTests
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
        public async Task Inserir_ProdutoValido_DeveAdicionarProdutoAoBanco()
        {            
            using var context = GetInMemoryDbContext(); 
            var produtoService = new ProdutoService(context);

            var produto = new Produto
            {
                Nome = "Teclado Mecânico",
                MarcaFabricante = "HyperX",
                Modelo = "Alloy Origins",
                TipoProduto = enTipoProduto.Analogico,
                Observacoes = "Produto para gamers",
                CategoriaId = Guid.NewGuid()
            };
            
            var result = await produtoService.Inserir(produto);
            
            Assert.NotNull(result);
            Assert.NotEqual(Guid.Empty, result.Id);
            
            var produtoNoBanco = await context.Produtos.FindAsync(result.Id);
            Assert.NotNull(produtoNoBanco);
            Assert.Equal(produto.Nome, produtoNoBanco.Nome);
        }

        [Fact]
        public async Task Inserir_ProdutoInvalido_DeveLancarApplicationException()
        {
            using var context = GetInMemoryDbContext();
            var produtoService = new ProdutoService(context);

            var produtoInvalido = new Produto
            {
                Nome = null,
                MarcaFabricante = "Marca",
                Modelo = "Modelo",
                TipoProduto = enTipoProduto.Digital,
                CategoriaId = Guid.NewGuid()
            };

            var exception = await Assert.ThrowsAsync<ApplicationException>(() => produtoService.Inserir(produtoInvalido));
            Assert.Equal("O nome do produto é obrigatório.", exception.Message);

            var produtosNoBanco = await context.Produtos.ToListAsync();
            Assert.Empty(produtosNoBanco);
        }

        [Fact]
        public async Task ProcurarPorId_ProdutoExiste_DeveRetornarProduto()
        {            
            using var context = GetInMemoryDbContext();
            var produtoService = new ProdutoService(context);

            var produtoExistente = new Produto
            {
                Id = Guid.NewGuid(),
                Nome = "Monitor",
                MarcaFabricante = "Dell",
                Modelo = "U2419H",
                TipoProduto = enTipoProduto.Analogico,
                CategoriaId = Guid.NewGuid(),
                Observacoes = " 1"
            };
            context.Produtos.Add(produtoExistente);
            await context.SaveChangesAsync();
            
            var result = await produtoService.ProcurarPorId(produtoExistente.Id);
            
            Assert.NotNull(result);
            Assert.Equal(produtoExistente.Id, result.Id);
            Assert.Equal(produtoExistente.Nome, result.Nome);
        }

        [Fact]
        public async Task ProcurarPorId_ProdutoNaoExiste_DeveRetornarNull()
        {
            using var context = GetInMemoryDbContext();
            var produtoService = new ProdutoService(context);
            var idNaoExistente = Guid.NewGuid();

            var result = await produtoService.ProcurarPorId(idNaoExistente);

            Assert.Null(result);
        }

        [Fact]
        public async Task Remover_ProdutoExistente_DeveRemoverProdutoDoBanco()
        {
            using var context = GetInMemoryDbContext();
            var produtoService = new ProdutoService(context);

            var produtoParaRemover = new Produto
            {
                Id = Guid.NewGuid(),
                Nome = "Mouse",
                MarcaFabricante = "Logitech",
                Modelo = "G203",
                TipoProduto = enTipoProduto.Digital,
                CategoriaId = Guid.NewGuid(),
                Observacoes = " 1",
            };
            context.Produtos.Add(produtoParaRemover);
            await context.SaveChangesAsync();
            Assert.NotNull(await context.Produtos.FindAsync(produtoParaRemover.Id));

            await produtoService.Remover(produtoParaRemover);

            var produtoRemovido = await context.Produtos.FindAsync(produtoParaRemover.Id);
            Assert.Null(produtoRemovido);
        }

        [Fact]
        public async Task Alterar_ProdutoExistente_DeveAtualizarProdutoNoBanco()
        {            
            using var context = GetInMemoryDbContext();
            var produtoService = new ProdutoService(context);

            var produtoOriginal = new Produto
            {
                Id = Guid.NewGuid(),
                Nome = "Headset Velho",
                MarcaFabricante = "Marca A",
                Modelo = "Modelo X",
                TipoProduto = enTipoProduto.Digital,
                CategoriaId = Guid.NewGuid(),
                Observacoes = "1 "
            };
            context.Produtos.Add(produtoOriginal);
            await context.SaveChangesAsync();

            var produtoAlterado = new Produto
            {
                Id = produtoOriginal.Id,
                Nome = "Headset Novo e Melhorado",
                MarcaFabricante = "Marca B",
                Modelo = "Modelo Y",
                TipoProduto = enTipoProduto.Analogico,
                CategoriaId = produtoOriginal.CategoriaId,
                Observacoes = "2 "
            };
            
            var result = await produtoService.Alterar(produtoAlterado);
            
            Assert.NotNull(result);
            Assert.Equal(produtoAlterado.Id, result.Id);
            Assert.Equal(produtoAlterado.Nome, result.Nome);
            Assert.Equal(produtoAlterado.MarcaFabricante, result.MarcaFabricante);

            var produtoNoBanco = await context.Produtos.AsNoTracking().FirstOrDefaultAsync(p => p.Id == produtoAlterado.Id);
            Assert.NotNull(produtoNoBanco);
            Assert.Equal("Headset Novo e Melhorado", produtoNoBanco.Nome);
            Assert.Equal("Marca B", produtoNoBanco.MarcaFabricante);
        }

        [Fact]
        public async Task ProcurarTodos_ComFiltro_DeveRetornarListaDeProdutosFiltrada()
        {            
            using var context = GetInMemoryDbContext();
            var produtoService = new ProdutoService(context);
            
            var produto1 = new Produto { Id = Guid.NewGuid(), Nome = "Produto A", MarcaFabricante = "M1", Modelo = "Mod1", TipoProduto = enTipoProduto.Digital, CategoriaId = Guid.NewGuid(), Observacoes = " " };
            var produto2 = new Produto { Id = Guid.NewGuid(), Nome = "Produto B", MarcaFabricante = "M2", Modelo = "Mod2", TipoProduto = enTipoProduto.Analogico, CategoriaId = Guid.NewGuid(), Observacoes = " " };
            var produto3 = new Produto { Id = Guid.NewGuid(), Nome = "Outro Produto A", MarcaFabricante = "M3", Modelo = "Mod3", TipoProduto = enTipoProduto.Digital, CategoriaId = Guid.NewGuid(), Observacoes = " " };

            await context.Produtos.AddAsync(produto1);
            await context.Produtos.AddAsync(produto2);
            await context.Produtos.AddAsync(produto3);
            await context.SaveChangesAsync();
            
            var result = await context.Produtos.ToListAsync();
            
            Assert.NotNull(result);
            Assert.Equal(3, result.Count);
        }
    }
}