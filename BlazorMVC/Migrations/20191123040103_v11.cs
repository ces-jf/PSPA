using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApi.Migrations
{
    public partial class v11 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ArquivoBase_Indice_IndexID",
                table: "ArquivoBase");

            migrationBuilder.DropForeignKey(
                name: "FK_ArquivoBase_PedidoImportacao_PedidoImportacaoID",
                table: "ArquivoBase");

            migrationBuilder.DropForeignKey(
                name: "FK_Cabecalho_ArquivoBase_ArquivoBaseID",
                table: "Cabecalho");

            migrationBuilder.DropForeignKey(
                name: "FK_LogPedidoImportacao_PedidoImportacao_PedidoImportacaoID",
                table: "LogPedidoImportacao");

            migrationBuilder.DropForeignKey(
                name: "FK_PedidoImportacao_usuario_UsuarioId",
                table: "PedidoImportacao");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PedidoImportacao",
                table: "PedidoImportacao");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LogPedidoImportacao",
                table: "LogPedidoImportacao");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ArquivoBase",
                table: "ArquivoBase");

            migrationBuilder.RenameTable(
                name: "PedidoImportacao",
                newName: "pedido_importacao",
                newSchema: "PSPABase");

            migrationBuilder.RenameTable(
                name: "LogPedidoImportacao",
                newName: "log_pedido_importacao",
                newSchema: "PSPABase");

            migrationBuilder.RenameTable(
                name: "ArquivoBase",
                newName: "arquivo_base",
                newSchema: "PSPABase");

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
                name: "FK_Cabecalho_arquivo_base_ArquivoBaseID",
                table: "Cabecalho",
                column: "ArquivoBaseID",
                principalSchema: "PSPABase",
                principalTable: "arquivo_base",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cabecalho_arquivo_base_ArquivoBaseID",
                table: "Cabecalho");

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
                name: "pedido_importacao",
                schema: "PSPABase",
                newName: "PedidoImportacao");

            migrationBuilder.RenameTable(
                name: "log_pedido_importacao",
                schema: "PSPABase",
                newName: "LogPedidoImportacao");

            migrationBuilder.RenameTable(
                name: "arquivo_base",
                schema: "PSPABase",
                newName: "ArquivoBase");

            migrationBuilder.RenameIndex(
                name: "IX_pedido_importacao_UsuarioId",
                table: "PedidoImportacao",
                newName: "IX_PedidoImportacao_UsuarioId");

            migrationBuilder.RenameIndex(
                name: "IX_log_pedido_importacao_PedidoImportacaoID",
                table: "LogPedidoImportacao",
                newName: "IX_LogPedidoImportacao_PedidoImportacaoID");

            migrationBuilder.RenameIndex(
                name: "IX_arquivo_base_PedidoImportacaoID",
                table: "ArquivoBase",
                newName: "IX_ArquivoBase_PedidoImportacaoID");

            migrationBuilder.RenameIndex(
                name: "IX_arquivo_base_IndexID",
                table: "ArquivoBase",
                newName: "IX_ArquivoBase_IndexID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PedidoImportacao",
                table: "PedidoImportacao",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LogPedidoImportacao",
                table: "LogPedidoImportacao",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ArquivoBase",
                table: "ArquivoBase",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_ArquivoBase_Indice_IndexID",
                table: "ArquivoBase",
                column: "IndexID",
                principalTable: "Indice",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ArquivoBase_PedidoImportacao_PedidoImportacaoID",
                table: "ArquivoBase",
                column: "PedidoImportacaoID",
                principalTable: "PedidoImportacao",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Cabecalho_ArquivoBase_ArquivoBaseID",
                table: "Cabecalho",
                column: "ArquivoBaseID",
                principalTable: "ArquivoBase",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_LogPedidoImportacao_PedidoImportacao_PedidoImportacaoID",
                table: "LogPedidoImportacao",
                column: "PedidoImportacaoID",
                principalTable: "PedidoImportacao",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PedidoImportacao_usuario_UsuarioId",
                table: "PedidoImportacao",
                column: "UsuarioId",
                principalSchema: "PSPABase",
                principalTable: "usuario",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
