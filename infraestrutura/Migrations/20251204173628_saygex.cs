using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Locadora.Infraestrutura.Migrations
{
    /// <inheritdoc />
    public partial class saygex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_aluguel_funcionarios_FuncionarioId",
                table: "aluguel");

            migrationBuilder.DropColumn(
                name: "DataDevolucao",
                table: "aluguel");

            migrationBuilder.DropColumn(
                name: "KmDevolucao",
                table: "aluguel");

            migrationBuilder.DropColumn(
                name: "LitrosNaChegada",
                table: "aluguel");

            migrationBuilder.DropColumn(
                name: "SeguroAcionado",
                table: "aluguel");

            migrationBuilder.DropColumn(
                name: "TanqueCheio",
                table: "aluguel");

            migrationBuilder.DropColumn(
                name: "Total",
                table: "aluguel");

            migrationBuilder.AlterColumn<Guid>(
                name: "FuncionarioId",
                table: "aluguel",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.CreateTable(
                name: "devolucoes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    DataDevolucao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    KmDevolucao = table.Column<float>(type: "real", nullable: false),
                    LitrosNaChegada = table.Column<int>(type: "integer", nullable: false),
                    SeguroAcionado = table.Column<bool>(type: "boolean", nullable: false),
                    TanqueCheio = table.Column<bool>(type: "boolean", nullable: false),
                    Total = table.Column<decimal>(type: "numeric", nullable: false),
                    AluguelId = table.Column<Guid>(type: "uuid", nullable: false),
                    EmpresaId = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_devolucoes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_devolucoes_AspNetUsers_TenantId",
                        column: x => x.TenantId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_devolucoes_aluguel_AluguelId",
                        column: x => x.AluguelId,
                        principalTable: "aluguel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_devolucoes_AluguelId",
                table: "devolucoes",
                column: "AluguelId");

            migrationBuilder.CreateIndex(
                name: "IX_devolucoes_TenantId",
                table: "devolucoes",
                column: "TenantId");

            migrationBuilder.AddForeignKey(
                name: "FK_aluguel_funcionarios_FuncionarioId",
                table: "aluguel",
                column: "FuncionarioId",
                principalTable: "funcionarios",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_aluguel_funcionarios_FuncionarioId",
                table: "aluguel");

            migrationBuilder.DropTable(
                name: "devolucoes");

            migrationBuilder.AlterColumn<Guid>(
                name: "FuncionarioId",
                table: "aluguel",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DataDevolucao",
                table: "aluguel",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "KmDevolucao",
                table: "aluguel",
                type: "real",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LitrosNaChegada",
                table: "aluguel",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "SeguroAcionado",
                table: "aluguel",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "TanqueCheio",
                table: "aluguel",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<decimal>(
                name: "Total",
                table: "aluguel",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddForeignKey(
                name: "FK_aluguel_funcionarios_FuncionarioId",
                table: "aluguel",
                column: "FuncionarioId",
                principalTable: "funcionarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
