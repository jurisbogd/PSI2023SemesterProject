namespace SEProject.Exceptions {
    public class FlashcardNotFoundException : Exception {
        public FlashcardNotFoundException() : base() {}
        public FlashcardNotFoundException(string message) : base(message) {}
        public FlashcardNotFoundException(string message, Exception inner) : base(message, inner) {}
    }
}