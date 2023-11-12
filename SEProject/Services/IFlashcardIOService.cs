using SEProject.Models;

namespace SEProject.Services;

public interface IFlashcardIOService
{
    Task SaveFlashcard(Flashcard flashcard);
    Task RemoveFlashcard(Flashcard flashcard);
}

