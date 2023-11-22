using Microsoft.EntityFrameworkCore;
using SEProject.Exceptions;
using SEProject.Models;
using SEProject.Services;

namespace SEProjectTests;

public class FlashcardDataServiceTests
{
    [Fact]
    public async Task SaveFlashcard_Flashcard_ReturnsTask() {
        // Arrange
        using var context = Mocks.DatabaseContext();
        var service = new FlashcardDataService(context);
        var flashcard = Mocks.Flashcard();

        // Act
        await service.SaveFlashcard(flashcard);

        // Assert
        Assert.Contains(flashcard, context.Flashcards);
    }

    [Fact]
    public async Task DeleteFlashcard_ValidGuid_ReturnsTask() {
        // Arrange
        using var context = Mocks.DatabaseContext();
        var service = new FlashcardDataService(context);
        var flashcard = Mocks.Flashcard();
        await context.Flashcards.AddAsync(flashcard);
        await context.SaveChangesAsync();

        // Act
        await service.DeleteFlashcard(flashcard.ID);

        // Assert
        Assert.DoesNotContain(flashcard, context.Flashcards);
    }

    [Fact]
    public async Task DeleteFlashcard_InvalidGuid_ReturnsTask() {
        // Arrange
        using var context = Mocks.DatabaseContext();
        
        var service = new FlashcardDataService(context);
        var randomGuid = Guid.NewGuid();

        // Act
        var caught = await Assert.ThrowsAsync<FlashcardNotFoundException>(
            async () => await service.DeleteFlashcard(randomGuid)
        );
    }

    [Fact]
    public async Task FetchFlashcard_ValidGuid_ReturnsTaskFlashcard() {
        // Arrange
        using var context = Mocks.DatabaseContext();
        
        var service = new FlashcardDataService(context);
        var flashcard = Mocks.Flashcard();
        await context.Flashcards.AddAsync(flashcard);
        await context.SaveChangesAsync();

        // Act
        var result = await service.FetchFlashcard(flashcard.ID);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(result, flashcard);
    }

    [Fact]
    public async Task FetchFlashcard_InvalidGuid_ThrowsException() {
        // Arrange
        using var context = Mocks.DatabaseContext();
        
        var service = new FlashcardDataService(context);
        var randomGuid = Guid.NewGuid();

        // Act
        var caught = await Assert.ThrowsAsync<FlashcardNotFoundException>(
            async () => await service.FetchFlashcard(randomGuid)
        );
    }
}