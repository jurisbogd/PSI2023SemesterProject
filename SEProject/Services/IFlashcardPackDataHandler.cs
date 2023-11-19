using SEProject.Models;
using SEProject.EventArguments;

namespace SEProject.Services;

public interface IFlashcardPackDataHandler
{
    event EventHandler<FlashcardPackEventArgs>? FlashcardPackChanged;
    void OnFlashcardPackChanged(FlashcardPackEventArgs e);
    Task<FlashcardPack<Flashcard>>? LoadFlashcardPackAsync(Guid ID);
    Task<List<FlashcardPack<Flashcard>>> LoadFlashcardPacksAsync();
    Task SaveFlashcardPackAsync(FlashcardPack<Flashcard> flashcardPack, Func<FlashcardPack<Flashcard>, bool> validationFunction = null);
    Task RemoveFlashcardPackAsync(Guid ID);
}