using SEProject.Models;

namespace SEProject.EventArguments
{
    public class FlashcardEventArgs : EventArgs
    {
        public Flashcard Flashcard { get; }

        public FlashcardEventArgs(Flashcard flashcard)
        {
            Flashcard = flashcard;
        }
    }
}