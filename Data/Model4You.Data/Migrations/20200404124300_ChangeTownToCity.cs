using Microsoft.EntityFrameworkCore.Migrations;

namespace Model4You.Data.Migrations
{
    public partial class ChangeTownToCity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Town",
                table: "ModelsInformation");

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "ModelsInformation",
                maxLength: 30,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "City",
                table: "ModelsInformation");

            migrationBuilder.AddColumn<string>(
                name: "Town",
                table: "ModelsInformation",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: true);
        }
    }
}
