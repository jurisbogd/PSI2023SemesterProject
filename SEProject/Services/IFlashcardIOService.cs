using SEProject.Models;

namespace SEProject.Services;

public interface IFlashcardIOService
{
    public void SaveFlashcard(Flashcard flashcard);
    public void RemoveFlashcard(Flashcard flashcard);
}

