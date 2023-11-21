namespace SEProject.Models
{
    public class FlashcardPack
    {
        public string? Name { get; set; }
        public Guid ID { get; set; }
        public List<Flashcard> Flashcards { get; set; }

        public FlashcardPack() {}

        public FlashcardPack(string name, Guid id, List<Flashcard> flashcards)
        {
            Name = name;
            ID = id;
            Flashcards = flashcards;
        }

        public FlashcardPack CloneWithNewFlashcards(List<Flashcard> newFlashcards)
        {
            return new FlashcardPack
            (
                Name, 
                ID, 
                newFlashcards
            );
        }
    }
}