using System;
using System.Collections.Generic;

namespace SEProject.Models
{
    public class FlashcardPack
    {
        public string Name { get; set; }
        public Guid ID { get; set; }
        public List<Flashcard> Flashcards { get; set; }

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
                Name = this.Name, // Keep the same name.
                ID = this.ID, // Keep the same ID.
                Flashcards = newFlashcards // Set the new list of flashcards.
            );
        }
    }
}