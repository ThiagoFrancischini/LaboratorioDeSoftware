using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LaboratorioDeSoftware.Core.Migrations
{
    /// <inheritdoc />
    public partial class Manutencoes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ManutencoesCorretivas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    EquipamentoId = table.Column<Guid>(type: "uuid", nullable: false),
                    DataProblemaApresentado = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    ProblemaApresentado = table.Column<string>(type: "text", nullable: false),
                    DataRetorno = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    EstadoRetorno = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ManutencoesCorretivas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ManutencoesCorretivas_Equipamentos_EquipamentoId",
                        column: x => x.EquipamentoId,
                        principalTable: "Equipamentos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ManutencoesCorretivas_EquipamentoId",
                table: "ManutencoesCorretivas",
                column: "EquipamentoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ManutencoesCorretivas");
        }
    }
}
