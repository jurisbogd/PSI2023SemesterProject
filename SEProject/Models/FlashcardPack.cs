namespace SEProject.Models
{
    public class FlashcardPack<T>
    {
        public string Name { get; set; }
        public Guid ID { get; set; }
        public List<T> Flashcards { get; set; }

        public FlashcardPack()
        {

        }

        public FlashcardPack(string name, Guid id, List<T> flashcards)
        {
            Name = name;
            ID = id;
            Flashcards = flashcards;
        }

        public FlashcardPack<T> CloneWithNewFlashcards(List<T> newFlashcards)
        {
            return new FlashcardPack<T>
            (
                Name = this.Name, 
                ID = this.ID, 
                Flashcards = newFlashcards
            );
        }
    }
}