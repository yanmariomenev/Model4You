using Microsoft.EntityFrameworkCore.Migrations;

namespace Model4You.Data.Migrations
{
    public partial class UpdateModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserImages_BlogContents_BlogContentId",
                table: "UserImages");

            migrationBuilder.DropIndex(
                name: "IX_UserImages_BlogContentId",
                table: "UserImages");

            migrationBuilder.DropColumn(
                name: "BlogContentId",
                table: "UserImages");

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "BlogContents",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "BlogContents");

            migrationBuilder.AddColumn<int>(
                name: "BlogContentId",
                table: "UserImages",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_UserImages_BlogContentId",
                table: "UserImages",
                column: "BlogContentId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserImages_BlogContents_BlogContentId",
                table: "UserImages",
                column: "BlogContentId",
                principalTable: "BlogContents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
