using Xunit;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using LaboratorioDeSoftware.Core.Data;
using LaboratorioDeSoftware.Core.Entities;
using static LaboratorioDeSoftware.Core.Entities.Enums.Enums; // Para enTipoProduto e enTipoUsuario

namespace LaboratorioDeSoftware.Tests.Core.Entities
{
    public class EquipamentoServiceIntegrationTests
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
        public async Task Equipamento_DeveSerAdicionadoAoBanco()
        {
            using var context = GetInMemoryDbContext();

            var produto = new Produto
            {
                Id = Guid.NewGuid(),
                Nome = "Produto Base XYZ",
                MarcaFabricante = "Marca ABC",
                Modelo = "Modelo 123",
                TipoProduto = enTipoProduto.Analogico,
                Observacoes = "Observações do Produto Base"
            };
            context.Produtos.Add(produto);
            await context.SaveChangesAsync();

            var usuarioResponsavel = new Usuario
            {
                Id = Guid.NewGuid(),
                Nome = "João Silva",
                Email = "joao.silva@empresa.com",
                Telefone = "51987654321",
                Cargo = "Coordenador de Laboratório",
                TipoUsuario = enTipoUsuario.Administrador,
                Senha = "senhaSegura123", // A propriedade Senha converte para Base64
                LaboratorioId = null // Pode ser nulo no momento da criação do usuário
            };
            context.Usuarios.Add(usuarioResponsavel);
            await context.SaveChangesAsync();

            var laboratorio = new Laboratorio
            {
                Id = Guid.NewGuid(),
                Nome = "Laboratório Central de Testes",
                Localizacao = "Prédio A, Sala 101",
                ResponsavelId = usuarioResponsavel.Id,
                Observacao = "Laboratório principal para testes de campo e bancada."
            };
            context.Laboratorios.Add(laboratorio);
            await context.SaveChangesAsync();


            var equipamento = new Equipamento
            {
                Id = Guid.NewGuid(),
                Nome = "Multímetro Digital Fluke 17B+",
                Numero_Serie = 1234567890123,
                Num_Patrimonio = 9876543210987,
                CACalibracao = "2024/05-BR-CAL-001",
                CAVerificacao = "2024/06-BR-VER-005",
                CapacidadeMedicao = "Tensão: 0-1000V, Corrente: 0-10A, Resistência: 0-40MΩ",
                PeriodicidadeCalibracao = 365,
                PeriodicidadeVerificacaoIntermediaria = 90,
                ResolucaoDivisaoEscala = "0.001V / 0.001A / 0.1Ω",
                ProdutoId = produto.Id,
                LaboratorioId = laboratorio.Id,
                DataColocacaoUso = new DateTime(2023, 1, 15),
                NumeroCertificadoCalibracao = 2024001,
                Disponivel = true
            };

            context.Equipamentos.Add(equipamento);
            await context.SaveChangesAsync();

            var equipamentoNoBanco = await context.Equipamentos
                                                  .Include(e => e.Produto)
                                                  .Include(e => e.Laboratorio)
                                                  .ThenInclude(l => l.Responsavel)
                                                  .FirstOrDefaultAsync(e => e.Id == equipamento.Id);

            Assert.NotNull(equipamentoNoBanco);
            Assert.Equal("Multímetro Digital Fluke 17B+", equipamentoNoBanco.Nome);
            Assert.Equal(1234567890123, equipamentoNoBanco.Numero_Serie);
            Assert.Equal(9876543210987, equipamentoNoBanco.Num_Patrimonio);
            Assert.Equal("2024/05-BR-CAL-001", equipamentoNoBanco.CACalibracao);
            Assert.Equal("2024/06-BR-VER-005", equipamentoNoBanco.CAVerificacao);
            Assert.Equal("Tensão: 0-1000V, Corrente: 0-10A, Resistência: 0-40MΩ", equipamentoNoBanco.CapacidadeMedicao);
            Assert.Equal(365, equipamentoNoBanco.PeriodicidadeCalibracao);
            Assert.Equal(90, equipamentoNoBanco.PeriodicidadeVerificacaoIntermediaria);
            Assert.Equal("0.001V / 0.001A / 0.1Ω", equipamentoNoBanco.ResolucaoDivisaoEscala);
            Assert.Equal(produto.Id, equipamentoNoBanco.ProdutoId);
            Assert.Equal(laboratorio.Id, equipamentoNoBanco.LaboratorioId);
            Assert.Equal(new DateTime(2023, 1, 15), equipamentoNoBanco.DataColocacaoUso);
            Assert.Equal(2024001, equipamentoNoBanco.NumeroCertificadoCalibracao);
            Assert.True(equipamentoNoBanco.Disponivel);

            Assert.Equal(produto.Nome, equipamentoNoBanco.Produto.Nome);
            Assert.Equal(laboratorio.Nome, equipamentoNoBanco.Laboratorio.Nome);
            Assert.Equal(usuarioResponsavel.Nome, equipamentoNoBanco.Laboratorio.Responsavel.Nome);
            Assert.Equal(usuarioResponsavel.Email, equipamentoNoBanco.Laboratorio.Responsavel.Email);
        }

