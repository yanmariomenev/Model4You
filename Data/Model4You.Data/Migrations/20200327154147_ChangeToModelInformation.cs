using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Model4You.Data.Migrations
{
    public partial class ChangeToModelInformation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                table: "ModelsInformation",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "ModelsInformation",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_ModelsInformation_IsDeleted",
                table: "ModelsInformation",
                column: "IsDeleted");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ModelsInformation_IsDeleted",
                table: "ModelsInformation");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                table: "ModelsInformation");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "ModelsInformation");
        }
    }
}
