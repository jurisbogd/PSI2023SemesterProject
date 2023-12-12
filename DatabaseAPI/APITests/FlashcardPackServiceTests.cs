using DatabaseAPI.Models;
using Microsoft.EntityFrameworkCore;
using DatabaseAPI.Services;

namespace APITests;

public class FlashcardPackServiceTests
{
    [Fact]
    public async Task Test1()
    {
        var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>()
            .UseInMemoryDatabase(databaseName: "Test1");
        var options = optionsBuilder.Options;

        using (var context = new DatabaseContext(options))
        {
            var flashcardPackService = new FlashcardPackIOService(context);
            var flashcardPack = new FlashcardPack { ID = Guid.NewGuid(), Name = "Test pack" };
            var user = new User { UserID = Guid.NewGuid() };
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
}