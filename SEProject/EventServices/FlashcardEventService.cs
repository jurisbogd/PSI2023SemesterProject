using SEProject.Services;
using SEProject.EventArguments;

namespace SEProject.EventServices;

public class FlashcardEventService : IFlashcardEventService {
    private readonly ILoggingHandler _logger;

    public FlashcardEventService(ILoggingHandler logger) {
        _logger = logger;
    }

    public void OnFlashcardChanged(object source, FlashcardEventArgs args) {
        _logger.Log(message: $"Flashcard: ID - {args.Flashcard.ID}, question - {args.Flashcard.Question} was updated. {e.Message}");
    }
}