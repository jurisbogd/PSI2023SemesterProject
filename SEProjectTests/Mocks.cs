using Microsoft.EntityFrameworkCore;
using SEProject.Models;

namespace SEProjectTests;

public class Mocks {
    public static FlashcardPack FlashcardPack() {
        return new FlashcardPack {
            Name = "N",
            ID = Guid.NewGuid(),
            Flashcards = new List<Flashcard>()
        };
    }

    public static Flashcard Flashcard() {
        return new Flashcard {
            Question = "Q",
            Answer = "A",
            ID = Guid.NewGuid(),
            Difficulty = 0,
            PackID = Guid.NewGuid()
        };
    }

    public static Flashcard Flashcard(Guid packID) {
        return new Flashcard {
            Question = "Q",
            Answer = "A",
            ID = Guid.NewGuid(),
            Difficulty = 0,
            PackID = packID
        };
    }

    public static DatabaseContext DatabaseContext() {
        var options = new DbContextOptionsBuilder<DatabaseContext>()
            .UseInMemoryDatabase(databaseName: "MockDatabaseContext")
            .Options;

        return new DatabaseContext(options);
    }
}