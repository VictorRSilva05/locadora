using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Locadora.Infraestrutura.Migrations
{
    /// <inheritdoc />
    public partial class asdasd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "clientes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Nome = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Telefone = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: false),
                    TipoCliente = table.Column<int>(type: "integer", nullable: false),
                    CPF = table.Column<string>(type: "character varying(14)", maxLength: 14, nullable: true),
                    CNPJ = table.Column<string>(type: "character varying(18)", maxLength: 18, nullable: true),
                    Estado = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Cidade = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Bairro = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Rua = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    Numero = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    EmpresaId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_clientes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "combustivels",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Nome = table.Column<string>(type: "text", nullable: false),
                    Preco = table.Column<decimal>(type: "numeric", nullable: false),
                    EmpresaId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_combustivels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "condutores",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Nome = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    Telefone = table.Column<string>(type: "text", nullable: false),
                    Cpf = table.Column<string>(type: "text", nullable: false),
                    Cnh = table.Column<string>(type: "text", nullable: false),
                    Validade = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EmpresaId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_condutores", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "funcionarios",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Nome = table.Column<string>(type: "text", nullable: false),
                    DataAdmissao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Salario = table.Column<decimal>(type: "numeric", nullable: false),
                    EmpresaId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_funcionarios", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "grupoVeiculos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Nome = table.Column<string>(type: "text", nullable: false),
                    EmpresaId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_grupoVeiculos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "cobrancas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    GrupoVeiculoId = table.Column<Guid>(type: "uuid", nullable: false),
                    PlanoCobranca = table.Column<int>(type: "integer", nullable: false),
                    PrecoDiaria = table.Column<decimal>(type: "numeric", nullable: true),
                    PrecoKm = table.Column<decimal>(type: "numeric", nullable: true),
                    KmDisponiveis = table.Column<int>(type: "integer", nullable: true),
                    PrecoPorKmExtrapolado = table.Column<decimal>(type: "numeric", nullable: true),
                    Taxa = table.Column<decimal>(type: "numeric", nullable: true),
                    EmpresaId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cobrancas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_cobrancas_grupoVeiculos_GrupoVeiculoId",
                        column: x => x.GrupoVeiculoId,
                        principalTable: "grupoVeiculos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "veiculos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Foto = table.Column<byte[]>(type: "bytea", nullable: true),
                    GrupoVeiculoId = table.Column<Guid>(type: "uuid", nullable: false),
                    Marca = table.Column<string>(type: "text", nullable: false),
                    Cor = table.Column<string>(type: "text", nullable: false),
                    Modelo = table.Column<string>(type: "text", nullable: false),
                    CombustivelId = table.Column<Guid>(type: "uuid", nullable: false),
                    CapacidadeCombustivel = table.Column<int>(type: "integer", nullable: false),
                    Ano = table.Column<int>(type: "integer", nullable: false),
                    EmpresaId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_veiculos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_veiculos_combustivels_CombustivelId",
                        column: x => x.CombustivelId,
                        principalTable: "combustivels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_veiculos_grupoVeiculos_GrupoVeiculoId",
                        column: x => x.GrupoVeiculoId,
                        principalTable: "grupoVeiculos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "aluguel",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CondutorId = table.Column<Guid>(type: "uuid", nullable: true),
                    ClienteId = table.Column<Guid>(type: "uuid", nullable: true),
                    CobrancaId = table.Column<Guid>(type: "uuid", nullable: false),
                    Caucao = table.Column<decimal>(type: "numeric", nullable: false),
                    VeiculoId = table.Column<Guid>(type: "uuid", nullable: false),
                    DataSaida = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DataRetornoPrevista = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DataDevolucao = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    KmInicial = table.Column<float>(type: "real", nullable: false),
                    KmDevolucao = table.Column<float>(type: "real", nullable: true),
                    TanqueCheio = table.Column<bool>(type: "boolean", nullable: true),
                    Status = table.Column<bool>(type: "boolean", nullable: false),
                    SeguroAcionado = table.Column<bool>(type: "boolean", nullable: false),
                    Total = table.Column<decimal>(type: "numeric", nullable: false),
                    EmpresaId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_aluguel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_aluguel_clientes_ClienteId",
                        column: x => x.ClienteId,
                        principalTable: "clientes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_aluguel_cobrancas_CobrancaId",
                        column: x => x.CobrancaId,
                        principalTable: "cobrancas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_aluguel_condutores_CondutorId",
                        column: x => x.CondutorId,
                        principalTable: "condutores",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_aluguel_veiculos_VeiculoId",
                        column: x => x.VeiculoId,
                        principalTable: "veiculos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "taxas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Descricao = table.Column<string>(type: "text", nullable: false),
                    Valor = table.Column<decimal>(type: "numeric", nullable: false),
                    PlanoCobranca = table.Column<int>(type: "integer", nullable: false),
                    AluguelId = table.Column<Guid>(type: "uuid", nullable: true),
                    EmpresaId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_taxas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_taxas_aluguel_AluguelId",
                        column: x => x.AluguelId,
                        principalTable: "aluguel",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_aluguel_ClienteId",
                table: "aluguel",
                column: "ClienteId");

            migrationBuilder.CreateIndex(
                name: "IX_aluguel_CobrancaId",
                table: "aluguel",
                column: "CobrancaId");

            migrationBuilder.CreateIndex(
                name: "IX_aluguel_CondutorId",
                table: "aluguel",
                column: "CondutorId");

            migrationBuilder.CreateIndex(
                name: "IX_aluguel_VeiculoId",
                table: "aluguel",
                column: "VeiculoId");

            migrationBuilder.CreateIndex(
                name: "IX_cobrancas_GrupoVeiculoId",
                table: "cobrancas",
                column: "GrupoVeiculoId");

            migrationBuilder.CreateIndex(
                name: "IX_taxas_AluguelId",
                table: "taxas",
                column: "AluguelId");

            migrationBuilder.CreateIndex(
                name: "IX_veiculos_CombustivelId",
                table: "veiculos",
                column: "CombustivelId");

            migrationBuilder.CreateIndex(
                name: "IX_veiculos_GrupoVeiculoId",
                table: "veiculos",
                column: "GrupoVeiculoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "funcionarios");

            migrationBuilder.DropTable(
                name: "taxas");

            migrationBuilder.DropTable(
                name: "aluguel");

            migrationBuilder.DropTable(
                name: "clientes");

            migrationBuilder.DropTable(
                name: "cobrancas");

            migrationBuilder.DropTable(
                name: "condutores");

            migrationBuilder.DropTable(
                name: "veiculos");

            migrationBuilder.DropTable(
                name: "combustivels");

            migrationBuilder.DropTable(
                name: "grupoVeiculos");
        }
    }
}
