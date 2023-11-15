using SEProject.Models;

namespace SEProject.EventArguments
{
    public class FlashcardPackEventArgs : EventArgs
    {
        public FlashcardPack<Flashcard> FlashcardPack { get; }
        public string Message { get; set; }

        public FlashcardPackEventArgs(FlashcardPack<Flashcard> flashcardPack, string message = "")
        {
            Message = message;
            FlashcardPack = flashcardPack;
        }
    }
}