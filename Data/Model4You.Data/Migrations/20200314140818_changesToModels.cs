using Microsoft.EntityFrameworkCore.Migrations;

namespace Model4You.Data.Migrations
{
    public partial class changesToModels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Gender",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "BodyType",
                table: "ModelsInformation",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FacebookUrl",
                table: "ModelsInformation",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Gender",
                table: "ModelsInformation",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "InstagramUrl",
                table: "ModelsInformation",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModelType",
                table: "ModelsInformation",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProfilePicture",
                table: "AspNetUsers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BodyType",
                table: "ModelsInformation");

            migrationBuilder.DropColumn(
                name: "FacebookUrl",
                table: "ModelsInformation");

            migrationBuilder.DropColumn(
                name: "Gender",
                table: "ModelsInformation");

            migrationBuilder.DropColumn(
                name: "InstagramUrl",
                table: "ModelsInformation");

            migrationBuilder.DropColumn(
                name: "ModelType",
                table: "ModelsInformation");

            migrationBuilder.DropColumn(
                name: "ProfilePicture",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<bool>(
                name: "Gender",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
