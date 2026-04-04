using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieTracker.Migrations
{
    /// <inheritdoc />
    public partial class AddedApprovalAndUserToMovie : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AddedByUserId",
                table: "Movies",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsApproved",
                table: "Movies",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Movies_AddedByUserId",
                table: "Movies",
                column: "AddedByUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Movies_AspNetUsers_AddedByUserId",
                table: "Movies",
                column: "AddedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Movies_AspNetUsers_AddedByUserId",
                table: "Movies");

            migrationBuilder.DropIndex(
                name: "IX_Movies_AddedByUserId",
                table: "Movies");

            migrationBuilder.DropColumn(
                name: "AddedByUserId",
                table: "Movies");

            migrationBuilder.DropColumn(
                name: "IsApproved",
                table: "Movies");
        }
    }
}
