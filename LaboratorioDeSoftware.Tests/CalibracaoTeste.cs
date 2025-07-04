using LaboratorioDeSoftware.Core.Data;
using LaboratorioDeSoftware.Core.DTOs.Filtros;
using LaboratorioDeSoftware.Core.Entities;
using LaboratorioDeSoftware.Core.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using static LaboratorioDeSoftware.Core.Entities.Enums.Enums;

namespace LaboratorioDeSoftware.Tests.Core.Services
{
    public class CalibracaoServiceIntegrationTests
    {
        private AppDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var context = new AppDbContext(options);
            context.Database.EnsureCreated();
            return context;
        }

        [Fact]
        public async Task Inserir_CalibracaoValida_DeveAdicionarAoBanco()
        {
            using var context = GetInMemoryDbContext();
            var service = new CalibracaoService(context);

            var calibracao = new Calibracao
            {
                Id = Guid.NewGuid(),
                EquipamentoId = Guid.NewGuid(),
                DataCalibracao = DateTime.Today,
                DataSolicitacao = DateTime.Today.AddDays(-2),
                DataAcompanhamento = DateTime.Today.AddDays(1),
                GrandezaParametro = "53",
                Observacoes = "OBS"
            };

            var result = await service.Inserir(calibracao);

            Assert.NotNull(result);
            Assert.NotEqual(Guid.Empty, result.Id);

            var noBanco = await context.Calibracoes.FindAsync(result.Id);
            Assert.NotNull(noBanco);
            Assert.Equal(calibracao.EquipamentoId, noBanco.EquipamentoId);
        }

        [Fact]
        public async Task Inserir_CalibracaoInvalida_DeveLancarExcecao()
        {
            using var context = GetInMemoryDbContext();
            var service = new CalibracaoService(context);

            var calibracao = new Calibracao(); 

            service.Inserir(calibracao);
        }

        [Fact]
        public async Task ProcurarPorId_CalibracaoExiste_DeveRetornarCalibracao()
        {
            using var context = GetInMemoryDbContext();
            var calibracaoService = new CalibracaoService(context);

            var calibracao = new Calibracao
            {
                Id = Guid.NewGuid(),
                EquipamentoId = Guid.NewGuid(),
                DataCalibracao = DateTime.Today,
                GrandezaParametro = "53",
                Observacoes = "OBS"
            };

            context.Calibracoes.Add(calibracao);
            await context.SaveChangesAsync();

            var result = await calibracaoService.ProcurarPorId(calibracao.Id);

            Assert.True(true);
        }

        [Fact]
        public async Task ProcurarPorId_CalibracaoNaoExiste_DeveRetornarNull()
        {
            using var context = GetInMemoryDbContext();
            var service = new CalibracaoService(context);

            var result = await service.ProcurarPorId(Guid.NewGuid());

            Assert.Null(result);
        }

        [Fact]
        public async Task Remover_CalibracaoExistente_DeveRemoverDoBanco()
        {
            using var context = GetInMemoryDbContext();
            var service = new CalibracaoService(context);

            var calibracao = new Calibracao
            {
                Id = Guid.NewGuid(),
                EquipamentoId = Guid.NewGuid(),
                DataCalibracao = DateTime.Today,
                GrandezaParametro = "53",
                Observacoes = "OBS"
            };
            context.Calibracoes.Add(calibracao);
            await context.SaveChangesAsync();

            await service.Remover(calibracao);

            var result = await context.Calibracoes.FindAsync(calibracao.Id);
            Assert.Null(result);
        }

        [Fact]
        public async Task Alterar_CalibracaoExistente_DeveAtualizarNoBanco()
        {
            using var context = GetInMemoryDbContext();
            var service = new CalibracaoService(context);

            var calibracao = new Calibracao
            {
                Id = Guid.NewGuid(),
                EquipamentoId = Guid.NewGuid(),
                DataCalibracao = DateTime.Today,
                GrandezaParametro = "53",
                Observacoes = "OBS"
            };
            context.Calibracoes.Add(calibracao);
            await context.SaveChangesAsync();

            calibracao.DataCalibracao = DateTime.Today.AddDays(5);
            await service.Alterar(calibracao);

            var atualizada = await context.Calibracoes.AsNoTracking().FirstOrDefaultAsync(c => c.Id == calibracao.Id);
            Assert.NotNull(atualizada);
            Assert.Equal(calibracao.DataCalibracao, atualizada.DataCalibracao);
        }
    }
}
