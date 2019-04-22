using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApi.Migrations
{
    public partial class v10 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Indice",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Indice", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Usuario",
                columns: table => new
                {
                    Matricula = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nome = table.Column<string>(nullable: false),
                    SobreNome = table.Column<string>(nullable: false),
                    Email = table.Column<string>(nullable: false),
                    Senha = table.Column<string>(nullable: false),
                    UsuarioMatricula = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuario", x => x.Matricula);
                    table.ForeignKey(
                        name: "FK_Usuario_Usuario_UsuarioMatricula",
                        column: x => x.UsuarioMatricula,
                        principalTable: "Usuario",
                        principalColumn: "Matricula",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ArquivoBase",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IndexID = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArquivoBase", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ArquivoBase_Indice_IndexID",
                        column: x => x.IndexID,
                        principalTable: "Indice",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PedidoImportacao",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    DataTermino = table.Column<DateTime>(nullable: false),
                    UsuarioMatricula = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PedidoImportacao", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PedidoImportacao_Usuario_UsuarioMatricula",
                        column: x => x.UsuarioMatricula,
                        principalTable: "Usuario",
                        principalColumn: "Matricula",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Cabecalho",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Descricao = table.Column<string>(nullable: false),
                    ArquivoBaseID = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cabecalho", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Cabecalho_ArquivoBase_ArquivoBaseID",
                        column: x => x.ArquivoBaseID,
                        principalTable: "ArquivoBase",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LinhaPedidoImportacao",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    EstaFeito = table.Column<string>(nullable: true),
                    ArquivoBaseID = table.Column<long>(nullable: true),
                    PedidoImportacaoID = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LinhaPedidoImportacao", x => x.ID);
                    table.ForeignKey(
                        name: "FK_LinhaPedidoImportacao_ArquivoBase_ArquivoBaseID",
                        column: x => x.ArquivoBaseID,
                        principalTable: "ArquivoBase",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LinhaPedidoImportacao_PedidoImportacao_PedidoImportacaoID",
                        column: x => x.PedidoImportacaoID,
                        principalTable: "PedidoImportacao",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ArquivoBase_IndexID",
                table: "ArquivoBase",
                column: "IndexID");

            migrationBuilder.CreateIndex(
                name: "IX_Cabecalho_ArquivoBaseID",
                table: "Cabecalho",
                column: "ArquivoBaseID");

            migrationBuilder.CreateIndex(
                name: "IX_LinhaPedidoImportacao_ArquivoBaseID",
                table: "LinhaPedidoImportacao",
                column: "ArquivoBaseID");

            migrationBuilder.CreateIndex(
                name: "IX_LinhaPedidoImportacao_PedidoImportacaoID",
                table: "LinhaPedidoImportacao",
                column: "PedidoImportacaoID");

            migrationBuilder.CreateIndex(
                name: "IX_PedidoImportacao_UsuarioMatricula",
                table: "PedidoImportacao",
                column: "UsuarioMatricula");

            migrationBuilder.CreateIndex(
                name: "IX_Usuario_UsuarioMatricula",
                table: "Usuario",
                column: "UsuarioMatricula");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Cabecalho");

            migrationBuilder.DropTable(
                name: "LinhaPedidoImportacao");

            migrationBuilder.DropTable(
                name: "ArquivoBase");

            migrationBuilder.DropTable(
                name: "PedidoImportacao");

            migrationBuilder.DropTable(
                name: "Indice");

            migrationBuilder.DropTable(
                name: "Usuario");
        }
    }
}
