using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApi.Migrations
{
    public partial class v9 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                table: "AspNetRoleClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                table: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "LinhaPedidoImportacao");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AspNetRoles",
                table: "AspNetRoles");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "AspNetRoles");

            migrationBuilder.RenameTable(
                name: "AspNetRoles",
                newName: "role",
                newSchema: "PSPABase");

            migrationBuilder.AddPrimaryKey(
                name: "PK_role",
                schema: "PSPABase",
                table: "role",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetRoleClaims_role_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId",
                principalSchema: "PSPABase",
                principalTable: "role",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserRoles_role_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId",
                principalSchema: "PSPABase",
                principalTable: "role",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetRoleClaims_role_RoleId",
                table: "AspNetRoleClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserRoles_role_RoleId",
                table: "AspNetUserRoles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_role",
                schema: "PSPABase",
                table: "role");

            migrationBuilder.RenameTable(
                name: "role",
                schema: "PSPABase",
                newName: "AspNetRoles");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "AspNetRoles",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AspNetRoles",
                table: "AspNetRoles",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "LinhaPedidoImportacao",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ArquivoBaseID = table.Column<long>(nullable: true),
                    EstaFeito = table.Column<string>(nullable: true),
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
                name: "IX_LinhaPedidoImportacao_ArquivoBaseID",
                table: "LinhaPedidoImportacao",
                column: "ArquivoBaseID");

            migrationBuilder.CreateIndex(
                name: "IX_LinhaPedidoImportacao_PedidoImportacaoID",
                table: "LinhaPedidoImportacao",
                column: "PedidoImportacaoID");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId",
                principalTable: "AspNetRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId",
                principalTable: "AspNetRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
