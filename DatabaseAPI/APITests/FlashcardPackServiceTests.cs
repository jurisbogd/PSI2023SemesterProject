using DatabaseAPI.Models;
using Microsoft.EntityFrameworkCore;
using DatabaseAPI.Services;
using Microsoft.Extensions.Options;

namespace APITests;

public class FlashcardPackServiceTests
{
    private readonly DbContextOptions<DatabaseContext> _options;

    public FlashcardPackServiceTests()
    {
        var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
        optionsBuilder.UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString());
        _options = optionsBuilder.Options;
    }

    [Fact]
    public async Task LoadFlashcardPackAsync_ShouldRetrieveFlashcardPackForUser()
    {
        using (var context = new DatabaseContext(_options))
        {
            var flashcardPackService = new FlashcardPackIOService(context);
            var flashcardPack = new FlashcardPack { ID = Guid.NewGuid(), Name = "Test pack" };
            var user = new User("user", "user@mail.com", "password");
            var userPack = new UserFlashcardPacks(user.UserID, flashcardPack.ID);

            context.FlashcardPacks.Add(flashcardPack);
            context.Users.Add(user);
            context.UserFlashcardPacks.Add(userPack);
            context.SaveChanges();

            var result = await flashcardPackService.LoadFlashcardPackAsync(flashcardPack.ID, user.UserID);

            Assert.NotNull(result);
            Assert.Equal(flashcardPack, result);
        }
    }

    [Fact]
    public async Task LoadFlashcardPackAsync_WithInvalidID_ReturnsNull()
    {
        using (var context = new DatabaseContext(_options))
        {
            var flashcardPackService = new FlashcardPackIOService(context);
            var flashcardPack = new FlashcardPack { ID = Guid.Empty, Name = "Test pack" };
            var user = new User("user", "user@mail.com", "password");
            context.SaveChanges();

            await Assert.ThrowsAsync<Exception>(async () => await flashcardPackService.LoadFlashcardPackAsync(flashcardPack.ID, user.UserID));
        }
    }

    [Fact]
    public async Task LoadFlashcardPacksAsync_ReturnsFlashcardPacksForUser()
    {
        using (var context = new DatabaseContext(_options))
        {
            var flashcardPackService = new FlashcardPackIOService(context);
            var flashcardPack1 = new FlashcardPack { ID = Guid.NewGuid(), Name = "Pack 1" };
            var flashcardPack2 = new FlashcardPack { ID = Guid.NewGuid(), Name = "Pack 2" };

            var flashcardPacks = new List<FlashcardPack>();
            flashcardPacks.Add(flashcardPack1);
            flashcardPacks.Add(flashcardPack2);

            var user = new User("user", "user@mail.com", "password");
            var userPack1 = new UserFlashcardPacks(user.UserID, flashcardPack1.ID);
            var userPack2 = new UserFlashcardPacks(user.UserID, flashcardPack2.ID);

            context.FlashcardPacks.Add(flashcardPack1);
            context.FlashcardPacks.Add(flashcardPack2);
            context.Users.Add(user);
            context.UserFlashcardPacks.Add(userPack1);
            context.UserFlashcardPacks.Add(userPack2);
            context.SaveChanges();

            var result = await flashcardPackService.LoadFlashcardPacksAsync(user.UserID);

            Assert.Equal(flashcardPacks.Count(), result.Count);
            Assert.Equal(flashcardPacks, result);
        }
    }

    [Fact]
    public async Task SaveFlashcardPackAsync_WithValidFlashcardPack_SavesFlashcardPack()
    {
        using (var context = new DatabaseContext(_options))
        {
            var flashcardPackService = new FlashcardPackIOService(context);
            var flashcardPack = new FlashcardPack { ID = Guid.NewGuid(), Name = "Test Pack" };
            var user = new User("user", "user@mail.com", "password");

            await flashcardPackService.SaveFlashcardPackAsync(flashcardPack, user.UserID);
            var savedPack = await context.FlashcardPacks.FindAsync(flashcardPack.ID);

            Assert.NotNull(savedPack);
            Assert.Equal(flashcardPack.Name, savedPack.Name);
        }
    }

    [Fact]
    public async Task RemoveFlashcardPackAsync_WithExistingFlashcardPack_RemovesFlashcardPack()
    {
        using (var context = new DatabaseContext(_options))
        {
            var flashcardPackService = new FlashcardPackIOService(context);
            var flashcardPack = new FlashcardPack { ID = Guid.NewGuid(), Name = "Test Pack" };
            var user = new User("user", "user@mail.com", "password");
            var userPack = new UserFlashcardPacks(user.UserID, flashcardPack.ID);

            context.FlashcardPacks.Add(flashcardPack);
            context.Users.Add(user);
            context.UserFlashcardPacks.Add(userPack);
            context.SaveChanges();

            await flashcardPackService.RemoveFlashcardPackAsync(flashcardPack.ID);
            var removedPack = await context.FlashcardPacks.FindAsync(flashcardPack.ID);

            Assert.Null(removedPack);
        }
    }

}