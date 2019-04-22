using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApi.Migrations
{
    public partial class v4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LogPedidoImportacao",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Descricao = table.Column<string>(nullable: false),
                    IndicadorStatus = table.Column<string>(maxLength: 1, nullable: false),
                    PedidoImportacaoID = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LogPedidoImportacao", x => x.ID);
                    table.ForeignKey(
                        name: "FK_LogPedidoImportacao_PedidoImportacao_PedidoImportacaoID",
                        column: x => x.PedidoImportacaoID,
                        principalTable: "PedidoImportacao",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LogPedidoImportacao_PedidoImportacaoID",
                table: "LogPedidoImportacao",
                column: "PedidoImportacaoID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LogPedidoImportacao");
        }
    }
}
