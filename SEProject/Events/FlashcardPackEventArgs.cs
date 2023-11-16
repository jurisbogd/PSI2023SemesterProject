namespace SEProject.Events;
using SEProject.Models;

public class FlashcardPackEventArgs : EventArgs
{
    public FlashcardPack FlashcardPack { get; }

    public FlashcardPackEventArgs(FlashcardPack flashcardPack)
    {
        FlashcardPack = flashcardPack;
    }
}