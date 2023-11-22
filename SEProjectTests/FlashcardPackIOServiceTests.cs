using Microsoft.EntityFrameworkCore;
using SEProject.Exceptions;
using SEProject.Models;
using SEProject.Services;

namespace SEProjectTests;

public class FlashcardPackIOServiceTests
{
    private DbContextOptions<DatabaseContext> options = new DbContextOptionsBuilder<DatabaseContext>()
        .UseInMemoryDatabase(databaseName: "LoadFlashcardPackAsync_WithValidID_ReturnsFlashcardPack")
        .Options;
    
    // This test is covering the LoadFlashcardPackAsync method with a valid ID
    [Fact]
    public async Task LoadFlashcardPackAsync_WithValidID_ReturnsFlashcardPack()
    {
        // Arrange
        using (var context = new DatabaseContext(options))
        {
            var service = new FlashcardPackIOService(context);
            var packId = Guid.NewGuid();
            var flashcardPack = new FlashcardPack { ID = packId, Name = "Test Pack" };
            context.FlashcardPacks.Add(flashcardPack);
            context.SaveChanges();

            // Act
            var result = await service.LoadFlashcardPackAsync(packId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(flashcardPack, result);
        }
    }
    
    // This test is covering the LoadFlashcardPackAsync method with an invalid ID
    [Fact]
    public async Task LoadFlashcardPackAsync_WithInvalidID_ReturnsNull()
    {
        // Arrange
        using (var context = new DatabaseContext(options))
        {
            var service = new FlashcardPackIOService(context);
            var validPackId = Guid.NewGuid();
            var invalidPackId = Guid.Empty;
            var flashcardPack = new FlashcardPack { ID = validPackId, Name = "Test Pack" };
            context.FlashcardPacks.Add(flashcardPack);
            context.SaveChanges();

            // Act and Assert
            await Assert.ThrowsAsync<Exception>(async () => await service.LoadFlashcardPackAsync(invalidPackId));
        }
    }
    
    // This test is covering the LoadFlashcardPackAsync method
    [Fact]
    public async Task LoadFlashcardPacksAsync_ReturnsFlashcardPacks()
    {
        // Arrange
        using (var context = new DatabaseContext(options))
        {
            var service = new FlashcardPackIOService(context);
            var flashcardPacks = new List<FlashcardPack>
            {
                new FlashcardPack { ID = Guid.NewGuid(), Name = "Pack 1" },
                new FlashcardPack { ID = Guid.NewGuid(), Name = "Pack 2" }
            };
            context.FlashcardPacks.AddRange(flashcardPacks);
            context.SaveChanges();

            // Act
            var result = await service.LoadFlashcardPacksAsync();

            // Assert
            Assert.Equal(flashcardPacks.Count, result.Count);
            Assert.Equal(flashcardPacks, result);
        }
    }
    
    // This test is covering the SaveFlashcardPackAsync method
    [Fact]
    public async Task SaveFlashcardPackAsync_WithValidFlashcardPack_SavesFlashcardPack()
    {
        // Arrange
        using (var context = new DatabaseContext(options))
        {
            var service = new FlashcardPackIOService(context);
            var flashcardPack = new FlashcardPack { ID = Guid.NewGuid(), Name = "Test Pack" };

            // Act
            await service.SaveFlashcardPackAsync(flashcardPack);
            var savedPack = await context.FlashcardPacks.FindAsync(flashcardPack.ID);

            // Assert
            Assert.NotNull(savedPack);
            Assert.Equal(flashcardPack.Name, savedPack.Name);
        }
    }
    
    // This test is covering the SaveFlashcardPackAsync method with a validation function
    [Fact]
    public async Task RemoveFlashcardPackAsync_WithExistingFlashcardPack_RemovesFlashcardPack()
    {
        // Arrange
        using (var context = new DatabaseContext(options))
        {
            var service = new FlashcardPackIOService(context);
            var flashcardPack = new FlashcardPack { ID = Guid.NewGuid(), Name = "Test Pack" };
            context.FlashcardPacks.Add(flashcardPack);
            context.SaveChanges();

            // Act
            await service.RemoveFlashcardPackAsync(flashcardPack.ID);
            var removedPack = await context.FlashcardPacks.FindAsync(flashcardPack.ID);

            // Assert
            Assert.Null(removedPack);
        }
    }
}