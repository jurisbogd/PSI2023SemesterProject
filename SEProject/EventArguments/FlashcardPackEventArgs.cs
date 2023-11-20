using SEProject.Models;

namespace SEProject.EventArguments
{
    public class FlashcardPackEventArgs : EventArgs
    {
        public FlashcardPack FlashcardPack { get; }
        public string Message { get; set; }

        public FlashcardPackEventArgs(FlashcardPack flashcardPack, string message = "")
        {
            Message = message;
            FlashcardPack = flashcardPack;
        }
    }
}