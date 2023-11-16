using SEProject.Models;
using SEProject.Events;

namespace SEProject.Services;

public interface IFlashcardPackDataHandler
{
    event EventHandler<FlashcardPackEventArgs> FlashcardPackSavedOrUpdated;
    event EventHandler<FlashcardPackEventArgs> FlashcardPackRemoved;
    void OnFlashcardPackSavedOrUpdated(FlashcardPackEventArgs e);
    void OnFlashcardPackRemoved(FlashcardPackEventArgs e);
    Task<FlashcardPack>? LoadFlashcardPackAsync(Guid ID);
    Task<List<FlashcardPack>> LoadFlashcardPacksAsync();
    Task SaveFlashcardPackAsync(FlashcardPack flashcardPack, Func<FlashcardPack, bool> validationFunction = null);
    Task RemoveFlashcardPackAsync(Guid ID);
}