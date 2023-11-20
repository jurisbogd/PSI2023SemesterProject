using SEProject.EventArguments;

namespace SEProject.EventServices;

public interface IFlashcardEventService
{
    void OnFlashcardChanged(object source, FlashcardEventArgs e);
}