using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LaboratorioDeSoftware.Core.Migrations
{
    /// <inheritdoc />
    public partial class Equips : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Equipamentos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Numero_Serie = table.Column<long>(type: "bigint", nullable: false),
                    Num_Patrimonio = table.Column<long>(type: "bigint", nullable: false),
                    CACalibracao = table.Column<string>(type: "text", nullable: false),
                    CAVerificacao = table.Column<string>(type: "text", nullable: false),
                    CapacidadeMedicao = table.Column<string>(type: "text", nullable: false),
                    PeriodicidadeCalibracao = table.Column<int>(type: "integer", nullable: false),
                    PeriodicidadeVerificacaoIntermediaria = table.Column<int>(type: "integer", nullable: false),
                    ResolucaoDivisaoEscala = table.Column<string>(type: "text", nullable: false),
                    ProdutoId = table.Column<Guid>(type: "uuid", nullable: false),
                    LaboratorioId = table.Column<Guid>(type: "uuid", nullable: false),
                    DataColocacaoUso = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    NumeroCertificadoCalibracao = table.Column<long>(type: "bigint", nullable: false),
                    Disponivel = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Equipamentos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Equipamentos_Laboratorios_LaboratorioId",
                        column: x => x.LaboratorioId,
                        principalTable: "Laboratorios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Equipamentos_Produtos_ProdutoId",
                        column: x => x.ProdutoId,
                        principalTable: "Produtos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Equipamentos_LaboratorioId",
                table: "Equipamentos",
                column: "LaboratorioId");

            migrationBuilder.CreateIndex(
                name: "IX_Equipamentos_ProdutoId",
                table: "Equipamentos",
                column: "ProdutoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Equipamentos");
        }
    }
}
