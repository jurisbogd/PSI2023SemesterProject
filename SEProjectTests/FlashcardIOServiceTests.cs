using Microsoft.EntityFrameworkCore;
using SEProject.Models;
using SEProject.Services;

namespace SEProjectTests;

public class FlashcardIOServiceTests
{
    private DbContextOptions<DatabaseContext> options = new DbContextOptionsBuilder<DatabaseContext>()
        .UseInMemoryDatabase(databaseName: "LoadFlashcardPacksAsync_ReturnsFlashcardPacks")
        .Options;
    
    // This test is covering the LoadFlashcardsAsync method
    [Fact]
    public async Task LoadFlashcardsAsync_ReturnsFlashcards()
    {
        // Arrange
        using (var context = new DatabaseContext(options))
        {
            var service = new FlashcardIOService(context);
            var packID = Guid.NewGuid();
            var flashcard = new Flashcard { ID = Guid.NewGuid(), PackID = packID };
            context.FlashcardPacks.Add(new FlashcardPack { ID = packID, Flashcards = new List<Flashcard> { flashcard } });
            context.SaveChanges();

            // Act
            var result = await service.LoadFlashcardsAsync(packID);

            // Assert
            Assert.Single(result);
            Assert.Equal(flashcard, result.Single());
        }
    }
    
    // This test is covering the SaveFlashcard method
    [Fact]
    public async Task SaveFlashcard_WithValidFlashcard_SavesFlashcard()
    {
        // Arrange
        using (var context = new DatabaseContext(options))
        {
            var service = new FlashcardIOService(context);
            var flashcard = new Flashcard { ID = Guid.NewGuid(), Question = "Test Question", Answer = "Test Answer" };

            // Act
            await service.SaveFlashcard(flashcard, null);
            var savedFlashcard = await context.Flashcards.FindAsync(flashcard.ID);

            // Assert
            Assert.NotNull(savedFlashcard);
            Assert.Equal(flashcard.Question, savedFlashcard.Question);
            Assert.Equal(flashcard.Answer, savedFlashcard.Answer);
        }
    }
    
    // This test is covering the RemoveFlashcard method
    [Fact]
    public async Task RemoveFlashcard_WithExistingFlashcard_RemovesFlashcard()
    {
        // Arrange
        using (var context = new DatabaseContext(options))
        {
            var service = new FlashcardIOService(context);
            var flashcard = new Flashcard { ID = Guid.NewGuid(), Question = "Test Question", Answer = "Test Answer" };
            context.Flashcards.Add(flashcard);
            context.SaveChanges();

            // Act
            await service.RemoveFlashcard(flashcard);
            var removedFlashcard = await context.Flashcards.FindAsync(flashcard.ID);

            // Assert
            Assert.Null(removedFlashcard);
        }
    }
}