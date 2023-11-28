using SEProject.Models;
using SEProject.Services;
using Microsoft.EntityFrameworkCore;

namespace SEProjectTests;

public class FlashcardPackIOServiceIntegrationTests
{
    private readonly DbContextOptions<DatabaseContext> _options = new DbContextOptionsBuilder<DatabaseContext>()
        .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
        .Options;

    // This test is covering the SaveFlashcardPackAsync and LoadFlashcardPackAsync methods
    [Fact]
    public async Task SaveFlashcardPackAsync_SavesAndLoadsFlashcardPack()
    {
        // Arrange
        using (var context = new DatabaseContext(_options))
        {
            var service = new FlashcardPackIOService(context);
            var flashcardPack = new FlashcardPack { ID = Guid.NewGuid(), Name = "Integration Test Pack" };

            // Act
            await service.SaveFlashcardPackAsync(flashcardPack);
            var loadedPack = await service.LoadFlashcardPackAsync(flashcardPack.ID);

            // Assert
            Assert.NotNull(loadedPack);
            Assert.Equal(flashcardPack.Name, loadedPack.Name);
        }
    }
    [Fact]
    public async Task LoadFlashcardPacksAsync_ReturnsListOfFlashcardPacks()
    {
        // Arrange
        using (var context = new DatabaseContext(_options))
        {
            var service = new FlashcardPackIOService(context);
            var flashcardPack1 = new FlashcardPack { ID = Guid.NewGuid(), Name = "Integration Test Pack 1" };
            var flashcardPack2 = new FlashcardPack { ID = Guid.NewGuid(), Name = "Integration Test Pack 2" };

            // Act
            await service.SaveFlashcardPackAsync(flashcardPack1);
            await service.SaveFlashcardPackAsync(flashcardPack2);
            var loadedPacks = await service.LoadFlashcardPacksAsync();

            // Assert
            Assert.NotNull(loadedPacks);
            Assert.Equal(2, loadedPacks.Count);
            Assert.Contains(loadedPacks, pack => pack.ID == flashcardPack1.ID);
            Assert.Contains(loadedPacks, pack => pack.ID == flashcardPack2.ID);
        }
    }
    
    [Fact]
    public async Task SaveFlashcardPackAsync_WithValidationFunction_DoesNotSaveInvalidFlashcardPack()
    {
        // Arrange
        using (var context = new DatabaseContext(_options))
        {
            var service = new FlashcardPackIOService(context);
            var flashcardPack = new FlashcardPack { ID = Guid.NewGuid(), Name = "Invalid Pack" };

            // Act and Assert
            await Assert.ThrowsAsync<Exception>(async () =>
            {
                await service.SaveFlashcardPackAsync(flashcardPack, pack => string.IsNullOrEmpty(pack.Name));
            });

            // Additional Assert to check that the flashcard pack was not saved
            Assert.ThrowsAsync<Exception>(async () =>
            {
                var loadedPack = await service.LoadFlashcardPackAsync(flashcardPack.ID);
                Assert.Null(loadedPack);
            });
        }
    }
}