        [Fact]
        public async Task Equipamento_DeveSerAtualizadoNoBanco()
        {
            using var context = GetInMemoryDbContext();

            var produto = new Produto
            {
                Id = Guid.NewGuid(),
                Nome = "Produto Base XYZ",
                MarcaFabricante = "Marca ABC",
                Modelo = "Modelo 123",
                TipoProduto = enTipoProduto.Analogico,
                Observacoes = "Observações do Produto Base"
            };
            context.Produtos.Add(produto);
            await context.SaveChangesAsync();

            var usuarioResponsavel = new Usuario
            {
                Id = Guid.NewGuid(),
                Nome = "João Silva",
                Email = "joao.silva@empresa.com",
                Telefone = "51987654321",
                Cargo = "Coordenador de Laboratório",
                TipoUsuario = enTipoUsuario.Administrador,
                Senha = "senhaSegura123",
                LaboratorioId = null
            };
            context.Usuarios.Add(usuarioResponsavel);
            await context.SaveChangesAsync();

            var laboratorio = new Laboratorio
            {
                Id = Guid.NewGuid(),
                Nome = "Laboratório Central de Testes",
                Localizacao = "Prédio A, Sala 101",
                ResponsavelId = usuarioResponsavel.Id,
                Observacao = "Laboratório principal para testes de campo e bancada."
            };
            context.Laboratorios.Add(laboratorio);
            await context.SaveChangesAsync();


            var equipamentoOriginal = new Equipamento
            {
                Id = Guid.NewGuid(),
                Nome = "Equipamento Antigo X",
                Numero_Serie = 1111111111111,
                Num_Patrimonio = 2222222222222,
                CACalibracao = "CA-001",
                CAVerificacao = "CV-001",
                CapacidadeMedicao = "1-100mA",
                PeriodicidadeCalibracao = 180,
                PeriodicidadeVerificacaoIntermediaria = 30,
                ResolucaoDivisaoEscala = "0.01mA",
                ProdutoId = produto.Id,
                LaboratorioId = laboratorio.Id,
                DataColocacaoUso = new DateTime(2022, 5, 10),
                NumeroCertificadoCalibracao = 1001,
                Disponivel = true
            };
            context.Equipamentos.Add(equipamentoOriginal);
            await context.SaveChangesAsync();

            equipamentoOriginal.Nome = "Equipamento Novo Nome Y";
            equipamentoOriginal.Numero_Serie = 9999999999999;
            equipamentoOriginal.Disponivel = false;
            equipamentoOriginal.PeriodicidadeCalibracao = 360;
            equipamentoOriginal.CAVerificacao = "CV-002-ATUALIZADO";

            context.Equipamentos.Update(equipamentoOriginal);
            await context.SaveChangesAsync();

            var equipamentoAtualizado = await context.Equipamentos.AsNoTracking().FirstOrDefaultAsync(e => e.Id == equipamentoOriginal.Id);

            Assert.NotNull(equipamentoAtualizado);
            Assert.Equal("Equipamento Novo Nome Y", equipamentoAtualizado.Nome);
            Assert.Equal(9999999999999, equipamentoAtualizado.Numero_Serie);
            Assert.False(equipamentoAtualizado.Disponivel);
            Assert.Equal(360, equipamentoAtualizado.PeriodicidadeCalibracao);
            Assert.Equal("CV-002-ATUALIZADO", equipamentoAtualizado.CAVerificacao);
        }

