using Microsoft.EntityFrameworkCore.Migrations;

namespace Model4You.Data.Migrations
{
    public partial class InitialDbBuildFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserImages_BlogContents_BlogContentId1",
                table: "UserImages");

            migrationBuilder.DropIndex(
                name: "IX_UserImages_BlogContentId1",
                table: "UserImages");

            migrationBuilder.DropColumn(
                name: "BlogContentId1",
                table: "UserImages");

            migrationBuilder.AlterColumn<int>(
                name: "BlogContentId",
                table: "UserImages",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserImages_BlogContents_BlogContentId",
                table: "UserImages");

            migrationBuilder.DropIndex(
                name: "IX_UserImages_BlogContentId",
                table: "UserImages");

            migrationBuilder.AlterColumn<string>(
                name: "BlogContentId",
                table: "UserImages",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<int>(
                name: "BlogContentId1",
                table: "UserImages",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserImages_BlogContentId1",
                table: "UserImages",
                column: "BlogContentId1");

            migrationBuilder.AddForeignKey(
                name: "FK_UserImages_BlogContents_BlogContentId1",
                table: "UserImages",
                column: "BlogContentId1",
                principalTable: "BlogContents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
