using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApi.Migrations
{
    public partial class v13 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetRoleClaims_role_RoleId",
                table: "AspNetRoleClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserClaims_usuario_UserId",
                table: "AspNetUserClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserLogins_usuario_UserId",
                table: "AspNetUserLogins");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserTokens_usuario_UserId",
                table: "AspNetUserTokens");

            migrationBuilder.DropForeignKey(
                name: "FK_arquivo_base_Indice_IndexID",
                schema: "PSPABase",
                table: "arquivo_base");

            migrationBuilder.DropForeignKey(
                name: "FK_arquivo_base_pedido_importacao_PedidoImportacaoID",
                schema: "PSPABase",
                table: "arquivo_base");

            migrationBuilder.DropForeignKey(
                name: "FK_log_pedido_importacao_pedido_importacao_PedidoImportacaoID",
                schema: "PSPABase",
                table: "log_pedido_importacao");

            migrationBuilder.DropForeignKey(
                name: "FK_pedido_importacao_usuario_UsuarioId",
                schema: "PSPABase",
                table: "pedido_importacao");

            migrationBuilder.DropForeignKey(
                name: "FK_usuario_role_role_RoleId",
                schema: "PSPABase",
                table: "usuario_role");

            migrationBuilder.DropForeignKey(
                name: "FK_usuario_role_usuario_UserId",
                schema: "PSPABase",
                table: "usuario_role");

            migrationBuilder.DropPrimaryKey(
                name: "PK_usuario",
                schema: "PSPABase",
                table: "usuario");

            migrationBuilder.DropPrimaryKey(
                name: "PK_role",
                schema: "PSPABase",
                table: "role");

            migrationBuilder.DropPrimaryKey(
                name: "PK_usuario_role",
                schema: "PSPABase",
                table: "usuario_role");

            migrationBuilder.DropPrimaryKey(
                name: "PK_pedido_importacao",
                schema: "PSPABase",
                table: "pedido_importacao");

            migrationBuilder.DropPrimaryKey(
                name: "PK_log_pedido_importacao",
                schema: "PSPABase",
                table: "log_pedido_importacao");

            migrationBuilder.DropPrimaryKey(
                name: "PK_arquivo_base",
                schema: "PSPABase",
                table: "arquivo_base");

            migrationBuilder.RenameTable(
                name: "usuario",
                schema: "PSPABase",
                newName: "Usuario",
                newSchema: "PSPABase");

            migrationBuilder.RenameTable(
                name: "role",
                schema: "PSPABase",
                newName: "Role",
                newSchema: "PSPABase");

            migrationBuilder.RenameTable(
                name: "Indice",
                newName: "Indice",
                newSchema: "PSPABase");

            migrationBuilder.RenameTable(
                name: "usuario_role",
                schema: "PSPABase",
                newName: "UsuarioRole",
                newSchema: "PSPABase");

            migrationBuilder.RenameTable(
                name: "pedido_importacao",
                schema: "PSPABase",
                newName: "PedidoImportacao",
                newSchema: "PSPABase");

            migrationBuilder.RenameTable(
                name: "log_pedido_importacao",
                schema: "PSPABase",
                newName: "LogPedidoImportacao",
                newSchema: "PSPABase");

            migrationBuilder.RenameTable(
                name: "arquivo_base",
                schema: "PSPABase",
                newName: "ArquivoBase",
                newSchema: "PSPABase");

            migrationBuilder.RenameIndex(
                name: "IX_usuario_UserName",
                schema: "PSPABase",
                table: "Usuario",
                newName: "IX_Usuario_UserName");

            migrationBuilder.RenameIndex(
                name: "IX_usuario_Email",
                schema: "PSPABase",
                table: "Usuario",
                newName: "IX_Usuario_Email");

            migrationBuilder.RenameColumn(
                name: "UserId",
                schema: "PSPABase",
                table: "UsuarioRole",
                newName: "UsuarioId");

            migrationBuilder.RenameIndex(
                name: "IX_usuario_role_RoleId",
                schema: "PSPABase",
                table: "UsuarioRole",
                newName: "IX_UsuarioRole_RoleId");

            migrationBuilder.RenameIndex(
                name: "IX_pedido_importacao_UsuarioId",
                schema: "PSPABase",
                table: "PedidoImportacao",
                newName: "IX_PedidoImportacao_UsuarioId");

            migrationBuilder.RenameIndex(
                name: "IX_log_pedido_importacao_PedidoImportacaoID",
                schema: "PSPABase",
                table: "LogPedidoImportacao",
                newName: "IX_LogPedidoImportacao_PedidoImportacaoID");

            migrationBuilder.RenameIndex(
                name: "IX_arquivo_base_PedidoImportacaoID",
                schema: "PSPABase",
                table: "ArquivoBase",
                newName: "IX_ArquivoBase_PedidoImportacaoID");

            migrationBuilder.RenameIndex(
                name: "IX_arquivo_base_IndexID",
                schema: "PSPABase",
                table: "ArquivoBase",
                newName: "IX_ArquivoBase_IndexID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Usuario",
                schema: "PSPABase",
                table: "Usuario",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Role",
                schema: "PSPABase",
                table: "Role",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UsuarioRole",
                schema: "PSPABase",
                table: "UsuarioRole",
                columns: new[] { "UsuarioId", "RoleId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_PedidoImportacao",
                schema: "PSPABase",
                table: "PedidoImportacao",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LogPedidoImportacao",
                schema: "PSPABase",
                table: "LogPedidoImportacao",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ArquivoBase",
                schema: "PSPABase",
                table: "ArquivoBase",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetRoleClaims_Role_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId",
                principalSchema: "PSPABase",
                principalTable: "Role",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserClaims_Usuario_UserId",
                table: "AspNetUserClaims",
                column: "UserId",
                principalSchema: "PSPABase",
                principalTable: "Usuario",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserLogins_Usuario_UserId",
                table: "AspNetUserLogins",
                column: "UserId",
                principalSchema: "PSPABase",
                principalTable: "Usuario",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserTokens_Usuario_UserId",
                table: "AspNetUserTokens",
                column: "UserId",
                principalSchema: "PSPABase",
                principalTable: "Usuario",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ArquivoBase_Indice_IndexID",
                schema: "PSPABase",
                table: "ArquivoBase",
                column: "IndexID",
                principalSchema: "PSPABase",
                principalTable: "Indice",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ArquivoBase_PedidoImportacao_PedidoImportacaoID",
                schema: "PSPABase",
                table: "ArquivoBase",
                column: "PedidoImportacaoID",
                principalSchema: "PSPABase",
                principalTable: "PedidoImportacao",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_LogPedidoImportacao_PedidoImportacao_PedidoImportacaoID",
                schema: "PSPABase",
                table: "LogPedidoImportacao",
                column: "PedidoImportacaoID",
                principalSchema: "PSPABase",
                principalTable: "PedidoImportacao",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PedidoImportacao_Usuario_UsuarioId",
                schema: "PSPABase",
                table: "PedidoImportacao",
                column: "UsuarioId",
                principalSchema: "PSPABase",
                principalTable: "Usuario",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UsuarioRole_Role_RoleId",
                schema: "PSPABase",
                table: "UsuarioRole",
                column: "RoleId",
                principalSchema: "PSPABase",
                principalTable: "Role",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UsuarioRole_Usuario_UsuarioId",
                schema: "PSPABase",
                table: "UsuarioRole",
                column: "UsuarioId",
                principalSchema: "PSPABase",
                principalTable: "Usuario",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetRoleClaims_Role_RoleId",
                table: "AspNetRoleClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserClaims_Usuario_UserId",
                table: "AspNetUserClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserLogins_Usuario_UserId",
                table: "AspNetUserLogins");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserTokens_Usuario_UserId",
                table: "AspNetUserTokens");

            migrationBuilder.DropForeignKey(
                name: "FK_ArquivoBase_Indice_IndexID",
                schema: "PSPABase",
                table: "ArquivoBase");

            migrationBuilder.DropForeignKey(
                name: "FK_ArquivoBase_PedidoImportacao_PedidoImportacaoID",
                schema: "PSPABase",
                table: "ArquivoBase");

            migrationBuilder.DropForeignKey(
                name: "FK_LogPedidoImportacao_PedidoImportacao_PedidoImportacaoID",
                schema: "PSPABase",
                table: "LogPedidoImportacao");

            migrationBuilder.DropForeignKey(
                name: "FK_PedidoImportacao_Usuario_UsuarioId",
                schema: "PSPABase",
                table: "PedidoImportacao");

            migrationBuilder.DropForeignKey(
                name: "FK_UsuarioRole_Role_RoleId",
                schema: "PSPABase",
                table: "UsuarioRole");

            migrationBuilder.DropForeignKey(
                name: "FK_UsuarioRole_Usuario_UsuarioId",
                schema: "PSPABase",
                table: "UsuarioRole");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Usuario",
                schema: "PSPABase",
                table: "Usuario");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Role",
                schema: "PSPABase",
                table: "Role");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UsuarioRole",
                schema: "PSPABase",
                table: "UsuarioRole");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PedidoImportacao",
                schema: "PSPABase",
                table: "PedidoImportacao");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LogPedidoImportacao",
                schema: "PSPABase",
                table: "LogPedidoImportacao");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ArquivoBase",
                schema: "PSPABase",
                table: "ArquivoBase");

            migrationBuilder.RenameTable(
                name: "Usuario",
                schema: "PSPABase",
                newName: "usuario",
                newSchema: "PSPABase");

            migrationBuilder.RenameTable(
                name: "Role",
                schema: "PSPABase",
                newName: "role",
                newSchema: "PSPABase");

            migrationBuilder.RenameTable(
                name: "Indice",
                schema: "PSPABase",
                newName: "Indice");

            migrationBuilder.RenameTable(
                name: "UsuarioRole",
                schema: "PSPABase",
                newName: "usuario_role",
                newSchema: "PSPABase");

            migrationBuilder.RenameTable(
                name: "PedidoImportacao",
                schema: "PSPABase",
                newName: "pedido_importacao",
                newSchema: "PSPABase");

            migrationBuilder.RenameTable(
                name: "LogPedidoImportacao",
                schema: "PSPABase",
                newName: "log_pedido_importacao",
                newSchema: "PSPABase");

            migrationBuilder.RenameTable(
                name: "ArquivoBase",
                schema: "PSPABase",
                newName: "arquivo_base",
                newSchema: "PSPABase");

            migrationBuilder.RenameIndex(
                name: "IX_Usuario_UserName",
                schema: "PSPABase",
                table: "usuario",
                newName: "IX_usuario_UserName");

            migrationBuilder.RenameIndex(
                name: "IX_Usuario_Email",
                schema: "PSPABase",
                table: "usuario",
                newName: "IX_usuario_Email");

            migrationBuilder.RenameColumn(
                name: "UsuarioId",
                schema: "PSPABase",
                table: "usuario_role",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_UsuarioRole_RoleId",
                schema: "PSPABase",
                table: "usuario_role",
                newName: "IX_usuario_role_RoleId");

            migrationBuilder.RenameIndex(
                name: "IX_PedidoImportacao_UsuarioId",
                schema: "PSPABase",
                table: "pedido_importacao",
                newName: "IX_pedido_importacao_UsuarioId");

            migrationBuilder.RenameIndex(
                name: "IX_LogPedidoImportacao_PedidoImportacaoID",
                schema: "PSPABase",
                table: "log_pedido_importacao",
                newName: "IX_log_pedido_importacao_PedidoImportacaoID");

            migrationBuilder.RenameIndex(
                name: "IX_ArquivoBase_PedidoImportacaoID",
                schema: "PSPABase",
                table: "arquivo_base",
                newName: "IX_arquivo_base_PedidoImportacaoID");

            migrationBuilder.RenameIndex(
                name: "IX_ArquivoBase_IndexID",
                schema: "PSPABase",
                table: "arquivo_base",
                newName: "IX_arquivo_base_IndexID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_usuario",
                schema: "PSPABase",
                table: "usuario",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_role",
                schema: "PSPABase",
                table: "role",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_usuario_role",
                schema: "PSPABase",
                table: "usuario_role",
                columns: new[] { "UserId", "RoleId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_pedido_importacao",
                schema: "PSPABase",
                table: "pedido_importacao",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_log_pedido_importacao",
                schema: "PSPABase",
                table: "log_pedido_importacao",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_arquivo_base",
                schema: "PSPABase",
                table: "arquivo_base",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetRoleClaims_role_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId",
                principalSchema: "PSPABase",
                principalTable: "role",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserClaims_usuario_UserId",
                table: "AspNetUserClaims",
                column: "UserId",
                principalSchema: "PSPABase",
                principalTable: "usuario",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserLogins_usuario_UserId",
                table: "AspNetUserLogins",
                column: "UserId",
                principalSchema: "PSPABase",
                principalTable: "usuario",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserTokens_usuario_UserId",
                table: "AspNetUserTokens",
                column: "UserId",
                principalSchema: "PSPABase",
                principalTable: "usuario",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_arquivo_base_Indice_IndexID",
                schema: "PSPABase",
                table: "arquivo_base",
                column: "IndexID",
                principalTable: "Indice",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_arquivo_base_pedido_importacao_PedidoImportacaoID",
                schema: "PSPABase",
                table: "arquivo_base",
                column: "PedidoImportacaoID",
                principalSchema: "PSPABase",
                principalTable: "pedido_importacao",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_log_pedido_importacao_pedido_importacao_PedidoImportacaoID",
                schema: "PSPABase",
                table: "log_pedido_importacao",
                column: "PedidoImportacaoID",
                principalSchema: "PSPABase",
                principalTable: "pedido_importacao",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_pedido_importacao_usuario_UsuarioId",
                schema: "PSPABase",
                table: "pedido_importacao",
                column: "UsuarioId",
                principalSchema: "PSPABase",
                principalTable: "usuario",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

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
    }
}
