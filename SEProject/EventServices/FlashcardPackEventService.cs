using SEProject.Models;
using SEProject.EventArguments;

namespace SEProject.EventServices;

public class FlashcardPackEventService : IFlashcardPackEventService
{
    private readonly ILoggingHandler _logger;

    public FlashcardPackEventService(ILoggingHandler logger) {
        _logger = logger;
    }

    public void OnFlashcardPackChanged(object source, FlashcardPackEventArgs e) {
        _logger.Log(message: $"FlashcardPack: ID - {e.FlashcardPack.ID}, name - {e.FlashcardPack.Name} was updated. {e.Message}");
    }
}