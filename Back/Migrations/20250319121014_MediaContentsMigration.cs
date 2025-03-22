using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Back.Migrations
{
    /// <inheritdoc />
    public partial class MediaContentsMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "UserId",
                table: "MediaContents",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_MediaContents_UserId",
                table: "MediaContents",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_MediaContents_Users_UserId",
                table: "MediaContents",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MediaContents_Users_UserId",
                table: "MediaContents");

            migrationBuilder.DropIndex(
                name: "IX_MediaContents_UserId",
                table: "MediaContents");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "MediaContents");
        }
    }
}
