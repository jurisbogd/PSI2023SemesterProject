using SEProject.Models;
using SEProject.Events;

namespace SEProject.Services;

public interface IFlashcardPackDataHandler
{
    event EventHandler<FlashcardPackEventArgs> FlashcardPackSavedOrUpdated;
    event EventHandler<FlashcardPackEventArgs> FlashcardPackRemoved;
    void OnFlashcardPackSavedOrUpdated(FlashcardPackEventArgs e);
    void OnFlashcardPackRemoved(FlashcardPackEventArgs e);
    Task<FlashcardPack<Flashcard>>? LoadFlashcardPackAsync(Guid ID);
    Task<List<FlashcardPack<Flashcard>>> LoadFlashcardPacksAsync();
    Task SaveFlashcardPackAsync(FlashcardPack<Flashcard> flashcardPack, Func<FlashcardPack<Flashcard>, bool> validationFunction = null);
    Task RemoveFlashcardPackAsync(Guid ID);
}