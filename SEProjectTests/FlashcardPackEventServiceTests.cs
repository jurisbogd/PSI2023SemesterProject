using SEProject.EventServices;
using SEProject.Models;
using SEProject.EventArguments;
using Microsoft.Extensions.Logging;
using Moq;

namespace SEProjectTests;

public class FlashcardPackEventServiceTests
{
    [Fact]
    public void OnFlashcardPackChanged_LogsCorrectMessageWithAdditionalMessage()
    {
        // Arrange
        var mockLogger = new Mock<ILoggingHandler>();
        var flashcardPackEventService = new FlashcardPackEventService(mockLogger.Object);

        var flashcardPack = new FlashcardPack
        {
            ID = new Guid(),
            Name = "Sample Flashcard Pack",
            // Set other properties as needed for your test
        };

        var eventArgs = new FlashcardPackEventArgs(flashcardPack, "Additional message");

        // Act
        flashcardPackEventService.OnFlashcardPackChanged(this, eventArgs);

        // Assert
        var expectedLogMessage = $"FlashcardPack: ID - {flashcardPack.ID}, name - {flashcardPack.Name} was updated. {eventArgs.Message}";
        mockLogger.Verify(logger => logger.Log(It.Is<LogEntry>(entry =>
            entry.Message == expectedLogMessage
            && entry.Level == LogLevel.Information)), Times.Once);
    }

    [Fact]
    public void OnFlashcardPackChanged_LogsCorrectMessageWithoutAdditionalMessage()
    {
        // Arrange
        var mockLogger = new Mock<ILoggingHandler>();
        var flashcardPackEventService = new FlashcardPackEventService(mockLogger.Object);

        var flashcardPack = new FlashcardPack
        {
            ID = new Guid(),
            Name = "Sample Flashcard Pack",
            // Set other properties as needed for your test
        };

        var eventArgs = new FlashcardPackEventArgs(flashcardPack);

        // Act
        flashcardPackEventService.OnFlashcardPackChanged(this, eventArgs);

        // Assert
        var expectedLogMessage = $"FlashcardPack: ID - {flashcardPack.ID}, name - {flashcardPack.Name} was updated. ";
        mockLogger.Verify(logger => logger.Log(It.Is<LogEntry>(entry =>
            entry.Message == expectedLogMessage
            && entry.Level == LogLevel.Information)), Times.Once);
    }

    [Fact]
    public void OnFlashcardPackChanged_ThrowsExceptionWithoutLogger()
    {
        // Arrange
        var flashcardPackEventService = new FlashcardPackEventService(logger: null); // No logger provided

        var flashcardPack = new FlashcardPack
        {
            ID = new Guid(),
            Name = "Sample Flashcard Pack",
            // Set other properties as needed for your test
        };

        var eventArgs = new FlashcardPackEventArgs(flashcardPack);

        // Act and Assert
        Assert.Throws<NullReferenceException>(() => flashcardPackEventService.OnFlashcardPackChanged(this, eventArgs));
    }
}