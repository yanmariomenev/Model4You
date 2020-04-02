using Microsoft.EntityFrameworkCore.Migrations;

namespace Model4You.Data.Migrations
{
    public partial class CommentTableChanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BlogComments_AspNetUsers_UserId",
                table: "BlogComments");

            migrationBuilder.DropIndex(
                name: "IX_BlogComments_UserId",
                table: "BlogComments");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "BlogComments");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "BlogComments",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "BlogComments",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "BlogComments");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "BlogComments");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "BlogComments",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BlogComments_UserId",
                table: "BlogComments",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_BlogComments_AspNetUsers_UserId",
                table: "BlogComments",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
