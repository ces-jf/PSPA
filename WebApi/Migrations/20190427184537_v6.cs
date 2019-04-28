using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApi.Migrations
{
    public partial class v6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PedidoImportacao_Usuario_UsuarioMatricula",
                table: "PedidoImportacao");

            migrationBuilder.DropForeignKey(
                name: "FK_Usuario_Usuario_UsuarioMatricula",
                table: "Usuario");

            migrationBuilder.DropIndex(
                name: "IX_Usuario_UsuarioMatricula",
                table: "Usuario");

            migrationBuilder.DropColumn(
                name: "Nome",
                table: "Usuario");

            migrationBuilder.DropColumn(
                name: "Senha",
                table: "Usuario");

            migrationBuilder.DropColumn(
                name: "SobreNome",
                table: "Usuario");

            migrationBuilder.DropColumn(
                name: "UsuarioMatricula",
                table: "Usuario");

            migrationBuilder.RenameColumn(
                name: "Matricula",
                table: "Usuario",
                newName: "RegisterNumber");

            migrationBuilder.RenameColumn(
                name: "UsuarioMatricula",
                table: "PedidoImportacao",
                newName: "UsuarioRegisterNumber");

            migrationBuilder.RenameIndex(
                name: "IX_PedidoImportacao_UsuarioMatricula",
                table: "PedidoImportacao",
                newName: "IX_PedidoImportacao_UsuarioRegisterNumber");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Usuario",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<int>(
                name: "AccessFailedCount",
                table: "Usuario",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ConcurrencyStamp",
                table: "Usuario",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "EmailConfirmed",
                table: "Usuario",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "Usuario",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Id",
                table: "Usuario",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "LockoutEnabled",
                table: "Usuario",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "LockoutEnd",
                table: "Usuario",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NormalizedEmail",
                table: "Usuario",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NormalizedUserName",
                table: "Usuario",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "Usuario",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PasswordHash",
                table: "Usuario",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "Usuario",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "PhoneNumberConfirmed",
                table: "Usuario",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "SecondName",
                table: "Usuario",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SecurityStamp",
                table: "Usuario",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "TwoFactorEnabled",
                table: "Usuario",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "Usuario",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PedidoImportacao_Usuario_UsuarioRegisterNumber",
                table: "PedidoImportacao",
                column: "UsuarioRegisterNumber",
                principalTable: "Usuario",
                principalColumn: "RegisterNumber",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PedidoImportacao_Usuario_UsuarioRegisterNumber",
                table: "PedidoImportacao");

            migrationBuilder.DropColumn(
                name: "AccessFailedCount",
                table: "Usuario");

            migrationBuilder.DropColumn(
                name: "ConcurrencyStamp",
                table: "Usuario");

            migrationBuilder.DropColumn(
                name: "EmailConfirmed",
                table: "Usuario");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "Usuario");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Usuario");

            migrationBuilder.DropColumn(
                name: "LockoutEnabled",
                table: "Usuario");

            migrationBuilder.DropColumn(
                name: "LockoutEnd",
                table: "Usuario");

            migrationBuilder.DropColumn(
                name: "NormalizedEmail",
                table: "Usuario");

            migrationBuilder.DropColumn(
                name: "NormalizedUserName",
                table: "Usuario");

            migrationBuilder.DropColumn(
                name: "Password",
                table: "Usuario");

            migrationBuilder.DropColumn(
                name: "PasswordHash",
                table: "Usuario");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "Usuario");

            migrationBuilder.DropColumn(
                name: "PhoneNumberConfirmed",
                table: "Usuario");

            migrationBuilder.DropColumn(
                name: "SecondName",
                table: "Usuario");

            migrationBuilder.DropColumn(
                name: "SecurityStamp",
                table: "Usuario");

            migrationBuilder.DropColumn(
                name: "TwoFactorEnabled",
                table: "Usuario");

            migrationBuilder.DropColumn(
                name: "UserName",
                table: "Usuario");

            migrationBuilder.RenameColumn(
                name: "RegisterNumber",
                table: "Usuario",
                newName: "Matricula");

            migrationBuilder.RenameColumn(
                name: "UsuarioRegisterNumber",
                table: "PedidoImportacao",
                newName: "UsuarioMatricula");

            migrationBuilder.RenameIndex(
                name: "IX_PedidoImportacao_UsuarioRegisterNumber",
                table: "PedidoImportacao",
                newName: "IX_PedidoImportacao_UsuarioMatricula");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Usuario",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Nome",
                table: "Usuario",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Senha",
                table: "Usuario",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SobreNome",
                table: "Usuario",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "UsuarioMatricula",
                table: "Usuario",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Usuario_UsuarioMatricula",
                table: "Usuario",
                column: "UsuarioMatricula");

            migrationBuilder.AddForeignKey(
                name: "FK_PedidoImportacao_Usuario_UsuarioMatricula",
                table: "PedidoImportacao",
                column: "UsuarioMatricula",
                principalTable: "Usuario",
                principalColumn: "Matricula",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Usuario_Usuario_UsuarioMatricula",
                table: "Usuario",
                column: "UsuarioMatricula",
                principalTable: "Usuario",
                principalColumn: "Matricula",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
