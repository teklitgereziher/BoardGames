using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BoardGames.DataContract.Migrations
{
    /// <inheritdoc />
    public partial class Remove_Publisher_From_BoardGame : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BoardGames_Publishers_PublisherId",
                table: "BoardGames");

            migrationBuilder.AlterColumn<int>(
                name: "PublisherId",
                table: "BoardGames",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_BoardGames_Publishers_PublisherId",
                table: "BoardGames",
                column: "PublisherId",
                principalTable: "Publishers",
                principalColumn: "PublisherId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BoardGames_Publishers_PublisherId",
                table: "BoardGames");

            migrationBuilder.AlterColumn<int>(
                name: "PublisherId",
                table: "BoardGames",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_BoardGames_Publishers_PublisherId",
                table: "BoardGames",
                column: "PublisherId",
                principalTable: "Publishers",
                principalColumn: "PublisherId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
