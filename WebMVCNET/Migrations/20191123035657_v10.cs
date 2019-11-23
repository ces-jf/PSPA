using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApi.Migrations
{
    public partial class v10 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserRoles_role_RoleId",
                table: "AspNetUserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserRoles_usuario_UserId",
                table: "AspNetUserRoles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AspNetUserRoles",
                table: "AspNetUserRoles");

            migrationBuilder.RenameTable(
                name: "AspNetUserRoles",
                newName: "usuario_role",
                newSchema: "PSPABase");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetUserRoles_RoleId",
                schema: "PSPABase",
                table: "usuario_role",
                newName: "IX_usuario_role_RoleId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_usuario_role",
                schema: "PSPABase",
                table: "usuario_role",
                columns: new[] { "UserId", "RoleId" });

            migrationBuilder.AddForeignKey(
                name: "FK_usuario_role_role_RoleId",
                schema: "PSPABase",
                table: "usuario_role",
                column: "RoleId",
                principalSchema: "PSPABase",
                principalTable: "role",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_usuario_role_usuario_UserId",
                schema: "PSPABase",
                table: "usuario_role",
                column: "UserId",
                principalSchema: "PSPABase",
                principalTable: "usuario",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_usuario_role_role_RoleId",
                schema: "PSPABase",
                table: "usuario_role");

            migrationBuilder.DropForeignKey(
                name: "FK_usuario_role_usuario_UserId",
                schema: "PSPABase",
                table: "usuario_role");

            migrationBuilder.DropPrimaryKey(
                name: "PK_usuario_role",
                schema: "PSPABase",
                table: "usuario_role");

            migrationBuilder.RenameTable(
                name: "usuario_role",
                schema: "PSPABase",
                newName: "AspNetUserRoles");

            migrationBuilder.RenameIndex(
                name: "IX_usuario_role_RoleId",
                table: "AspNetUserRoles",
                newName: "IX_AspNetUserRoles_RoleId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AspNetUserRoles",
                table: "AspNetUserRoles",
                columns: new[] { "UserId", "RoleId" });

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserRoles_role_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId",
                principalSchema: "PSPABase",
                principalTable: "role",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserRoles_usuario_UserId",
                table: "AspNetUserRoles",
                column: "UserId",
                principalSchema: "PSPABase",
                principalTable: "usuario",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
