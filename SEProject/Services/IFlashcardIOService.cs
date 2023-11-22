using SEProject.Models;
using SEProject.EventArguments;
using static SEProject.Services.FlashcardIOService;

namespace SEProject.Services;

public interface IFlashcardIOService
{
    event FlashcardChangedEventHandler? FlashcardChanged;
    void OnFlashcardChanged(FlashcardEventArgs e);
    Task<List<Flashcard>> LoadFlashcardsAsync(Guid packID);
    Task SaveFlashcard(Flashcard flashcard, Func<Flashcard, bool> validationFunction);
    Task RemoveFlashcard(Flashcard flashcard);
}

