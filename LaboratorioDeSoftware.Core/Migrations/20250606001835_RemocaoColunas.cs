using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LaboratorioDeSoftware.Core.Migrations
{
    /// <inheritdoc />
    public partial class RemocaoColunas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CACalibracao",
                table: "Produtos");

            migrationBuilder.DropColumn(
                name: "CAVerificacoes",
                table: "Produtos");

            migrationBuilder.DropColumn(
                name: "CapaciadadeMedicao",
                table: "Produtos");

            migrationBuilder.DropColumn(
                name: "PeriodicidadeCalibracao",
                table: "Produtos");

            migrationBuilder.DropColumn(
                name: "PeriodicidadeVerificacaoIntermediaria",
                table: "Produtos");

            migrationBuilder.DropColumn(
                name: "ResolucaoDivisaoEscala",
                table: "Produtos");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CACalibracao",
                table: "Produtos",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CAVerificacoes",
                table: "Produtos",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CapaciadadeMedicao",
                table: "Produtos",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "PeriodicidadeCalibracao",
                table: "Produtos",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PeriodicidadeVerificacaoIntermediaria",
                table: "Produtos",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ResolucaoDivisaoEscala",
                table: "Produtos",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
