using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApi.Migrations
{
    public partial class v15 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Estado",
                schema: "PSPABase",
                table: "PedidoImportacao");

            migrationBuilder.AddColumn<int>(
                name: "OrderState",
                schema: "PSPABase",
                table: "PedidoImportacao",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrderState",
                schema: "PSPABase",
                table: "PedidoImportacao");

            migrationBuilder.AddColumn<string>(
                name: "Estado",
                schema: "PSPABase",
                table: "PedidoImportacao",
                maxLength: 1,
                nullable: true);
        }
    }
}
