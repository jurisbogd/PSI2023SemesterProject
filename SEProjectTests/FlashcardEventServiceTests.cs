using Moq;
using SEProject.Models;
using SEProject.EventArguments;
using Microsoft.Extensions.Logging;
using SEProject.EventServices;

namespace SEProjectTests;

public class FlashcardEventServiceTests
{
    [Fact]
    public void OnFlashcardChanged_LogsCorrectMessageWithAdditionalMessage()
    {
        // Arrange
        var mockLogger = new Mock<ILoggingHandler>();
        var flashcardEventService = new FlashcardEventService(mockLogger.Object);

        var flashcard = new Flashcard
        {
            ID = new Guid(),
            Question = "Sample Question",
            // Set other properties as needed for your test
        };

        var eventArgs = new FlashcardEventArgs(flashcard, "Additional message");

        // Act
        flashcardEventService.OnFlashcardChanged(this, eventArgs);

        // Assert
        var expectedLogMessage = $"Flashcard: ID - {flashcard.ID}, question - {flashcard.Question} was updated. {eventArgs.Message}";
        mockLogger.Verify(logger => logger.Log(It.Is<LogEntry>(entry =>
            entry.Message == expectedLogMessage
            && entry.Level == LogLevel.Information)), Times.Once);
    }

    [Fact]
    public void OnFlashcardChanged_LogsCorrectMessageWithoutAdditionalMessage()
    {
        // Arrange
        var mockLogger = new Mock<ILoggingHandler>();
        var flashcardEventService = new FlashcardEventService(mockLogger.Object);

        var flashcard = new Flashcard
        {
            ID = new Guid(),
            Question = "Sample Question",
            // Set other properties as needed for your test
        };

        var eventArgs = new FlashcardEventArgs(flashcard);

        // Act
        flashcardEventService.OnFlashcardChanged(this, eventArgs);

        // Assert
        var expectedLogMessage = $"Flashcard: ID - {flashcard.ID}, question - {flashcard.Question} was updated. ";
        mockLogger.Verify(logger => logger.Log(It.Is<LogEntry>(entry =>
            entry.Message == expectedLogMessage
            && entry.Level == LogLevel.Information)), Times.Once);
    }

    [Fact]
    public void OnFlashcardChanged_ThrowsExceptionWithoutLogger()
    {
        // Arrange
        var flashcardEventService = new FlashcardEventService(logger: null); // No logger provided

        var flashcard = new Flashcard
        {
            ID = new Guid(),
            Question = "Sample Question",
            // Set other properties as needed for your test
        };

        var eventArgs = new FlashcardEventArgs(flashcard);

        // Act and Assert
        Assert.Throws<NullReferenceException>(() => flashcardEventService.OnFlashcardChanged(this, eventArgs));
    }
}