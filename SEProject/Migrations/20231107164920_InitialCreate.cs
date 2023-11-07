using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SEProject.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FlashcardPacks",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FlashcardPacks", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Flashcards",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PackID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Question = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Answer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Difficulty = table.Column<int>(type: "int", nullable: false),
                    IsFavorite = table.Column<bool>(type: "bit", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FlashcardPackFlashcardID = table.Column<Guid>(name: "FlashcardPack<Flashcard>ID", type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Flashcards", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Flashcards_FlashcardPacks_FlashcardPack<Flashcard>ID",
                        column: x => x.FlashcardPackFlashcardID,
                        principalTable: "FlashcardPacks",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Flashcards_FlashcardPack<Flashcard>ID",
                table: "Flashcards",
                column: "FlashcardPack<Flashcard>ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Flashcards");

            migrationBuilder.DropTable(
                name: "FlashcardPacks");
        }
    }
}
