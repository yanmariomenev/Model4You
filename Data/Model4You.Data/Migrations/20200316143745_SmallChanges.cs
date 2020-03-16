using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Model4You.Data.Migrations
{
    public partial class SmallChanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "ModelsInformation",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedOn",
                table: "ModelsInformation",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "Locations",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedOn",
                table: "Locations",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "ModelsInformation");

            migrationBuilder.DropColumn(
                name: "ModifiedOn",
                table: "ModelsInformation");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "ModifiedOn",
                table: "Locations");
        }
    }
}
