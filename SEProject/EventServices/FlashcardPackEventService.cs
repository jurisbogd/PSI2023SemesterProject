using SEProject.Services;
using SEProject.EventArguments;

namespace SEProject.EventServices
{
    public class FlashcardPackEventService : IFlashcardPackEventService
    {
        private readonly ILoggingHandler _logger;

        public FlashcardPackEventService(ILoggingHandler logger)
        {
            _logger = logger;
        }
        public void OnFlashcardPackChanged(object source, FlashcardPackEventArgs e)
        {
            var logEntry = new LogEntry(
                        message: $"FlashcardPack {e.FlashcardPack.Name} changed",
                        level: LogLevel.Information);
            _logger.Log(logEntry);
        }
    }
}