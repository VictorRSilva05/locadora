using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Locadora.Infraestrutura.Migrations
{
    /// <inheritdoc />
    public partial class kskdsk : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "Kilometragem",
                table: "veiculos",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<string>(
                name: "Placa",
                table: "veiculos",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "TipoCambio",
                table: "veiculos",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "CNH",
                table: "clientes",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "PJId",
                table: "clientes",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RG",
                table: "clientes",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_clientes_PJId",
                table: "clientes",
                column: "PJId");

            migrationBuilder.AddForeignKey(
                name: "FK_clientes_clientes_PJId",
                table: "clientes",
                column: "PJId",
                principalTable: "clientes",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_clientes_clientes_PJId",
                table: "clientes");

            migrationBuilder.DropIndex(
                name: "IX_clientes_PJId",
                table: "clientes");

            migrationBuilder.DropColumn(
                name: "Kilometragem",
                table: "veiculos");

            migrationBuilder.DropColumn(
                name: "Placa",
                table: "veiculos");

            migrationBuilder.DropColumn(
                name: "TipoCambio",
                table: "veiculos");

            migrationBuilder.DropColumn(
                name: "CNH",
                table: "clientes");

            migrationBuilder.DropColumn(
                name: "PJId",
                table: "clientes");

            migrationBuilder.DropColumn(
                name: "RG",
                table: "clientes");
        }
    }
}
