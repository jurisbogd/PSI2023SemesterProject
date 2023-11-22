namespace SEProject.Exceptions {
    public class FlashcardPackNotFoundException : Exception {
        public FlashcardPackNotFoundException() : base() {}
        public FlashcardPackNotFoundException(string message) : base(message) {}
        public FlashcardPackNotFoundException(string message, Exception inner) : base(message, inner) {}
    }
}