        [Fact]
        public async Task Equipamento_DeveSerRemovidoDoBanco()
        {
            using var context = GetInMemoryDbContext();

            var produto = new Produto
            {
                Id = Guid.NewGuid(),
                Nome = "Produto Base",
                MarcaFabricante = "Marca",
                Modelo = "Modelo",
                TipoProduto = enTipoProduto.Analogico,
                Observacoes = "Obs"
            };
            context.Produtos.Add(produto);
            await context.SaveChangesAsync();

            var usuarioResponsavel = new Usuario
            {
                Id = Guid.NewGuid(),
                Nome = "Responsavel Teste",
                Email = "responsavel@teste.com",
                Senha = "senha123",
                Telefone = "1111-2222",
                Cargo = "Gerente",
                TipoUsuario = enTipoUsuario.Administrador
            };
            context.Usuarios.Add(usuarioResponsavel);
            await context.SaveChangesAsync();

            var laboratorio = new Laboratorio
            {
                Id = Guid.NewGuid(),
                Nome = "Laboratorio Teste",
                Localizacao = "Sala 1",
                ResponsavelId = usuarioResponsavel.Id,
                Observacao = "Laboratorio para testes"
            };
            context.Laboratorios.Add(laboratorio);
            await context.SaveChangesAsync();

            var equipamentoParaRemover = new Equipamento
            {
                Id = Guid.NewGuid(),
                Nome = "Equipamento Para Remover",
                Numero_Serie = 123,
                Num_Patrimonio = 456,
                CACalibracao = "CA-REM",
                CAVerificacao = "CV-REM",
                CapacidadeMedicao = "Medida X",
                PeriodicidadeCalibracao = 60,
                PeriodicidadeVerificacaoIntermediaria = 15,
                ResolucaoDivisaoEscala = "Res X",
                ProdutoId = produto.Id,
                LaboratorioId = laboratorio.Id,
                DataColocacaoUso = DateTime.Now.AddYears(-1),
                NumeroCertificadoCalibracao = 777,
                Disponivel = true
            };
            context.Equipamentos.Add(equipamentoParaRemover);
            await context.SaveChangesAsync();

            Assert.NotNull(await context.Equipamentos.FindAsync(equipamentoParaRemover.Id));

            context.Equipamentos.Remove(equipamentoParaRemover);
            await context.SaveChangesAsync();

            var equipamentoRemovido = await context.Equipamentos.FindAsync(equipamentoParaRemover.Id);
            Assert.Null(equipamentoRemovido);
        }

