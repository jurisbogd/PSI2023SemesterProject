namespace SEProject.Events;
using SEProject.Models;

public class FlashcardEventArgs : EventArgs
{
    public Flashcard Flashcard { get; }

    public FlashcardEventArgs(Flashcard flashcard)
    {
        Flashcard = flashcard;
    }
}