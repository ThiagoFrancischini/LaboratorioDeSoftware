using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LaboratorioDeSoftware.Core.Migrations
{
    /// <inheritdoc />
    public partial class addtablecalibracoes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Nome",
                table: "Equipamentos",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Calibracoes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    EquipamentoId = table.Column<Guid>(type: "uuid", nullable: false),
                    GrandezaParametro = table.Column<string>(type: "text", nullable: false),
                    DataCalibracao = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    DataAcompanhamento = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Erro = table.Column<double>(type: "double precision", nullable: false),
                    Incerteza = table.Column<double>(type: "double precision", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    Observacoes = table.Column<string>(type: "text", nullable: false),
                    CS = table.Column<bool>(type: "boolean", nullable: false),
                    NumeroSolicitacao = table.Column<long>(type: "bigint", nullable: false),
                    DataSolicitacao = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Custo = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Calibracoes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Calibracoes_Equipamentos_EquipamentoId",
                        column: x => x.EquipamentoId,
                        principalTable: "Equipamentos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Calibracoes_EquipamentoId",
                table: "Calibracoes",
                column: "EquipamentoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Calibracoes");

            migrationBuilder.DropColumn(
                name: "Nome",
                table: "Equipamentos");
        }
    }
}
