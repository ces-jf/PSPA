using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApi.Migrations
{
    public partial class v7 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PedidoImportacao_Usuario_UsuarioRegisterNumber",
                table: "PedidoImportacao");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Usuario",
                table: "Usuario");

            migrationBuilder.DropIndex(
                name: "IX_PedidoImportacao_UsuarioRegisterNumber",
                table: "PedidoImportacao");

            migrationBuilder.DropColumn(
                name: "RegisterNumber",
                table: "Usuario");

            migrationBuilder.DropColumn(
                name: "UsuarioRegisterNumber",
                table: "PedidoImportacao");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "Usuario",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UsuarioId",
                table: "PedidoImportacao",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Usuario",
                table: "Usuario",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_PedidoImportacao_UsuarioId",
                table: "PedidoImportacao",
                column: "UsuarioId");

            migrationBuilder.AddForeignKey(
                name: "FK_PedidoImportacao_Usuario_UsuarioId",
                table: "PedidoImportacao",
                column: "UsuarioId",
                principalTable: "Usuario",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PedidoImportacao_Usuario_UsuarioId",
                table: "PedidoImportacao");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Usuario",
                table: "Usuario");

            migrationBuilder.DropIndex(
                name: "IX_PedidoImportacao_UsuarioId",
                table: "PedidoImportacao");

            migrationBuilder.DropColumn(
                name: "UsuarioId",
                table: "PedidoImportacao");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "Usuario",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<long>(
                name: "RegisterNumber",
                table: "Usuario",
                nullable: false,
                defaultValue: 0L)
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddColumn<long>(
                name: "UsuarioRegisterNumber",
                table: "PedidoImportacao",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Usuario",
                table: "Usuario",
                column: "RegisterNumber");

            migrationBuilder.CreateIndex(
                name: "IX_PedidoImportacao_UsuarioRegisterNumber",
                table: "PedidoImportacao",
                column: "UsuarioRegisterNumber");

            migrationBuilder.AddForeignKey(
                name: "FK_PedidoImportacao_Usuario_UsuarioRegisterNumber",
                table: "PedidoImportacao",
                column: "UsuarioRegisterNumber",
                principalTable: "Usuario",
                principalColumn: "RegisterNumber",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
