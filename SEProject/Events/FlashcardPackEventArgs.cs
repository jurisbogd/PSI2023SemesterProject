namespace SEProject.Events;
using SEProject.Models;

public class FlashcardPackEventArgs : EventArgs
{
    public FlashcardPack<Flashcard> FlashcardPack { get; }

    public FlashcardPackEventArgs(FlashcardPack<Flashcard> flashcardPack)
    {
        FlashcardPack = flashcardPack;
    }
}