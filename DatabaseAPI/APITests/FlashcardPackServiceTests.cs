

using Microsoft.EntityFrameworkCore;
using DatabaseAPI.Models;
using DatabaseAPI.Services;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace APITests;

[TestClass]
public class FlashcardPackServiceTests
{
    private DbContextOptions<DatabaseContext> _options = new DbContextOptionsBuilder<DatabaseContext>()
    .UseInMemoryDatabase(databaseName: "LoadFlashcardPackAsync_WithValidID_ReturnsFlashcardPack")
    .Options;

    [TestMethod]
    public async Task LoadFlashcardPackAsync_WithValidID_ReturnsFlashcardPack()
    {
        using (var context = new DatabaseContext(_options))
        {
            var flashcardPackService = new FlashcardPackIOService(context);
            var flashcardPack = new FlashcardPack { ID = Guid.NewGuid(), Name = "Integration Test Pack" };
            var user = new User { UserID = Guid.NewGuid(), Email = "test_user"};

            // Act
            await flashcardPackService.SaveFlashcardPackAsync(flashcardPack, user.UserID);
            var loadedPack = await flashcardPackService.LoadFlashcardPackAsync(flashcardPack.ID, user.UserID);

            // Assert
            Assert.IsNotNull(loadedPack);
            Assert.AreEqual(flashcardPack.Name, loadedPack.Name);
        }
    }



/*    [Fact]
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
    }*/
}

