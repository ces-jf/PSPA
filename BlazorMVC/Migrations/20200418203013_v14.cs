using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApi.Migrations
{
    public partial class v14 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Header",
                schema: "PSPABase",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: false),
                    HeaderType = table.Column<int>(nullable: false),
                    ArquivoBaseID = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Header", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Header_ArquivoBase_ArquivoBaseID",
                        column: x => x.ArquivoBaseID,
                        principalSchema: "PSPABase",
                        principalTable: "ArquivoBase",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Header_ArquivoBaseID",
                schema: "PSPABase",
                table: "Header",
                column: "ArquivoBaseID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Header",
                schema: "PSPABase");
        }
    }
}
