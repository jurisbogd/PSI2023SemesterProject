using SEProject.Models;
using SEProject.Events;

namespace SEProject.Services;

public interface IFlashcardIOService
{
    event EventHandler<FlashcardEventArgs> FlashcardSavedOrUpdated;
    event EventHandler<FlashcardEventArgs> FlashcardRemoved;
    void OnFlashcardSavedOrUpdated(FlashcardEventArgs e);
    void OnFlashcardRemoved(FlashcardEventArgs e);
    Task<List<Flashcard>> LoadFlashcardsAsync(Guid packID);
    Task SaveFlashcard(Flashcard flashcard, Func<Flashcard, bool> validationFunction);
    Task RemoveFlashcard(Flashcard flashcard);
}

