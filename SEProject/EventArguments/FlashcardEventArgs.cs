using SEProject.Models;

namespace SEProject.EventArguments
{
    public class FlashcardEventArgs : EventArgs
    {
        public Flashcard Flashcard { get; }
        public string Message { get; set; }

        public FlashcardEventArgs(Flashcard flashcard, string message = "")
        {
            Flashcard = flashcard;
            Message = message;
        }
    }
}