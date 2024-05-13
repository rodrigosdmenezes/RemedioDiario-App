using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RemedioDiario.Migrations
{
    /// <inheritdoc />
    public partial class RefazendoObjetoMedicamento : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Comprimido",
                table: "MedicamentosApp");

            migrationBuilder.DropColumn(
                name: "Gotas",
                table: "MedicamentosApp");

            migrationBuilder.DropColumn(
                name: "HoraTomar",
                table: "MedicamentosApp");

            migrationBuilder.AddColumn<string>(
                name: "Data",
                table: "MedicamentosApp",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Hora",
                table: "MedicamentosApp",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Quantidade",
                table: "MedicamentosApp",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Tipo",
                table: "MedicamentosApp",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Data",
                table: "MedicamentosApp");

            migrationBuilder.DropColumn(
                name: "Hora",
                table: "MedicamentosApp");

            migrationBuilder.DropColumn(
                name: "Quantidade",
                table: "MedicamentosApp");

            migrationBuilder.DropColumn(
                name: "Tipo",
                table: "MedicamentosApp");

            migrationBuilder.AddColumn<bool>(
                name: "Comprimido",
                table: "MedicamentosApp",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Gotas",
                table: "MedicamentosApp",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "HoraTomar",
                table: "MedicamentosApp",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
