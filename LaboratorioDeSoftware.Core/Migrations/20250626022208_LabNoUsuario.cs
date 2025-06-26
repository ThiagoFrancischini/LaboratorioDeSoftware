using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LaboratorioDeSoftware.Core.Migrations
{
    /// <inheritdoc />
    public partial class LabNoUsuario : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "LaboratorioId",
                table: "Usuarios",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_LaboratorioId",
                table: "Usuarios",
                column: "LaboratorioId");

            migrationBuilder.AddForeignKey(
                name: "FK_Usuarios_Laboratorios_LaboratorioId",
                table: "Usuarios",
                column: "LaboratorioId",
                principalTable: "Laboratorios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Usuarios_Laboratorios_LaboratorioId",
                table: "Usuarios");

            migrationBuilder.DropIndex(
                name: "IX_Usuarios_LaboratorioId",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "LaboratorioId",
                table: "Usuarios");
        }
    }
}
