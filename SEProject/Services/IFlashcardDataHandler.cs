using SEProject.Models;

namespace SEProject.Services;

public interface IFlashcardDataHandler
{
    Flashcard LoadFlashcard(string filepath);
    List<Flashcard> LoadFlashcards();
    void SaveFlashcard(Flashcard flashcard);
    void SaveFlashcards(List<Flashcard> flashcards);
    void RemoveFlashcard(Guid id);
}