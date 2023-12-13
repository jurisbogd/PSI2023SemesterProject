using DatabaseAPI.Exceptions;
using DatabaseAPI.Models;
using DatabaseAPI.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APITests
{
    public class FlashcardServiceTests
    {
        private readonly DbContextOptions<DatabaseContext> _options;

        public FlashcardServiceTests()
        {
            var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
            optionsBuilder.UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString());
            _options = optionsBuilder.Options;
        }

        [Fact]
        public async Task SaveFlashcard_ShouldAddNewFlashcard()
        {
            // Arrange
            using var context = new DatabaseContext(_options);
            var flashcardIOService = new FlashcardIOService(context);

            var flashcard = new Flashcard { ID = Guid.NewGuid() };

            // Act
            await flashcardIOService.SaveFlashcard(flashcard, null);

            // Assert
            Assert.Equal(1, await context.Flashcards.CountAsync());
        }

        [Fact]
        public async Task SaveFlashcard_ShouldUpdateExistingFlashcard()
        {
            // Arrange
            using var context = new DatabaseContext(_options);
            var flashcardIOService = new FlashcardIOService(context);

            var flashcard = new Flashcard { ID = Guid.NewGuid() };
            await context.Flashcards.AddAsync(flashcard);
            await context.SaveChangesAsync();

            // Act
            flashcard.Question = "Updated Question";
            await flashcardIOService.SaveFlashcard(flashcard, null);

            // Assert
            Assert.Equal("Updated Question", (await context.Flashcards.FindAsync(flashcard.ID))?.Question);
        }

        [Fact]
        public async Task RemoveFlashcard_ShouldRemoveExistingFlashcard()
        {
            // Arrange
            using var context = new DatabaseContext(_options);
            var flashcardIOService = new FlashcardIOService(context);

            var flashcard = new Flashcard { ID = Guid.NewGuid() };
            await context.Flashcards.AddAsync(flashcard);
            await context.SaveChangesAsync();

            // Act
            await flashcardIOService.RemoveFlashcard(flashcard);

            // Assert
            Assert.Equal(0, await context.Flashcards.CountAsync());
        }

        [Fact]
        public async Task RemoveFlashcard_ShouldThrowExceptionForNonexistentFlashcard()
        {
            // Arrange
            using var context = new DatabaseContext(_options);
            var flashcardIOService = new FlashcardIOService(context);

            var nonExistentFlashcard = new Flashcard { ID = Guid.NewGuid() };

            // Act & Assert
            await Assert.ThrowsAsync<FlashcardNotFoundException>(() => flashcardIOService.RemoveFlashcard(nonExistentFlashcard));
        }
    }
}
