using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Common.Migrations
{
    public partial class a : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BasicModel",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Discriminator = table.Column<string>(nullable: false),
                    Amount = table.Column<decimal>(nullable: true),
                    DiscordId = table.Column<decimal>(nullable: true),
                    DisplayName = table.Column<string>(nullable: true),
                    AccountBalance = table.Column<int>(nullable: true),
                    OwnerId = table.Column<int>(nullable: true),
                    ExecutionTime = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BasicModel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BasicModel_BasicModel_AccountBalance",
                        column: x => x.AccountBalance,
                        principalTable: "BasicModel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BasicModel_BasicModel_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "BasicModel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BasicModel_AccountBalance",
                table: "BasicModel",
                column: "AccountBalance",
                unique: true,
                filter: "[AccountBalance] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_BasicModel_OwnerId",
                table: "BasicModel",
                column: "OwnerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BasicModel");
        }
    }
}