        [Fact]
        public async Task Equipamento_DeveSerProcuradoPorId()
        {
            using var context = GetInMemoryDbContext();

            var produto = new Produto
            {
                Id = Guid.NewGuid(),
                Nome = "Produto Base",
                MarcaFabricante = "Marca",
                Modelo = "Modelo",
                TipoProduto = enTipoProduto.Analogico,
                Observacoes = "Obs"
            };
            context.Produtos.Add(produto);
            await context.SaveChangesAsync();

            var usuarioResponsavel = new Usuario
            {
                Id = Guid.NewGuid(),
                Nome = "Responsavel Teste",
                Email = "responsavel@teste.com",
                Senha = "senha123",
                Telefone = "1111-2222",
                Cargo = "Gerente",
                TipoUsuario = enTipoUsuario.Administrador
            };
            context.Usuarios.Add(usuarioResponsavel);
            await context.SaveChangesAsync();

            var laboratorio = new Laboratorio
            {
                Id = Guid.NewGuid(),
                Nome = "Laboratorio Teste",
                Localizacao = "Sala 1",
                ResponsavelId = usuarioResponsavel.Id,
                Observacao = "Laboratorio para testes"
            };
            context.Laboratorios.Add(laboratorio);
            await context.SaveChangesAsync();

            var equipamentoExistente = new Equipamento
            {
                Id = Guid.NewGuid(),
                Nome = "Equipamento Buscavel ABC",
                Numero_Serie = 7890123456789,
                Num_Patrimonio = 9870123456789,
                CACalibracao = "CA-BUSCA",
                CAVerificacao = "CV-BUSCA",
                CapacidadeMedicao = "Medicao Y",
                PeriodicidadeCalibracao = 90,
                PeriodicidadeVerificacaoIntermediaria = 30,
                ResolucaoDivisaoEscala = "Res Y",
                ProdutoId = produto.Id,
                LaboratorioId = laboratorio.Id,
                DataColocacaoUso = new DateTime(2024, 1, 1),
                NumeroCertificadoCalibracao = 2002,
                Disponivel = true
            };
            context.Equipamentos.Add(equipamentoExistente);
            await context.SaveChangesAsync();

            var equipamentoEncontrado = await context.Equipamentos.FindAsync(equipamentoExistente.Id);

            Assert.NotNull(equipamentoEncontrado);
            Assert.Equal(equipamentoExistente.Id, equipamentoEncontrado.Id);
            Assert.Equal("Equipamento Buscavel ABC", equipamentoEncontrado.Nome);
            Assert.Equal(7890123456789, equipamentoEncontrado.Numero_Serie);
            Assert.Equal(produto.Id, equipamentoEncontrado.ProdutoId);
            Assert.Equal(laboratorio.Id, equipamentoEncontrado.LaboratorioId);
        }

        [Fact]
        public async Task Equipamento_DeveRetornarNuloSeNaoEncontradoPorId()
        {
            using var context = GetInMemoryDbContext();
            var idNaoExistente = Guid.NewGuid();

            var equipamentoNaoEncontrado = await context.Equipamentos.FindAsync(idNaoExistente);

            Assert.Null(equipamentoNaoEncontrado);
        }

