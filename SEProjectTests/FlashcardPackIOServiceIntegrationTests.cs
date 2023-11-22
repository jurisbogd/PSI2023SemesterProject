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
}