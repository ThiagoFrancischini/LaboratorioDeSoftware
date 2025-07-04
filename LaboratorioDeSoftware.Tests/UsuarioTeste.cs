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
    public class UsuarioServiceIntegrationTests
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
        public async Task Inserir_UsuarioValido_DeveAdicionarUsuarioAoBanco()
        {
            using var context = GetInMemoryDbContext();
            var usuarioService = new UsuarioService(context);

            var usuario = new Usuario
            {
                Nome = "João Silva",
                Email = "joao.silva@example.com",
                Senha = "Senha123",
                TipoUsuario = enTipoUsuario.Administrador,
                Cargo = "Cargo",
                Telefone = "54 99800-5773"
            };

            var result = await usuarioService.Registrar(usuario);

            Assert.NotNull(result);
            Assert.NotEqual(Guid.Empty, result.Id);

            var usuarioNoBanco = await context.Usuarios.FindAsync(result.Id);
            Assert.NotNull(usuarioNoBanco);
            Assert.Equal(usuario.Nome, usuarioNoBanco.Nome);
            Assert.Equal(usuario.Email, usuarioNoBanco.Email);
        }

        [Fact]
        public async Task Inserir_UsuarioInvalido_DeveLancarApplicationException()
        {
            using var context = GetInMemoryDbContext();
            var usuarioService = new UsuarioService(context);

            var usuarioInvalido = new Usuario
            {
                Nome = null,  // Nome inválido
                Email = "emailinvalido@example.com",
                Senha = "Senha123",
                TipoUsuario = enTipoUsuario.Operador,
                Cargo = "Cargo",
                Telefone = "54 99800-5773"
            };

            usuarioService.Registrar(usuarioInvalido);

            var usuariosNoBanco = await context.Usuarios.ToListAsync();
            Assert.Empty(usuariosNoBanco);
        }

        [Fact]
        public async Task ProcurarPorId_UsuarioExiste_DeveRetornarUsuario()
        {
            using var context = GetInMemoryDbContext();
            var usuarioService = new UsuarioService(context);

            var usuarioExistente = new Usuario
            {
                Id = Guid.NewGuid(),
                Nome = "Maria Oliveira",
                Email = "maria.oliveira@example.com",
                Senha = "Senha123",
                TipoUsuario = enTipoUsuario.Operador,
                Cargo = "Cargo",
                Telefone = "54 99800-5773"
            };
            context.Usuarios.Add(usuarioExistente);
            await context.SaveChangesAsync();

            var result = await usuarioService.ProcurarPorId(usuarioExistente.Id);

            Assert.NotNull(result);
            Assert.Equal(usuarioExistente.Id, result.Id);
            Assert.Equal(usuarioExistente.Nome, result.Nome);
        }

        [Fact]
        public async Task ProcurarPorId_UsuarioNaoExiste_DeveRetornarNull()
        {
            using var context = GetInMemoryDbContext();
            var usuarioService = new UsuarioService(context);
            var idNaoExistente = Guid.NewGuid();

            var result = await usuarioService.ProcurarPorId(idNaoExistente);

            Assert.Null(result);
        }

        [Fact]
        public async Task Remover_UsuarioExistente_DeveRemoverUsuarioDoBanco()
        {
            using var context = GetInMemoryDbContext();
            var usuarioService = new UsuarioService(context);

            var usuarioParaRemover = new Usuario
            {
                Id = Guid.NewGuid(),
                Nome = "Lucas Costa",
                Email = "lucas.costa@example.com",
                Senha = "Senha456",
                TipoUsuario = enTipoUsuario.Operador,
                Cargo = "Cargo",
                Telefone = "54 99800-5773"
            };
            context.Usuarios.Add(usuarioParaRemover);
            await context.SaveChangesAsync();
            Assert.NotNull(await context.Usuarios.FindAsync(usuarioParaRemover.Id));

            await usuarioService.RemoverUsuario(usuarioParaRemover.Id);

            var usuarioRemovido = await context.Usuarios.FindAsync(usuarioParaRemover.Id);
            Assert.Null(usuarioRemovido);
        }

        [Fact]
        public async Task Alterar_UsuarioExistente_DeveAtualizarUsuarioNoBanco()
        {
            using var context = GetInMemoryDbContext();
            var usuarioService = new UsuarioService(context);

            var usuarioOriginal = new Usuario
            {
                Id = Guid.NewGuid(),
                Nome = "Rafael Souza",
                Email = "rafael.souza@example.com",
                Senha = "Senha789",
                TipoUsuario = enTipoUsuario.Administrador,
                Cargo = "Cargo",
                Telefone = "54 99800-5773"
            };
            context.Usuarios.Add(usuarioOriginal);
            await context.SaveChangesAsync();

            var usuarioAlterado = new Usuario
            {
                Id = usuarioOriginal.Id,
                Nome = "Rafael Souza Filho",
                Email = "rafael.souza.filho@example.com",
                Senha = "Senha456",
                TipoUsuario = enTipoUsuario.Operador,
                Cargo = "Cargo",
                Telefone = "54 99800-5773"
            };

            var result = await usuarioService.AtualizarUsuario(usuarioAlterado);

            Assert.NotNull(result);
            Assert.Equal(usuarioAlterado.Id, result.Id);
            Assert.Equal(usuarioAlterado.Nome, result.Nome);
            Assert.Equal(usuarioAlterado.Email, result.Email);

            var usuarioNoBanco = await context.Usuarios.AsNoTracking().FirstOrDefaultAsync(u => u.Id == usuarioAlterado.Id);
            Assert.NotNull(usuarioNoBanco);
            Assert.Equal("Rafael Souza Filho", usuarioNoBanco.Nome);
            Assert.Equal("rafael.souza.filho@example.com", usuarioNoBanco.Email);
        }
    }
}
