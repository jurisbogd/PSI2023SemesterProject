using SEProject.Models;

namespace SEProject.EventArguments
{
    public class FlashcardPackEventArgs : EventArgs
    {
        public FlashcardPack<Flashcard> FlashcardPack { get; }

        public FlashcardPackEventArgs(FlashcardPack<Flashcard> flashcardPack)
        {
            FlashcardPack = flashcardPack;
        }
    }
}