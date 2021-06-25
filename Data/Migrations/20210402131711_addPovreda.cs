using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class addPovreda : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "povrede",
                columns: table => new
                {
                    povredaID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    igracID = table.Column<int>(nullable: false),
                    datumPovrede = table.Column<DateTime>(nullable: false),
                    odsustvoDO = table.Column<DateTime>(nullable: false),
                    tipPovrede = table.Column<int>(nullable: false),
                    kratkiOpis = table.Column<string>(nullable: true),
                    misljenjeLjekara = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_povrede", x => x.povredaID);
                    table.ForeignKey(
                        name: "FK_povrede_igraci_igracID",
                        column: x => x.igracID,
                        principalTable: "igraci",
                        principalColumn: "igracID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_povrede_igracID",
                table: "povrede",
                column: "igracID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "povrede");
        }
    }
}
