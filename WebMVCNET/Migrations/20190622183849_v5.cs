using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApi.Migrations
{
    public partial class v5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UrlOrigem",
                table: "PedidoImportacao",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "PedidoImportacaoID",
                table: "ArquivoBase",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ArquivoBase_PedidoImportacaoID",
                table: "ArquivoBase",
                column: "PedidoImportacaoID");

            migrationBuilder.AddForeignKey(
                name: "FK_ArquivoBase_PedidoImportacao_PedidoImportacaoID",
                table: "ArquivoBase",
                column: "PedidoImportacaoID",
                principalTable: "PedidoImportacao",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ArquivoBase_PedidoImportacao_PedidoImportacaoID",
                table: "ArquivoBase");

            migrationBuilder.DropIndex(
                name: "IX_ArquivoBase_PedidoImportacaoID",
                table: "ArquivoBase");

            migrationBuilder.DropColumn(
                name: "UrlOrigem",
                table: "PedidoImportacao");

            migrationBuilder.DropColumn(
                name: "PedidoImportacaoID",
                table: "ArquivoBase");
        }
    }
}