        [Fact]
        public async Task Equipamento_DeveRetornarTodosOsEquipamentos()
        {
            using var context = GetInMemoryDbContext();

            var produto1 = new Produto { Id = Guid.NewGuid(), Nome = "Produto Teste 1", MarcaFabricante = "M1", Modelo = "Mod1", TipoProduto = enTipoProduto.Analogico, Observacoes = "Obs1" };
            var produto2 = new Produto { Id = Guid.NewGuid(), Nome = "Produto Teste 2", MarcaFabricante = "M2", Modelo = "Mod2", TipoProduto = enTipoProduto.Digital, Observacoes = "Obs2" };
            context.Produtos.AddRange(produto1, produto2);
            await context.SaveChangesAsync();

            var usuarioResponsavel1 = new Usuario
            {
                Id = Guid.NewGuid(),
                Nome = "Responsavel Um",
                Email = "resp.um@teste.com",
                Telefone = "11111111111",
                Cargo = "Chefe",
                TipoUsuario = enTipoUsuario.Administrador,
                Senha = "senhaA",
                LaboratorioId = null
            };
            var usuarioResponsavel2 = new Usuario
            {
                Id = Guid.NewGuid(),
                Nome = "Responsavel Dois",
                Email = "resp.dois@teste.com",
                Telefone = "22222222222",
                Cargo = "Supervisor",
                TipoUsuario = enTipoUsuario.Responsavel,
                Senha = "senhaB",
                LaboratorioId = null
            };
            context.Usuarios.AddRange(usuarioResponsavel1, usuarioResponsavel2);
            await context.SaveChangesAsync();

            var laboratorio1 = new Laboratorio { Id = Guid.NewGuid(), Nome = "Laboratorio Alfa",  Localizacao = "Setor A", ResponsavelId = usuarioResponsavel1.Id, Observacao = "Lab de calibração" };
            var laboratorio2 = new Laboratorio { Id = Guid.NewGuid(), Nome = "Laboratorio Beta",  Localizacao = "Setor B", ResponsavelId = usuarioResponsavel2.Id, Observacao = "Lab de testes" };
            context.Laboratorios.AddRange(laboratorio1, laboratorio2);
            await context.SaveChangesAsync();

            var equipamento1 = new Equipamento
            {
                Id = Guid.NewGuid(),
                Nome = "Balanca de Precisao",
                Numero_Serie = 1000000000001,
                Num_Patrimonio = 1000000000002,
                CACalibracao = "CAL-BP-001",
                CAVerificacao = "VER-BP-001",
                CapacidadeMedicao = "0-500g",
                PeriodicidadeCalibracao = 365,
                PeriodicidadeVerificacaoIntermediaria = 60,
                ResolucaoDivisaoEscala = "0.001g",
                ProdutoId = produto1.Id,
                LaboratorioId = laboratorio1.Id,
                DataColocacaoUso = new DateTime(2023, 3, 1),
                NumeroCertificadoCalibracao = 100,
                Disponivel = true
            };
            var equipamento2 = new Equipamento
            {
                Id = Guid.NewGuid(),
                Nome = "Medidor de pH Digital",
                Numero_Serie = 2000000000001,
                Num_Patrimonio = 2000000000002,
                CACalibracao = "CAL-PH-002",
                CAVerificacao = "VER-PH-002",
                CapacidadeMedicao = "0-14 pH",
                PeriodicidadeCalibracao = 180,
                PeriodicidadeVerificacaoIntermediaria = 30,
                ResolucaoDivisaoEscala = "0.01 pH",
                ProdutoId = produto2.Id,
                LaboratorioId = laboratorio2.Id,
                DataColocacaoUso = new DateTime(2024, 2, 10),
                NumeroCertificadoCalibracao = 200,
                Disponivel = false
            };
            var equipamento3 = new Equipamento
            {
                Id = Guid.NewGuid(),
                Nome = "Termômetro Infravermelho",
                Numero_Serie = 3000000000001,
                Num_Patrimonio = 3000000000002,
                CACalibracao = "CAL-TI-003",
                CAVerificacao = "VER-TI-003",
                CapacidadeMedicao = "-50 a 500 °C",
                PeriodicidadeCalibracao = 365,
                PeriodicidadeVerificacaoIntermediaria = 90,
                ResolucaoDivisaoEscala = "0.1 °C",
                ProdutoId = produto1.Id,
                LaboratorioId = laboratorio1.Id,
                DataColocacaoUso = new DateTime(2023, 7, 20),
                NumeroCertificadoCalibracao = 300,
                Disponivel = true
            };

            context.Equipamentos.AddRange(equipamento1, equipamento2, equipamento3);
            await context.SaveChangesAsync();

            var todosEquipamentos = await context.Equipamentos.ToListAsync();

            Assert.NotNull(todosEquipamentos);
            Assert.Equal(3, todosEquipamentos.Count);
            Assert.Contains(todosEquipamentos, e => e.Nome == "Balanca de Precisao");
            Assert.Contains(todosEquipamentos, e => e.Nome == "Medidor de pH Digital");
            Assert.Contains(todosEquipamentos, e => e.Nome == "Termômetro Infravermelho");
        }
    }
}