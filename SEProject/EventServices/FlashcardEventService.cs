using SEProject.EventArguments;
using SEProject.Models;

namespace SEProject.EventServices;

public class FlashcardEventService : IFlashcardEventService {
    private readonly ILoggingHandler _logger;

    public FlashcardEventService(ILoggingHandler logger) {
        _logger = logger;
    }

    public void OnFlashcardChanged(object source, FlashcardEventArgs args) {
        _logger.Log(message: $"Flashcard: ID - {args.Flashcard.ID}, question - {args.Flashcard.Question} was updated. {args.Message}");
    }
}