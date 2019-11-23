using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApi.Migrations
{
    public partial class v12 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Cabecalho");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cabecalho",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ArquivoBaseID = table.Column<long>(nullable: true),
                    Descricao = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cabecalho", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Cabecalho_arquivo_base_ArquivoBaseID",
                        column: x => x.ArquivoBaseID,
                        principalSchema: "PSPABase",
                        principalTable: "arquivo_base",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cabecalho_ArquivoBaseID",
                table: "Cabecalho",
                column: "ArquivoBaseID");
        }
    }
}
