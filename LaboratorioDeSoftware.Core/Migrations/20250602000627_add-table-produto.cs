using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LaboratorioDeSoftware.Core.Migrations
{
    /// <inheritdoc />
    public partial class addtableproduto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Produtos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Nome = table.Column<string>(type: "text", nullable: false),
                    MarcaFabricante = table.Column<string>(type: "text", nullable: false),
                    Modelo = table.Column<string>(type: "text", nullable: false),
                    CACalibracao = table.Column<string>(type: "text", nullable: false),
                    CAVerificacoes = table.Column<string>(type: "text", nullable: false),
                    CapaciadadeMedicao = table.Column<string>(type: "text", nullable: false),
                    PeriodicidadeCalibracao = table.Column<int>(type: "integer", nullable: false),
                    PeriodicidadeVerificacaoIntermediaria = table.Column<int>(type: "integer", nullable: false),
                    ResolucaoDivisaoEscala = table.Column<string>(type: "text", nullable: false),
                    TipoProduto = table.Column<int>(type: "integer", nullable: false),
                    Observacoes = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Produtos", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Produtos");
        }
    }
}
