using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace BoardGames.DataContract.Migrations
{
  /// <inheritdoc />
  public partial class add_publisher_and_category_table : Migration
  {
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.AddColumn<int>(
          name: "PublisherId",
          table: "BoardGames",
          type: "integer",
          nullable: false,
          defaultValue: 0);

      migrationBuilder.CreateTable(
          name: "Categories",
          columns: table => new
          {
            CategoryId = table.Column<int>(type: "integer", nullable: false)
                  .Annotation("Npgsql:ValueGenerationStrategy",
                  NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
            Name = table.Column<string>(type: "character varying(200)",
              maxLength: 200, nullable: false),
            CreatedDate = table.Column<DateTime>(type: "timestamp with time zone",
              nullable: false),
            LastModifiedDate = table.Column<DateTime>(type: "timestamp with time zone",
              nullable: false)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_Categories", x => x.CategoryId);
          });

      migrationBuilder.CreateTable(
          name: "Publishers",
          columns: table => new
          {
            PublisherId = table.Column<int>(type: "integer", nullable: false)
                  .Annotation("Npgsql:ValueGenerationStrategy",
                  NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
            Name = table.Column<string>(type: "character varying(200)",
              maxLength: 200, nullable: false),
            CreatedDate = table.Column<DateTime>(type: "timestamp with time zone",
              nullable: false),
            LastModifiedDate = table.Column<DateTime>(type: "timestamp with time zone",
              nullable: false)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_Publishers", x => x.PublisherId);
          });

      migrationBuilder.CreateTable(
          name: "BoardGames_Categories",
          columns: table => new
          {
            BoardGameId = table.Column<int>(type: "integer", nullable: false),
            CategoryId = table.Column<int>(type: "integer", nullable: false)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_BoardGames_Categories",
                    x => new { x.BoardGameId, x.CategoryId });
            table.ForeignKey(
                      name: "FK_BoardGames_Categories_BoardGames_BoardGameId",
                      column: x => x.BoardGameId,
                      principalTable: "BoardGames",
                      principalColumn: "BoardGameId",
                      onDelete: ReferentialAction.Cascade);
            table.ForeignKey(
                      name: "FK_BoardGames_Categories_Categories_CategoryId",
                      column: x => x.CategoryId,
                      principalTable: "Categories",
                      principalColumn: "CategoryId",
                      onDelete: ReferentialAction.Cascade);
          });

      migrationBuilder.CreateIndex(
          name: "IX_BoardGames_PublisherId",
          table: "BoardGames",
          column: "PublisherId");

      migrationBuilder.CreateIndex(
          name: "IX_BoardGames_Categories_CategoryId",
          table: "BoardGames_Categories",
          column: "CategoryId");

      migrationBuilder.AddForeignKey(
          name: "FK_BoardGames_Publishers_PublisherId",
          table: "BoardGames",
          column: "PublisherId",
          principalTable: "Publishers",
          principalColumn: "PublisherId",
          onDelete: ReferentialAction.Cascade);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropForeignKey(
          name: "FK_BoardGames_Publishers_PublisherId",
          table: "BoardGames");

      migrationBuilder.DropTable(
          name: "BoardGames_Categories");

      migrationBuilder.DropTable(
          name: "Publishers");

      migrationBuilder.DropTable(
          name: "Categories");

      migrationBuilder.DropIndex(
          name: "IX_BoardGames_PublisherId",
          table: "BoardGames");

      migrationBuilder.DropColumn(
          name: "PublisherId",
          table: "BoardGames");
    }
  }
}
