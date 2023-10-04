using SEProject.Models;

namespace SEProject.Services;

public interface IFlashcardFileIOService
{
    Flashcard LoadFlashcard(string filepath);
    List<Flashcard> LoadFlashcards();
    void SaveFlashcard(Flashcard flashcard);
    void SaveFlashcards(List<Flashcard> flashcards);
    void RemoveFlashcard(Guid IDToRemove);
}