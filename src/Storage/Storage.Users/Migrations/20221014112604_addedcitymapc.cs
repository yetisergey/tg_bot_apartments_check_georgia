using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Storage.Bot.Migrations
{
    public partial class addedcitymapc : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MapC",
                table: "Cities",
                type: "text",
                nullable: false,
                defaultValue: string.Empty);

            migrationBuilder.UpdateData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 1,
                column: "MapC",
                value: "41.73188365,44.8368762993663");

            migrationBuilder.UpdateData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 2,
                column: "MapC",
                value: "41.73188365,44.8368762993663");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MapC",
                table: "Cities");
        }
    }
}
