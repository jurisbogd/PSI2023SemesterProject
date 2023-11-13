using SEProject.Models;

namespace SEProject.Services;

public interface IFlashcardIOService
{
    Task<List<Flashcard>> LoadFlashcardsAsync(Guid packID);
    Task SaveFlashcard(Flashcard flashcard);
    Task RemoveFlashcard(Flashcard flashcard);
}

