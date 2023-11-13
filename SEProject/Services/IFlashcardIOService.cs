using SEProject.Models;

namespace SEProject.Services;

public interface IFlashcardIOService
{
    Task<List<Flashcard>> LoadFlashcardsAsync(Guid packID);
    Task SaveFlashcard(Flashcard flashcard, Func<Flashcard, bool> validationFunction);
    Task RemoveFlashcard(Flashcard flashcard);
}

