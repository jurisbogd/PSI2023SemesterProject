using SEProject.Services;
using SEProject.EventArguments;

namespace SEProject.EventServices
{
    public class FlashcardEventService : IFlashcardEventService
    {
        private readonly ILoggingHandler _logger;

        public FlashcardEventService(ILoggingHandler logger)
        {
            _logger = logger;
        }
        public void OnFlashcardChanged(object source, FlashcardEventArgs e)
        {
            var logEntry = new LogEntry(
                        message: $"Flashcard: ID - {e.Flashcard.ID}, question - {e.Flashcard.Question} was updated. {e.Message}",
                        level: LogLevel.Information);
            _logger.Log(logEntry);
        }
    }
}