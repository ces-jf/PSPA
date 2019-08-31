using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApi.Migrations
{
    public partial class v6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UrlOrigem",
                table: "PedidoImportacao",
                newName: "PastaTemp");

            migrationBuilder.AddColumn<string>(
                name: "UrlOrigem",
                table: "ArquivoBase",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UrlOrigem",
                table: "ArquivoBase");

            migrationBuilder.RenameColumn(
                name: "PastaTemp",
                table: "PedidoImportacao",
                newName: "UrlOrigem");
        }
    }
}
