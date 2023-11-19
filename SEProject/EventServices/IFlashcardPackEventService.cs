using SEProject.EventArguments;

namespace SEProject.EventServices;

public interface IFlashcardPackEventService
{
    void OnFlashcardPackChanged(object source, FlashcardPackEventArgs e);
}