using Microsoft.EntityFrameworkCore.Migrations;

namespace Model4You.Data.Migrations
{
    public partial class SomeChangesToModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BodyType",
                table: "ModelsInformation");

            migrationBuilder.AddColumn<double>(
                name: "Bust",
                table: "ModelsInformation",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Hips",
                table: "ModelsInformation",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Waist",
                table: "ModelsInformation",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Bust",
                table: "ModelsInformation");

            migrationBuilder.DropColumn(
                name: "Hips",
                table: "ModelsInformation");

            migrationBuilder.DropColumn(
                name: "Waist",
                table: "ModelsInformation");

            migrationBuilder.AddColumn<string>(
                name: "BodyType",
                table: "ModelsInformation",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
