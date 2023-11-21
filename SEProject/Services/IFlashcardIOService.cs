using SEProject.Models;
using SEProject.EventArguments;

namespace SEProject.Services;

public interface IFlashcardIOService
{
    event EventHandler<FlashcardEventArgs>? FlashcardChanged;
    void OnFlashcardChanged(FlashcardEventArgs e);
    Task<List<Flashcard>> LoadFlashcardsAsync(Guid packID);
    Task SaveFlashcard(Flashcard flashcard, Func<Flashcard, bool> validationFunction);
    Task RemoveFlashcardFromPack(Guid packID, Guid flashcardID);
    public Task<Flashcard> FetchFlashcard(Guid id);
    public Task<FlashcardPack> FetchFlashcardPack(Guid id);
}

